using System;
using System.Linq;

using Pidgin;
using static Pidgin.Parser;
using static DiceScript.Implementation.Parsing.ExpressionParser;

using DiceScript.Implementation.SyntaxTree;
using System.Collections.Generic;

namespace DiceScript.Implementation.Parsing
{
    internal static class StatementsParser
    {
        public static Parser<char, Script> ScriptParser
        {
            get
            {
                return Parsing.StatementsParser.AnyStatement
                    .SeparatedAndOptionallyTerminated(Char(';').Then(SkipWhitespaces))
                    .Select(s => new Script
                    {
                        Statements = s.ToList()
                    });
            }
        }

        private static Parser<char, Statement> AnyStatement
        {
            get
            {
                return OneOf(
                    AssignementStmt.Cast<Statement>(),
                    ExpressionStmt.Cast<Statement>(),
                    RangeMappingStmt.Cast<Statement>());
            }
        }

        private static Parser<char, AssignementStatement> AssignementStmt
        {
            get
            {
                return Map(
                    (type, name, _, expr) => new AssignementStatement
                    {
                        Type = type,
                        VariableName = name,
                        Expression = expr
                    },
                    AssignementTypeParser.Before(Char(' ').Then(SkipWhitespaces)),
                    BaseParser.Variable,
                    SkipWhitespaces.Then(String("<-").Then(SkipWhitespaces)),
                    AnyExpression);
            }
        }

        private static Parser<char, AssignementType> AssignementTypeParser
        {
            get
            {
                var types = new Dictionary<string, AssignementType>
                {
                    { "int", AssignementType.Number },
                    { "dice", AssignementType.Dice },
                };
                return OneOf(types.Keys.Select(String)).Select(s => types[s]);
            }
        }

        private static Parser<char, ExpressionStatement> ExpressionStmt
        {
            get
            {
                return AnyExpression.Select(e => new ExpressionStatement { Expression = e });
            }
        }
        private static Parser<char, RangeMappingStatement> RangeMappingStmt
        {
            get
            {
                var ranges = BaseParser.List(Range);
                return Try(String("match "))
                    .Then(SkipWhitespaces)
                    .Then(Map(
                        (s, r) => new RangeMappingStatement { Ranges = r.ToList(), Scalar = s },
                        BaseParser.Scalar,
                        SkipWhitespaces.Then(ranges)));
            }
        }

        private static Parser<char, RangeDeclaration> Range
        {
            get
            {
                var defaultFilter = String("default")
                    .ThenReturn(new FilterOption { Type = FilterType.None });
                var rangeFilter = Try(defaultFilter).Or(OptionParser.Filter);
                var separator = Char(',').Between(SkipWhitespaces);
                return Map(
                    (f, s) => new RangeDeclaration { Filter = f, Value = s },
                    rangeFilter,
                    separator.Then(BaseParser.QuotedString))
                    .Between(Char('('), Char(')'));
            }
        }
    }
}