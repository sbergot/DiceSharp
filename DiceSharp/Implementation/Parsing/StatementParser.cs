using System;
using System.Linq;

using Pidgin;
using static Pidgin.Parser;
using static DiceSharp.Implementation.Parsing.ExpressionParser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal static class StatementParser
    {
        public static Parser<char, Statement> AnyStatement
        {
            get
            {
                return OneOf(
                    Try(AssignementStmt.Cast<Statement>()),
                    Try(ExpressionStmt.Cast<Statement>()),
                    Try(RangeMappingStmt.Cast<Statement>()));
            }
        }

        private static Parser<char, AssignementStatement> AssignementStmt
        {
            get
            {
                return String("var ")
                    .Then(SkipWhitespaces)
                    .Then(Map(
                    (name, _, expr) => new AssignementStatement { VariableName = name, Expression = expr },
                    BaseParser.Variable,
                    SkipWhitespaces.Then(String("<-").Then(SkipWhitespaces)),
                    AnyExpression
                    ));
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
                var ranges = Range
                    .Separated(Char(',').Between(SkipWhitespaces))
                    .Between(Char('('), Char(')'));
                return String("range ")
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
                var filterSubSet = OptionParser.Filter
                    .Assert(f =>
                    {
                        return f.Type != FilterType.Bottom && f.Type != FilterType.Top;
                    });
                var rangeFilter = Try(defaultFilter).Or(filterSubSet);
                var separator = Char(';').Between(SkipWhitespaces);
                return Map(
                    (s, f) => new RangeDeclaration { Filter = f, Value = s },
                    BaseParser.QuotedString,
                    separator.Then(rangeFilter)).Between(Char('('), Char(')'));
            }
        }
    }
}