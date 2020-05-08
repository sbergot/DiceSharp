using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceScript.Contracts;
using DiceScript.Implementation;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Test.TestData
{
    internal class RangeMappingTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Script, List<Result>)>
            {
            (
                "match 4 ((default,\"hello rangemap\"))",
                Helpers.RangeMapStmt(
                    new ConstantScalar { Value = 4 },
                    new List<RangeDeclaration>
                    {
                        new RangeDeclaration
                        {
                            Value = "hello rangemap",
                            Filter = new FilterOption { Type = FilterType.None }
                        }
                    }),
                new List<Result> {
                    new PrintResult
                    {
                        Value = "hello rangemap",
                    }
                }
            ),
            (
                "var $a <- roll D6;match $a ((<4, \"wont pass\"), (=5, \"will pass\"), (default, \"hello rangemap\"))",
                new Script
                {
                    Statements = new List<Statement>
                    {
                        new AssignementStatement
                        {
                            VariableName = "a",
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 6 },
                                    Number = new ConstantScalar { Value = 1 }
                                },
                                Aggregation = AggregationType.Sum
                            }
                        },
                        new RangeMappingStatement
                        {
                            Scalar = new VariableScalar { VariableName = "a" },
                            Ranges = new List<RangeDeclaration>
                            {
                                new RangeDeclaration
                                {
                                    Value = "wont pass",
                                    Filter = new FilterOption { Type = FilterType.Smaller, Scalar = new ConstantScalar { Value = 4 } }
                                },
                                new RangeDeclaration
                                {
                                    Value = "will pass",
                                    Filter = new FilterOption { Type = FilterType.Equal, Scalar = new ConstantScalar { Value = 5 } }
                                },
                                new RangeDeclaration
                                {
                                    Value = "hello rangemap",
                                    Filter = new FilterOption { Type = FilterType.None }
                                }
                            }
                        }
                    }
                },
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new PrintResult
                    {
                        Value = "will pass",
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