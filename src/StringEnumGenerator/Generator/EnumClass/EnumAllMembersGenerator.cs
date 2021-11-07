using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using StringEnumGenerator.Generator.Model;
using StringEnumGenerator.Generator.Utils;

namespace StringEnumGenerator.Generator.EnumClass
{
    internal class EnumAllMembersGenerator
    {
        public EnumAllMembersGenerator(EnumContext enumContext, IndentedTextWriter sourceWriter)
        {
            EnumContext enumContext1 = enumContext;
            _sourceWriter = sourceWriter;
            _accessibility = SyntaxFacts.GetText(enumContext1.Symbol.DeclaredAccessibility);
            _docWriter = new DocWriter(_sourceWriter);
            _enumName = enumContext1.Symbol.Name;
        }

        private readonly string _enumName;
        private readonly string _accessibility;

        private readonly IndentedTextWriter _sourceWriter;
        private readonly DocWriter _docWriter;

        public void GenerateAllMembersFunction(IReadOnlyList<string> enumMembers)
        {
            const string allMembersFieldName = "_allMembersCache";
            string allMembersFieldDeclaration = $"private static Lazy<ImmutableArray<{_enumName}>>? {allMembersFieldName};";
            _sourceWriter.WriteLine(allMembersFieldDeclaration);
            _sourceWriter.WriteLine();

            _docWriter.WriteSummary(DocumentationText.AllMembers.Summary(_enumName));
            string allMembersSignature = $"{_accessibility} static ImmutableArray<{_enumName}> AllMembers";
            _sourceWriter.WriteLine(allMembersSignature);

            using (_sourceWriter.Scope())
            {
                _sourceWriter.WriteLine("get");
                using (_sourceWriter.Scope())
                {
                    _sourceWriter.WriteLine($"if ({allMembersFieldName} == null)");

                    using (_sourceWriter.Scope())
                    {
                        _sourceWriter.WriteLine($"var newLazy = new Lazy<ImmutableArray<{_enumName}>>(() =>");


                        using (_sourceWriter.Scope())
                        {
                            _sourceWriter.WriteLine($"return ImmutableArray.Create<{_enumName}>(");
                            
                            for(int i = 0; i < enumMembers.Count; i++)
                            {
                                _sourceWriter.Write($"{_enumName}.{enumMembers[i]}");

                                if(i + 1 < enumMembers.Count)
                                {
                                    _sourceWriter.Write(",");
                                }
                                _sourceWriter.WriteLine();
                            }

                                
                            _sourceWriter.WriteLine(");");
                        }
                        _sourceWriter.WriteLine(");");
                        _sourceWriter.WriteLine($"Interlocked.CompareExchange(ref {allMembersFieldName}, newLazy, null);");
                    }

                    _sourceWriter.WriteLine($"return {allMembersFieldName}.Value;");





                }
            }
        }
    }
}
