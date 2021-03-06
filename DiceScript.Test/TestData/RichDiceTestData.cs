using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceScript.Contracts;
using DiceScript.Implementation;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Test.TestData
{
    internal class RichDiceTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Script, List<Result>)>
            {
            (
                "roll D6(=7)",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 1 }
                    },
                    Options = new OptionGroup
                    {
                        Options = new List<Option>
                        {
                            new FilterOption { Type = FilterType.Equal, Scalar = new ConstantScalar { Value = 7 } },
                        }
                    },
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = false, Result = 5, Faces = 6 } },
                        Result = 0,
                    }
                }
            ),
            (
                "roll D6 (top7)",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 1 }
                    },
                    Options = new OptionGroup
                    {
                        Options = new List<Option>
                        {
                            new RankingOption { Type = RankingType.Top, Scalar = new ConstantScalar { Value = 7 } },
                        }
                    },
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
                "roll D6 (sum)",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 1 }
                    },
                    Options = new OptionGroup
                    {
                        Options = new List<Option>
                        {
                            new AggregateOption { Type = AggregationType.Sum },
                        }
                    },
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
                "roll D6 (count)",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 1 }
                    },
                    Options = new OptionGroup
                    {
                        Options = new List<Option>
                        {
                            new AggregateOption { Type = AggregationType.Count },
                        }
                    },
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 1,
                    }
                }
            ),
            (
                "roll D6 (min)",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 1 }
                    },
                    Options = new OptionGroup
                    {
                        Options = new List<Option>
                        {
                            new AggregateOption { Type = AggregationType.Min },
                        }
                    },
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
                "roll 3D6(=4, count)",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 3 }
                    },
                    Options = new OptionGroup
                    {
                        Options = new List<Option>
                        {
                            new FilterOption { Type = FilterType.Equal, Scalar = new ConstantScalar { Value = 4 } },
                            new AggregateOption { Type = AggregationType.Count },
                        }
                    },
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 3, Bonus = 0, Exploding = false },
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                        },
                        Result = 0,
                    }
                }
            ),
            (
                "roll 3D6(top1)",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 3 }
                    },
                    Options = new OptionGroup
                    {
                        Options = new List<Option>
                        {
                            new RankingOption { Type = RankingType.Top, Scalar = new ConstantScalar { Value = 1 } },
                        }
                    },
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 3, Bonus = 0, Exploding = false },
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                        },
                        Result = 5,
                    }
                }
            ),
            (
                "roll 2D2(exp)",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 2 },
                        Number = new ConstantScalar { Value = 2 }
                    },
                    Options = new OptionGroup
                    {
                        Options = new List<Option>
                        {
                            new ExplodingOption(),
                        }
                    },
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 2, Number = 2, Bonus = 0, Exploding = true },
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 9, Faces = 2 },
                            new Dice { Valid = true, Result = 5, Faces = 2 },
                        },
                        Result = 14,
                    }
                }
            ),
            (
                "roll \"my name!\" 2D2",
                Helpers.ToAst(new DiceExpression
                {
                    Dices = new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 2 },
                        Number = new ConstantScalar { Value = 2 }
                    },
                    Name = "my name!"
                }),
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 2, Number = 2, Bonus = 0, Exploding = false },
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 2, Faces = 2 },
                            new Dice { Valid = true, Result = 2, Faces = 2 },
                        },
                        Result = 4,
                        Name = "my name!"
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