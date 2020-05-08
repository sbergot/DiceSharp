using Pidgin;
using static Pidgin.Parser;

using DiceScript.Implementation.SyntaxTree;
using System.Collections.Generic;
using System.Linq;

namespace DiceScript.Implementation.Parsing
{
    internal static class ArithmeticParser
    {
        public static Parser<char, CalcExpression> ArithmeticExpression
        {
            get
            {
                return Try(String("calc "))
                    .Then(SkipWhitespaces)
                    .Then(Map(
                        (left, op, right) => new CalcExpression
                        {
                            LeftValue = left,
                            RightValue = right,
                            Operator = op
                        },
                        BaseParser.Scalar,
                        Operation,
                        BaseParser.Scalar));
            }
        }
        public static Parser<char, SignType> Operation
        {
            get
            {
                var operationTypes = new Dictionary<string, SignType>
                {
                    { "+", SignType.Plus },
                    { "-", SignType.Minus },
                    { "*", SignType.Multiply },
                };
                return OneOf(operationTypes.Keys.Select(String))
                    .Between(SkipWhitespaces)
                    .Select(s => operationTypes[s]);
            }
        }
    }
}