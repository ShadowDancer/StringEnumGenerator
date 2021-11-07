using System;
using System.CodeDom.Compiler;

namespace StringEnumGenerator.Generator
{
    internal class DocWriter
    {
        private readonly IndentedTextWriter _sourceWriter;

        internal DocWriter(IndentedTextWriter sourceWriter)
        {
            _sourceWriter = sourceWriter;
        }

        public void WriteTagWithTextContent(string tagName, string? tagAttributes, string[] text)
        {
            using (OpenTag(tagName, tagAttributes))
            {
                foreach (var line in text)
                {
                    foreach (var segment in line.Split('\n'))
                    {
                        _sourceWriter.Write("///  ");
                        _sourceWriter.WriteLine(segment);
                    }
                }
            }
        }

        public static string Cref(string cref)
        {
            return $"<see cref=\"{cref}\"/>";
        }
        
        public void WriteSummary(string text)
        {
            WriteTagWithTextContent("summary", null, new[] { text });
        }

        public void WriteParamDescription(string param, string text)
        {
            WriteTagWithTextContent("param", $"name=\"{param}\"", new[] { text });
        }

        public void WriteReturn(string text)
        {
            WriteTagWithTextContent("return", null, new[] { text });
        }

        public void WriteException(string exceptionName, string text)
        {
            WriteTagWithTextContent("exception", "cref=\"" + (exceptionName) + "\"", new[] { text });
        }

        private IDisposable OpenTag(string tagName, string? attributes = null)
        {
            return new SummaryTag(_sourceWriter, tagName, attributes);
        }

        private class SummaryTag : IDisposable
        {
            private readonly string _tagName;
            private readonly IndentedTextWriter _sourceWriter;

            public SummaryTag(IndentedTextWriter sourceWriter, string tagName, string? attributes)
            {
                _sourceWriter = sourceWriter;
                _tagName = tagName;

                string tagContent = tagName;
                if(attributes != null)
                {
                    tagContent += " " + attributes;
                }

                _sourceWriter.WriteLine($"/// <{tagContent}>");
            }

            public void Dispose()
            {
                _sourceWriter.WriteLine($"/// </{_tagName}>");
            }
        }
    }
}
