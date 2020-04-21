using System;
using System.Collections;
using System.Collections.Generic;
using DiceSharp.Contracts;
using DiceSharp.Implementation;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Test
{
    internal class TestData
    {
        public static List<(string, Ast, List<Roll>)> GetTestData()
        {
            return new List<(string, Ast, List<Roll>)>
            {
            (
                "D6",
                Helpers.ToAst(new DiceExpression { Faces = 6, Number = 1 }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 4, Faces = 6 } },
                        Result = 4,
                    }
                }
            ),
            (
                "3D6",
                Helpers.ToAst(new DiceExpression { Faces = 6, Number = 3 }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                        },
                        Result = 13,
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
                                Dices = new DiceExpression { Faces = 8, Number = 1 }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new RichDiceExpression
                            {
                                Dices = new DiceExpression { Faces = 3, Number = 4 }
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
                            new Dice { Valid = true, Result = 2, Faces = 3 },
                            new Dice { Valid = true, Result = 2, Faces = 3 },
                            new Dice { Valid = true, Result = 2, Faces = 3 },
                            new Dice { Valid = true, Result = 1, Faces = 3 },
                        },
                        Result = 7,
                    }
                }
            ),
            (
                "3D6+2",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 3 },
                    Aggregation = null,
                    Filter = null,
                    SumBonus = new SumBonusExpression { Value = 2 }
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                        },
                        Result = 15,
                    }
                }
            ),
            (
                "3D6-2",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 3 },
                    Aggregation = null,
                    Filter = null,
                    SumBonus = new SumBonusExpression { Value = -2 }
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                        },
                        Result = 11,
                    }
                }
            ),
            (
                "D6(=7)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 1 },
                    Aggregation = null,
                    Filter = new FilterExpression { Type = FilterType.Equal, Scalar = 7 },
                    SumBonus = null
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice> { new Dice { Valid = false, Result = 4, Faces = 6 } },
                        Result = 0,
                    }
                }
            ),
            (
                "D6(top7)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 1 },
                    Aggregation = null,
                    Filter = new FilterExpression { Type = FilterType.Top, Scalar = 7 },
                    SumBonus = null
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 4, Faces = 6 } },
                        Result = 4,
                    }
                }
            ),
            (
                "D6| sum",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 1 },
                    Aggregation = new AggregateExpression { Type = AggregationType.Sum },
                    Filter = null,
                    SumBonus = null
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 4, Faces = 6 } },
                        Result = 4,
                    }
                }
            ),
            (
                "D6|count",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 1 },
                    Aggregation = new AggregateExpression { Type = AggregationType.Count },
                    Filter = null,
                    SumBonus = null
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 4, Faces = 6 } },
                        Result = 1,
                    }
                }
            ),
            (
                "D6 |min",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 1 },
                    Aggregation = new AggregateExpression { Type = AggregationType.Min },
                    Filter = null,
                    SumBonus = null
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 4, Faces = 6 } },
                        Result = 4,
                    }
                }
            ),
            (
                "3D6(=4)| count",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 3 },
                    Aggregation = new AggregateExpression { Type = AggregationType.Count },
                    Filter = new FilterExpression { Type = FilterType.Equal, Scalar = 4 },
                    SumBonus = null
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                        },
                        Result = 2,
                    }
                }
            ),
            (
                "3D6(top1)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceExpression { Faces = 6, Number = 3 },
                    Aggregation = null,
                    Filter = new FilterExpression { Type = FilterType.Top, Scalar = 1 },
                    SumBonus = null
                }),
                new List<Roll> {
                    new Roll
                    {
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = false, Result = 4, Faces = 6 },
                            new Dice { Valid = false, Result = 4, Faces = 6 },
                        },
                        Result = 5,
                    }
                }
            ),
            };
        }
    }

}