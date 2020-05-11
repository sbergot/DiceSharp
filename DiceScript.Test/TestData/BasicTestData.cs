using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceScript.Contracts;
using DiceScript.Implementation;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Test.TestData
{
    internal class BasicTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Script, List<Result>)>
            {
            (
                "roll D6",
                Helpers.ToAst(new DiceDeclaration
                {
                    Faces = new ConstantScalar { Value = 6 },
                    Number = new ConstantScalar { Value = 1 }
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    }
                }
            ),
            (
                "roll 3D6",
                Helpers.ToAst(new DiceDeclaration
                {
                    Faces = new ConstantScalar { Value = 6 },
                    Number = new ConstantScalar { Value = 3 }
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 3, Bonus = 0, Exploding = false },
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
                "roll D8;roll 4D3",
                new Script
                {
                    Statements = new List<Statement>
                    {
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 8 },
                                    Number = new ConstantScalar { Value = 1 }
                                },
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 3 },
                                    Number = new ConstantScalar { Value = 4 }
                                },
                            }
                        }
                    }
                },
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 8, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 6, Faces = 8 } },
                        Result = 6,
                    },
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 3, Number = 4, Bonus = 0, Exploding = false },
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
                "roll D8 ; roll 4D3",
                new Script
                {
                    Statements = new List<Statement>
                    {
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 8 },
                                    Number = new ConstantScalar { Value = 1 }
                                },
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 3 },
                                    Number = new ConstantScalar { Value = 4 }
                                },
                            }
                        }
                    }
                },
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 8, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 6, Faces = 8 } },
                        Result = 6,
                    },
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 3, Number = 4, Bonus = 0, Exploding = false },
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
                "roll 3D6+2",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 3 }
                    },
                    SumBonus = new SumBonusDeclaration { Scalar = new ConstantScalar { Value = 2 }, Sign = SignType.Plus }
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 3, Bonus = 2, Exploding = false },
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
                "roll 3D6-2",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 3 }
                    },
                    SumBonus = new SumBonusDeclaration { Scalar = new ConstantScalar { Value = 2 }, Sign = SignType.Minus }
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 3, Bonus = -2, Exploding = false },
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