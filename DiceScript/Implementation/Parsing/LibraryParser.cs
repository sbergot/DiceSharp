using Pidgin;
using static Pidgin.Parser;

using DiceScript.Implementation.SyntaxTree;
using System.Collections.Generic;
using System.Linq;

namespace DiceScript.Implementation.Parsing
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
                return OneOf(
                    Try(FunctionImplementationParser).Cast<FunctionDeclaration>(),
                    Try(PartialApplicationParser).Cast<FunctionDeclaration>());
            }
        }

        private static Parser<char, PartialApplicationDeclaration> PartialApplicationParser
        {
            get
            {
                return String("macro ")
                    .Then(SkipWhitespaces)
                    .Then(Map(
                        (name, args, appliedName, scalars) => new PartialApplicationDeclaration
                        {
                            Name = name,
                            Arguments = args,
                            AppliedFunction = appliedName,
                            ProvidedValues = scalars
                        },
                        BaseParser.Name,
                        Arguments,
                        String("<- apply ").Between(SkipWhitespaces).Then(BaseParser.Name),
                        ScalarList));
            }
        }

        private static Parser<char, List<Scalar>> ScalarList
        {
            get
            {
                return BaseParser.List(BaseParser.Scalar);
            }
        }

        private static Parser<char, FunctionImplementation> FunctionImplementationParser
        {
            get
            {
                return String("macro ")
                    .Then(SkipWhitespaces)
                    .Then(Map(
                        (name, args, script) => new FunctionImplementation
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
                return BaseParser.List(BaseParser.Variable);
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