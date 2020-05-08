using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceScript.Contracts;
using DiceScript.Implementation;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Test.TestData
{
    internal class AssignTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Script, List<Result>)>
            {
            (
                "var $a<-roll D6",
                Helpers.AssignStmt(
                    "a",
                    new DiceDeclaration
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
                "var $my_name_42 <-  roll D6",
                Helpers.AssignStmt(
                    "my_name_42",
                    new DiceDeclaration
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
                "var $a<-roll D6;roll 2D6(>$a,count)",
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
                                }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 6 },
                                    Number = new ConstantScalar { Value = 2 }
                                },
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
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 2, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> {
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                            new Dice { Valid = false, Result = 5, Faces = 6 },
                        },
                        Result = 0,
                    },
                }
            ),
            (
                "var $a<-roll D6;roll D6+$a",
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
                                }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 6 },
                                    Number = new ConstantScalar { Value = 1 }
                                },
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
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 5, Exploding = false },
                        Dices = new List<Dice> {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                        },
                        Result = 10,
                    },
                }
            ),
            (
                "var $a<-roll D6;roll D6-$a",
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
                                }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new ConstantScalar { Value = 6 },
                                    Number = new ConstantScalar { Value = 1 }
                                },
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
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = -5, Exploding = false },
                        Dices = new List<Dice> {
                            new Dice { Valid = true, Result = 5, Faces = 6 },
                        },
                        Result = 0,
                    },
                }
            ),
            (
                "var $a<-roll D6;roll 1D$a+$a",
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
                                }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new VariableScalar { VariableName = "a" },
                                    Number = new ConstantScalar { Value = 1 },
                                },
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
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 5, Number = 1, Bonus = 5, Exploding = false },
                        Dices = new List<Dice> {
                            new Dice { Valid = true, Result = 5, Faces = 5 },
                        },
                        Result = 10,
                    },
                }
            ),
            (
                "var $a<-roll D6;roll $a$D$a+$a",
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
                                }
                            }
                        },
                        new ExpressionStatement
                        {
                            Expression = new DiceExpression
                            {
                                Dices = new DiceDeclaration
                                {
                                    Faces = new VariableScalar { VariableName = "a" },
                                    Number = new VariableScalar { VariableName = "a" },
                                },
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
                        Description = new RollDescription { Faces = 6, Number = 1, Bonus = 0, Exploding = false },
                        Dices = new List<Dice> { new Dice { Valid = true, Result = 5, Faces = 6 } },
                        Result = 5,
                    },
                    new RollResult
                    {
                        Description = new RollDescription { Faces = 5, Number = 5, Bonus = 5, Exploding = false },
                        Dices = new List<Dice> {
                            new Dice { Valid = true, Result = 5, Faces = 5 },
                            new Dice { Valid = true, Result = 4, Faces = 5 },
                            new Dice { Valid = true, Result = 3, Faces = 5 },
                            new Dice { Valid = true, Result = 2, Faces = 5 },
                            new Dice { Valid = true, Result = 3, Faces = 5 },
                        },
                        Result = 22,
                    },
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