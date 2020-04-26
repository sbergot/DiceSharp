using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using DiceSharp.Implementation;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Test.TestData
{
    internal class RichDiceTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Ast, List<Roll>)>
            {
            (
                "D6(=7)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceDeclaration { Faces = 6, Number = 1 },
                    Aggregation = AggregationType.Sum,
                    Filter = new FilterOption { Type = FilterType.Equal, Scalar = new ConstantScalar { Value = 7 } },
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
                "D6 (top7)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceDeclaration { Faces = 6, Number = 1 },
                    Aggregation = AggregationType.Sum,
                    Filter = new FilterOption { Type = FilterType.Top, Scalar = new ConstantScalar { Value = 7 } },
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
                "D6 (sum)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceDeclaration { Faces = 6, Number = 1 },
                    Aggregation = AggregationType.Sum,
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
                "D6 (count)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceDeclaration { Faces = 6, Number = 1 },
                    Aggregation = AggregationType.Count,
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
                "D6 (min)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceDeclaration { Faces = 6, Number = 1 },
                    Aggregation = AggregationType.Min,
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
                "3D6(=4, count)",
                Helpers.ToAst(new RichDiceExpression
                {
                    Dices = new DiceDeclaration { Faces = 6, Number = 3 },
                    Aggregation = AggregationType.Count,
                    Filter = new FilterOption { Type = FilterType.Equal, Scalar = new ConstantScalar { Value = 4 } },
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
                    Dices = new DiceDeclaration { Faces = 6, Number = 3 },
                    Aggregation = AggregationType.Sum,
                    Filter = new FilterOption { Type = FilterType.Top, Scalar = new ConstantScalar { Value = 1 } },
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