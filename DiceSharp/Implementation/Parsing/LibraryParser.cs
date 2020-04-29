using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;
using System.Collections.Generic;
using System.Linq;

namespace DiceSharp.Implementation.Parsing
{
    internal static class LibraryParser
    {
        public static Parser<char, LibraryTree> LibraryBlock
        {
            get
            {
                return FunctionParser
                    .SeparatedAndOptionallyTerminated(SkipWhitespaces)
                    .Between(SkipWhitespaces)
                    .Select(f => new LibraryTree { Functions = f.ToList() });
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
                            Arguments = args,
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