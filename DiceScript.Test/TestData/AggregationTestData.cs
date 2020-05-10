using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceScript.Contracts;
using DiceScript.Implementation;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Test.TestData
{
    internal class AggregationTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Script, List<Result>)>
            {
            (
                "dice $a<-roll D6",
                Helpers.AssignStmt(
                    "a",
                    new DiceDeclaration
                    {
                        Faces = new ConstantScalar { Value = 6 },
                        Number = new ConstantScalar { Value = 1 }
                    },
                    AssignementType.Dice),
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
                "dice $a<-roll D6; aggregate $a (sum)",
                new Script
                {
                    Statements = new List<Statement>
                    {
                        new AssignementStatement
                        {
                            VariableName = "a",
                            Type = AssignementType.Dice,
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 6 },
                                    Number = new ConstantScalar { Value = 1 }
                                }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new AggregationExpression
                            {
                                Variable = new VariableScalar { VariableName = "a" },
                                Aggregation = AggregationType.Sum
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
                    new ValueResult { Result = 5 }
                }
            ),
            (
                @"dice $a<- roll 8D6 (exp);
                int $successes <- aggregate $a (>4, count);
                int $failures <- aggregate $a (<3, count);
                calc $successes - $failures",
                new Script
                {
                    Statements = new List<Statement>
                    {
                        new AssignementStatement
                        {
                            VariableName = "a",
                            Type = AssignementType.Dice,
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 6 },
                                    Number = new ConstantScalar { Value = 8 }
                                },
                                Exploding = true
                            }
                        },
                        new AssignementStatement
                        {
                            VariableName = "successes",
                            Expression = new AggregationExpression
                            {
                                Variable = new VariableScalar { VariableName = "a" },
                                Aggregation = AggregationType.Count,
                                Filter = new FilterOption
                                {
                                    Type = FilterType.Larger,
                                    Scalar = new ConstantScalar { Value = 4 }
                                }
                            }
                        },
                        new AssignementStatement
                        {
                            VariableName = "failures",
                            Expression = new AggregationExpression
                            {
                                Variable = new VariableScalar { VariableName = "a" },
                                Aggregation = AggregationType.Count,
                                Filter = new FilterOption
                                {
                                    Type = FilterType.Smaller,
                                    Scalar = new ConstantScalar { Value = 3 }
                                }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new CalcExpression
                            {
                                LeftValue = new VariableScalar { VariableName = "successes" },
                                RightValue = new VariableScalar { VariableName = "failures" },
                                Operator = SignType.Minus
                            }
                        }

                    }
                },
                new List<Result> {
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 8, Bonus = 0, Exploding = true },
                        Dices = new List<Dice>
                        {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                            new Dice { Valid = true, Result = 2, Faces = 6 },
                            new Dice { Valid = true, Result = 4, Faces = 6 },
                            new Dice { Valid = true, Result = 9, Faces = 6 },
                            new Dice { Valid = true, Result = 8, Faces = 6 },
                        },
                        Result = 42,
                    },
                    new ValueResult { Result = 5 },
                    new ValueResult { Result = 1 },
                    new ValueResult { Result = 4 },
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