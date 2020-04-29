using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;
using System.Collections.Generic;
using System.Linq;

namespace DiceSharp.Implementation.Parsing
{
    internal static class LibraryParser
    {
        public static Parser<char, Library> LibraryBlock
        {
            get
            {
                return FunctionParser
                    .Separated(Whitespace.Then(SkipWhitespaces))
                    .Select(f => new Library { Functions = f.ToList() });
            }
        }

        private static Parser<char, FunctionDeclaration> FunctionParser
        {
            get
            {
                return String("function ")
                    .Then(SkipWhitespaces)
                    .Then(Map(
                        (name, args, script) => new FunctionDeclaration
                        {
                            Name = name,
                            Variables = args,
                            Script = script
                        },
                        BaseParser.Name,
                        Arguments,
                        SkipWhitespaces.Then(ScriptBlock)));
            }
        }

        private static Parser<char, List<string>> Arguments
        {
            get
            {
                return BaseParser.Variable
                    .Separated(Char(',').Between(SkipWhitespaces))
                    .Between(Char('('), Char(')'))
                    .Select(s => s.ToList());
            }
        }

        private static Parser<char, Script> ScriptBlock
        {
            get
            {
                return StatementsParser.ScriptParser
                    .Between(
                        Char('{').Then(SkipWhitespaces),
                        SkipWhitespaces.Then(Char('}')));
            }
        }
    }
}