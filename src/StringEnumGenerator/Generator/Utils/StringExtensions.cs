using Microsoft.CodeAnalysis.CSharp;

namespace StringEnumGenerator.Generator.Utils
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts string to literal which may be embedded into c# source
        /// </summary>
        public static string ToLiteral(this string input)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(input)).ToFullString();
        }
    }
}
