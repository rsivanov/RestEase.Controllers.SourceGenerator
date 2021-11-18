using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace RestEase.Controllers.SourceGenerator
{
	internal class RoutingAttributesAnalyzer
	{
		private readonly INamedTypeSymbol _routeAttributeSymbol;
		private readonly INamedTypeSymbol _getAttributeSymbol;
		private readonly INamedTypeSymbol _postAttributeSymbol;
		private readonly INamedTypeSymbol _putAttributeSymbol;
		private readonly INamedTypeSymbol _deleteAttributeSymbol;
		private readonly INamedTypeSymbol _patchAttributeSymbol;
		private readonly INamedTypeSymbol _optionsAttributeSymbol;
		private readonly INamedTypeSymbol _headAttributeSymbol;
		private readonly INamedTypeSymbol _fromBodyAttributeSymbol;
		private readonly INamedTypeSymbol _fromHeaderAttributeSymbol;
		private readonly INamedTypeSymbol _fromQueryAttributeSymbol;
		private readonly INamedTypeSymbol _fromRouteAttributeSymbol;

		public RoutingAttributesAnalyzer(GeneratorExecutionContext context)
		{
			_routeAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.RouteAttribute");
			_getAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.HttpGetAttribute");
			_postAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.HttpPostAttribute");
			_putAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.HttpPutAttribute");
			_deleteAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.HttpDeleteAttribute");
			_patchAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.HttpPatchAttribute");
			_optionsAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.HttpOptionsAttribute");
			_headAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.HttpHeadAttribute");
			_fromBodyAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.FromBodyAttribute");
			_fromHeaderAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.FromHeaderAttribute");
			_fromQueryAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.FromQueryAttribute");
			_fromRouteAttributeSymbol = context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Mvc.FromRouteAttribute");
		}

		public string GetBasePathAttribute(INamedTypeSymbol controllerClassSymbol)
		{
			var routeAttributeData = controllerClassSymbol.GetAttributes().FirstOrDefault(a => _routeAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (routeAttributeData != null)
			{
				var controllerNamePrefix = controllerClassSymbol.Name.Substring(0, controllerClassSymbol.Name.Length - "Controller".Length);
				return $"[BasePath(\"{ routeAttributeData.ConstructorArguments[0].Value.ToString().Replace("[controller]", controllerNamePrefix)}\")]";
			}
			return null;
		}

		public string GetGetMethodAttribute(IMethodSymbol methodSymbol, out string route)
		{
			route = null;
			var getAttributeData = methodSymbol.GetAttributes().FirstOrDefault(a => _getAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (getAttributeData != null)
			{
				if (getAttributeData.ConstructorArguments.Length > 0)
				{
					route = getAttributeData.ConstructorArguments[0].Value.ToString();
					return $"[Get(\"{route}\")]";
				}
				return $"[Get]";
			}
			return null;
		}

		public string GetPostMethodAttribute(IMethodSymbol methodSymbol, out string route)
		{
			route = null;
			var postAttributeData = methodSymbol.GetAttributes().FirstOrDefault(a => _postAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (postAttributeData != null)
			{
				if (postAttributeData.ConstructorArguments.Length > 0)
				{
					route = postAttributeData.ConstructorArguments[0].Value.ToString();
					return $"[Post(\"{route}\")]";
				}
				return $"[Post]";
			}
			return null;
		}

		public string GetPutMethodAttribute(IMethodSymbol methodSymbol, out string route)
		{
			route = null;
			var putAttributeData = methodSymbol.GetAttributes().FirstOrDefault(a => _putAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (putAttributeData != null)
			{
				if (putAttributeData.ConstructorArguments.Length > 0)
				{
					route = putAttributeData.ConstructorArguments[0].Value.ToString();
					return $"[Put(\"{route}\")]";
				}
				return $"[Put]";
			}
			return null;
		}

		public string GetDeleteMethodAttribute(IMethodSymbol methodSymbol, out string route)
		{
			route = null;
			var deleteAttributeData = methodSymbol.GetAttributes().FirstOrDefault(a => _deleteAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (deleteAttributeData != null)
			{
				if (deleteAttributeData.ConstructorArguments.Length > 0)
				{
					route = deleteAttributeData.ConstructorArguments[0].Value.ToString();
					return $"[Delete(\"{route}\")]";
				}
				return $"[Delete]";
			}
			return null;
		}

		public string GetPatchMethodAttribute(IMethodSymbol methodSymbol, out string route)
		{
			route = null;
			var patchAttributeData = methodSymbol.GetAttributes().FirstOrDefault(a => _patchAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (patchAttributeData != null)
			{
				if (patchAttributeData.ConstructorArguments.Length > 0)
				{
					route = patchAttributeData.ConstructorArguments[0].Value.ToString();
					return $"[Patch(\"{route}\")]";
				}
				return $"[Patch]";
			}
			return null;
		}

		public string GetOptionsMethodAttribute(IMethodSymbol methodSymbol, out string route)
		{
			route = null;
			var optionsAttributeData = methodSymbol.GetAttributes().FirstOrDefault(a => _optionsAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (optionsAttributeData != null)
			{
				if (optionsAttributeData.ConstructorArguments.Length > 0)
				{
					route = optionsAttributeData.ConstructorArguments[0].Value.ToString();
					return $"[Options(\"{route}\")]";
				}
				return $"[Options]";
			}
			return null;
		}

		public string GetHeadMethodAttribute(IMethodSymbol methodSymbol, out string route)
		{
			route = null;
			var headAttributeData = methodSymbol.GetAttributes().FirstOrDefault(a => _headAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (headAttributeData != null)
			{
				if (headAttributeData.ConstructorArguments.Length > 0)
				{
					route = headAttributeData.ConstructorArguments[0].Value.ToString();
					return $"[Head(\"{route}\")]";
				}
				return $"[Head]";
			}
			return null;
		}

		public string GetBodyParameterAttribute(IParameterSymbol parameterSymbol)
		{
			var bodyAttributeData = parameterSymbol.GetAttributes().FirstOrDefault(a => _fromBodyAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (bodyAttributeData != null)
				return $"[Body]";

			return null;
		}

		public string GetHeaderParameterAttribute(IParameterSymbol parameterSymbol)
		{
			var headerAttributeData = parameterSymbol.GetAttributes().FirstOrDefault(a => _fromHeaderAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (headerAttributeData != null)
			{
				var headerName = headerAttributeData.NamedArguments.Where(na => na.Key == "Name").Select(na => na.Value.ToCSharpString()).FirstOrDefault();
				if (headerName != null)
					return $"[Header({headerName})]";
			}
			return null;
		}

		public string GetQueryParameterAttribute(IParameterSymbol parameterSymbol)
		{
			var queryAttributeData = parameterSymbol.GetAttributes().FirstOrDefault(a => _fromQueryAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (queryAttributeData != null)
			{
				var queryParamName = queryAttributeData.NamedArguments.Where(na => na.Key == "Name").Select(na => na.Value.ToCSharpString()).FirstOrDefault();
				if (queryParamName != null)
					return $"[Query({queryParamName}, QuerySerializationMethod.Serialized)]";

				return "[Query(QuerySerializationMethod.Serialized)]";
			}
			return null;
		}

		public string GetPathParameterAttribute(IParameterSymbol parameterSymbol, string route)
		{
			if (route == null)
				return null;
			if (route.Contains($"{{{parameterSymbol.Name}}}"))
				return "[Path]";

			var routeAttributeData = parameterSymbol.GetAttributes().FirstOrDefault(a => _fromRouteAttributeSymbol.Equals(a.AttributeClass, SymbolEqualityComparer.Default));
			if (routeAttributeData != null)
			{
				var routeParamName = routeAttributeData.NamedArguments.Where(na => na.Key == "Name").Select(na => na.Value.ToCSharpString()).FirstOrDefault();
				if (routeParamName != null && route.Contains($"{{{routeParamName.Trim('"')}}}"))
					return $"[Path({routeParamName})]";
			}
			return null;
		}

	}
}
