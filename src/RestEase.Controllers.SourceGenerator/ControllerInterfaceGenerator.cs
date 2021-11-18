//#define LAUNCH_DEBUGGER
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestEase.Controllers.SourceGenerator
{
	[Generator]
	public class ControllerInterfaceGenerator : ISourceGenerator
	{
		class SyntaxReceiver : ISyntaxReceiver
		{
			public List<ClassDeclarationSyntax> Controllers { get; } = new List<ClassDeclarationSyntax>();

			public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
			{
				//For the sake of simplicity we only process public classes with Controller name postfix
				if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
					classDeclarationSyntax.Modifiers.Any(m => m.ValueText == "public") &&
					 classDeclarationSyntax.Identifier.ValueText.EndsWith("Controller"))
				{
					Controllers.Add(classDeclarationSyntax);
				}
			}
		}

		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
				return;

			var baseControllerSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.ControllerBase");
			var routingAttributesAnalyzer = new RoutingAttributesAnalyzer(context);

			foreach (var controllerDeclaration in receiver.Controllers)
			{
				var semanticModel = context.Compilation.GetSemanticModel(controllerDeclaration.SyntaxTree);
				var namedTypeSymbol = semanticModel.GetDeclaredSymbol(controllerDeclaration) as INamedTypeSymbol;
				if (namedTypeSymbol.InheritsFrom(baseControllerSymbol))
				{
					ProcessController(context, namedTypeSymbol, routingAttributesAnalyzer);
				}
			}
		}

		private void ProcessController(GeneratorExecutionContext context, INamedTypeSymbol controllerClassSymbol, RoutingAttributesAnalyzer routingAttributesAnalyzer)
		{
			var namespaceName = controllerClassSymbol.ContainingNamespace.ToDisplayString();
			var interfaceName = $"I{controllerClassSymbol.Name}";

			var basePathAttribute = routingAttributesAnalyzer.GetBasePathAttribute(controllerClassSymbol);

			var interfaceSourceBuilder = new StringBuilder($@"
using RestEase;

namespace {namespaceName}
{{
	{ basePathAttribute ?? "" }
	public interface {interfaceName}
	{{
");
			foreach (var methodSymbol in controllerClassSymbol.GetMembers().OfType<IMethodSymbol>().Where(m => m.MethodKind == MethodKind.Ordinary && m.DeclaredAccessibility == Accessibility.Public))
			{
				ProcessMethod(methodSymbol, routingAttributesAnalyzer, interfaceSourceBuilder);
			}
			interfaceSourceBuilder.Append(@"
	} 
}");
#if DEBUG && LAUNCH_DEBUGGER
			System.Diagnostics.Debugger.Launch();
#endif
			context.AddSource($"{interfaceName}.generated.cs", SourceText.From(interfaceSourceBuilder.ToString(), Encoding.UTF8));
		}

		private void ProcessMethod(IMethodSymbol methodSymbol, RoutingAttributesAnalyzer routingAttributesAnalyzer, StringBuilder methodSourceBuilder)
		{
			string route;
			var requestAttribute = routingAttributesAnalyzer.GetGetMethodAttribute(methodSymbol, out route);
			if (requestAttribute == null)
				requestAttribute = routingAttributesAnalyzer.GetPostMethodAttribute(methodSymbol, out route);
			if (requestAttribute == null)
				requestAttribute = routingAttributesAnalyzer.GetPutMethodAttribute(methodSymbol, out route);
			if (requestAttribute == null)
				requestAttribute = routingAttributesAnalyzer.GetDeleteMethodAttribute(methodSymbol, out route);
			if (requestAttribute == null)
				requestAttribute = routingAttributesAnalyzer.GetPatchMethodAttribute(methodSymbol, out route);
			if (requestAttribute == null)
				requestAttribute = routingAttributesAnalyzer.GetOptionsMethodAttribute(methodSymbol, out route);
			if (requestAttribute == null)
				requestAttribute = routingAttributesAnalyzer.GetHeadMethodAttribute(methodSymbol, out route);

			if (requestAttribute != null)
			{
				var returnType = methodSymbol.ReturnType.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat);
				methodSourceBuilder.Append($@"
		{requestAttribute ?? ""}
		{returnType} {methodSymbol.Name}(");

				var delimeter = "";
				foreach (var parameterSymbol in methodSymbol.Parameters)
				{
					methodSourceBuilder.Append(delimeter);
					ProcessMethodParameter(parameterSymbol, route, routingAttributesAnalyzer, methodSourceBuilder);
					delimeter = ", ";
				}
				methodSourceBuilder.AppendLine(");");
			}
		}

		private void ProcessMethodParameter(IParameterSymbol parameterSymbol, string route, RoutingAttributesAnalyzer routingAttributesAnalyzer, StringBuilder parameterSourceBuilder)
		{
			var bodyParameterAttribute = routingAttributesAnalyzer.GetBodyParameterAttribute(parameterSymbol);
			var headerParameterAttribute = routingAttributesAnalyzer.GetHeaderParameterAttribute(parameterSymbol);
			var queryParameterAttribute = routingAttributesAnalyzer.GetQueryParameterAttribute(parameterSymbol);
			var pathParameterAttribute = routingAttributesAnalyzer.GetPathParameterAttribute(parameterSymbol, route);

			if (bodyParameterAttribute != null)
				parameterSourceBuilder.Append(bodyParameterAttribute).Append(" ");
			else if (headerParameterAttribute != null)
				parameterSourceBuilder.Append(headerParameterAttribute).Append(" ");
			else if (queryParameterAttribute != null)
				parameterSourceBuilder.Append(queryParameterAttribute).Append(" ");
			else if (pathParameterAttribute != null)
				parameterSourceBuilder.Append(pathParameterAttribute).Append(" ");

			parameterSourceBuilder.Append(parameterSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
				.Append(" ").Append(parameterSymbol.Name);
		}
	}
}
