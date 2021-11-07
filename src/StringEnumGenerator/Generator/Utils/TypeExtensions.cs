using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace StringEnumGenerator.Generator.Utils
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Returns array containing all parents, starting from closest ending at top-level.
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <returns></returns>
        public static ImmutableArray<INamedTypeSymbol> GetContainingTypes(this INamedTypeSymbol typeSymbol)
        {
            var current = typeSymbol;
            List<INamedTypeSymbol> containingTypes = new();

            while (true)
            {
                var owning = current.ContainingType;
                current = owning;
                if (current == null)
                {
                    break;
                }

                containingTypes.Add(current);
            }

            return containingTypes.ToImmutableArray();
        }
    }
}
