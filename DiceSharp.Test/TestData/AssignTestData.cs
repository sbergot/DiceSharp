using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using DiceSharp.Implementation;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Test.TestData
{
    internal class AssignTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Ast, List<Result>)>
            {
            (
                "var $a<-D6",
                Helpers.AssignStmt("a", new DiceDeclaration { Faces = 6, Number = 1 }),
                new List<Result> {
                    new RollResult
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    }
                }
            ),
            (
                "var $my_name_42 <-  D6",
                Helpers.AssignStmt("my_name_42", new DiceDeclaration { Faces = 6, Number = 1 }),
                new List<Result> {
                    new RollResult
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    }
                }
            ),
            (
                "var $a<-D6\n2D6(>$a,count)",
                new Ast
                {
                    Statements = new List<Statement>
                    {
                        new AssignementStatement
                        {
                            VariableName = "a",
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceDeclaration { Faces = 6, Number = 1 }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceDeclaration { Faces = 6, Number = 2 },
                                Filter = new FilterOption
                                {
                                    Type = FilterType.Larger,
                                    Scalar = new VariableScalar { VariableName = "a" }
                                },
                                Aggregation = AggregationType.Count
                            }
                        }
                    }
                },
                new List<Result> {
                    new RollResult
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new RollResult
                    {
                        Dices = new List<Dice> {
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                        },
                        Result = 0,
                    },
                }
            ),
            (
                "var $a<-D6\nD6+$a",
                new Ast
                {
                    Statements = new List<Statement>
                    {
                        new AssignementStatement
                        {
                            VariableName = "a",
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceDeclaration { Faces = 6, Number = 1 }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceDeclaration { Faces = 6, Number = 1 },
                                SumBonus = new SumBonusDeclaration {
                                    Scalar = new VariableScalar { VariableName = "a" },
                                    Sign = SignType.Plus
                                }
                            }
                        }
                    }
                },
                new List<Result> {
                    new RollResult
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new RollResult
                    {
                        Dices = new List<Dice> {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                        },
                        Result = 10,
                    },
                }
            ),
            (
                "var $a<-D6\nD6-$a",
                new Ast
                {
                    Statements = new List<Statement>
                    {
                        new AssignementStatement
                        {
                            VariableName = "a",
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceDeclaration { Faces = 6, Number = 1 }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceDeclaration { Faces = 6, Number = 1 },
                                SumBonus = new SumBonusDeclaration {
                                    Scalar = new VariableScalar { VariableName = "a" },
                                    Sign = SignType.Minus
                                }
                            }
                        }
                    }
                },
                new List<Result> {
                    new RollResult
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new RollResult
                    {
                        Dices = new List<Dice> {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                        },
                        Result = 0,
                    },
                }
            ),
            }
            .Select(t => new TestVector { Program = t.Item1, Ast = t.Item2, Results = t.Item3 })
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