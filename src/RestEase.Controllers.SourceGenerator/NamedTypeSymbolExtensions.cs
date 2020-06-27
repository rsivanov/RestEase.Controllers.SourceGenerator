using Microsoft.CodeAnalysis;

namespace RestEase.Controllers.SourceGenerator
{
	internal static class NamedTypeSymbolExtensions
	{
		public static bool InheritsFrom(this INamedTypeSymbol symbol, INamedTypeSymbol type)
		{
			var baseType = symbol.BaseType;
			while (baseType != null)
			{
				if (type.Equals(baseType, SymbolEqualityComparer.Default))
					return true;

				baseType = baseType.BaseType;
			}

			return false;
		}
	}
}
