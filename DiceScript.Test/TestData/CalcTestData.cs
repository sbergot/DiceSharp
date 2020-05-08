using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceScript.Contracts;
using DiceScript.Implementation;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Test.TestData
{
    internal class CalcTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Script, List<Result>)>
            {
            (
                "calc 1 + 2",
                Helpers.ToAst(new CalcExpression
                {
                    LeftValue = new ConstantScalar { Value = 1 },
                    RightValue = new ConstantScalar { Value = 2 },
                    Operator = SignType.Plus
                }),
                new List<Result> {
                    new ValueResult
                    {
                        Result = 3,
                    }
                }
            ),
            (
                "calc 2 - 1",
                Helpers.ToAst(new CalcExpression
                {
                    LeftValue = new ConstantScalar { Value = 2 },
                    RightValue = new ConstantScalar { Value = 1 },
                    Operator = SignType.Minus
                }),
                new List<Result> {
                    new ValueResult
                    {
                        Result = 1,
                    }
                }
            ),
            (
                "calc 2 * 3",
                Helpers.ToAst(new CalcExpression
                {
                    LeftValue = new ConstantScalar { Value = 2 },
                    RightValue = new ConstantScalar { Value = 3 },
                    Operator = SignType.Multiply
                }),
                new List<Result> {
                    new ValueResult
                    {
                        Result = 6,
                    }
                }
            ),
            (
                "int $a <- calc 2 * 3; calc $a + $a",
                new Script
                {
                    Statements = new List<Statement>
                    {
                        new AssignementStatement
                        {
                            Expression = new CalcExpression
                            {
                                LeftValue = new ConstantScalar { Value = 2 },
                                RightValue = new ConstantScalar { Value = 3 },
                                Operator = SignType.Multiply
                            },
                            VariableName = "a"
                        },
                        new ExpressionStatement
                        {
                            Expression = new CalcExpression
                            {
                                LeftValue = new VariableScalar { VariableName = "a" },
                                RightValue = new VariableScalar { VariableName = "a" },
                                Operator = SignType.Plus
                            }
                        }
                    }
                },
                new List<Result> {
                    new ValueResult
                    {
                        Result = 6,
                    },
                    new ValueResult
                    {
                        Result = 12
                    }
                }
            ),
            }
            .Select(t => new TestVector { Program = t.Item1, Script = t.Item2, Results = t.Item3 })
            .ToList();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return GetTestData()
                .Select(t => new object[] { t })
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetTestData()
                .Select(t => new object[] { t })
                .GetEnumerator();
        }
    }

}