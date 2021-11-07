using System;
using System.CodeDom.Compiler;

namespace StringEnumGenerator.Generator.Utils
{
    internal class Scope : IDisposable
    {
        private readonly IndentedTextWriter _indentedTextWriter;
        private readonly bool _expression;

        public Scope(IndentedTextWriter indentedTextWriter, bool expression = false)
        {
            _indentedTextWriter = indentedTextWriter;
            _expression = expression;
            _indentedTextWriter.WriteLine("{");
            _indentedTextWriter.Indent++;
        }

        public void Dispose()
        {
            _indentedTextWriter.Indent--;

            if (_expression)
            {
                _indentedTextWriter.WriteLine("};");
            }
            else
            {
                _indentedTextWriter.WriteLine("}");
            }
            
        }
    }

    internal static class ScopeExtensions
    {
        public static IDisposable Scope(this IndentedTextWriter writer)
        {
            return new Scope(writer);
        }
        public static IDisposable ExpressionScope(this IndentedTextWriter writer)
        {
            return new Scope(writer, true);
        }
    }
}
