using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using DiceSharp.Implementation;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Test.TestData
{
    internal class BasicTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Ast, List<Roll>)>
            {
            (
                "D6",
                Helpers.ToAst(new DiceDeclaration { Faces = 6, Number = 1 }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    }
                }
            ),
            (
                "3D6",
                Helpers.ToAst(new DiceDeclaration { Faces = 6, Number = 3 }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                        },
                        Result = 15,
                    }
                }
            ),
            (
                "D8\n4D3",
                new Ast
                {
                    Statements = new List<Statement>
                    {
                        new ExpressionStatement
                        {
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceDeclaration { Faces = 8, Number = 1 }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceDeclaration { Faces = 3, Number = 4 }
                            }
                        }
                    }
                },
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 6, Faces = 8 } },
                        Result = 6,
                    },
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 3, Faces = 3 },
                            new Dice { Valid = true, Result = 3, Faces = 3 },
                            new Dice { Valid = true, Result = 2, Faces = 3 },
                            new Dice { Valid = true, Result = 1, Faces = 3 },
                        },
                        Result = 9,
                    }
                }
            ),
            (
                "3D6+2",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceDeclaration { Faces = 6, Number = 3 },
                    Aggregation = AggregationType.Sum,
                    Filter = null,
                    SumBonus = new SumBonusDeclaration { Scalar = new ConstantScalar { Value = 2 }, Sign = SignType.Plus }
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                        },
                        Result = 17,
                    }
                }
            ),
            (
                "3D6-2",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceDeclaration { Faces = 6, Number = 3 },
                    Aggregation = AggregationType.Sum,
                    Filter = null,
                    SumBonus = new SumBonusDeclaration { Scalar = new ConstantScalar { Value = 2 }, Sign = SignType.Minus }
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                        },
                        Result = 13,
                    }
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