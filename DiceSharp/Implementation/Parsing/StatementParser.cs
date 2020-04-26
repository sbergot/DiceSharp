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
                    Try(ExpressionStmt.Cast<Statement>()));
            }
        }

        private static Parser<char, AssignementStatement> AssignementStmt
        {
            get
            {
                return Map(
                    (name, _, expr) => new AssignementStatement { VariableName = name, Expression = expr },
                    BaseParser.Variable,
                    SkipWhitespaces.Then(String("<-").Then(SkipWhitespaces)),
                    AnyExpression
                    );
            }
        }

        private static Parser<char, ExpressionStatement> ExpressionStmt
        {
            get
            {
                return AnyExpression.Select(e => new ExpressionStatement { Expression = e });
            }
        }

    }
}