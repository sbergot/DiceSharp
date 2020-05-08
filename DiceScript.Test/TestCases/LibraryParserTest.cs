using System.Collections.Generic;
using DiceScript.Implementation;
using DiceScript.Implementation.Parsing;
using DiceScript.Implementation.SyntaxTree;
using Xunit;

namespace DiceScript.Test.TestCases
{
    public class LibraryParserTest
    {
        [Fact]
        public void TestSingleFunctionParsing()
        {
            var parser = new Parser();
            var libast = parser.ParseLibrary("macro signledice($faces) { roll D6+$faces }");
            var expectedScript = new Script
            {
                Statements = new List<Statement>
                {
                    new ExpressionStatement
                    {
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration {
                                Faces = new ConstantScalar { Value = 6 },
                                Number = new ConstantScalar { Value = 1 }
                            },
                            Aggregation = AggregationType.Sum,
                            SumBonus = new SumBonusDeclaration
                            {
                                Scalar = new VariableScalar { VariableName = "faces" },
                                Sign = SignType.Plus
                            }
                        }
                    }
                }
            };
            var expectedLibAst = new LibraryTree
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionImplementation
                    {
                        Name = "signledice",
                        Arguments = new List<string> { "faces" },
                        Script = expectedScript,
                    }
                }
            };
            Helpers.CompareObjects(expectedLibAst, libast);
        }

        [Fact]
        public void TestSingleFunctionNoArgsParsing()
        {
            var parser = new Parser();
            var libast = parser.ParseLibrary("macro signledice() { roll D6 }");
            var expectedScript = new Script
            {
                Statements = new List<Statement>
                {
                    new ExpressionStatement
                    {
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration
                            {
                                Faces = new ConstantScalar { Value = 6 },
                                Number = new ConstantScalar { Value = 1 }
                            },
                            Aggregation = AggregationType.Sum,
                        }
                    }
                }
            };
            var expectedLibAst = new LibraryTree
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionImplementation
                    {
                        Name = "signledice",
                        Arguments = new List<string>(),
                        Script = expectedScript,
                    }
                }
            };
            Helpers.CompareObjects(expectedLibAst, libast);
        }


        [Fact]
        public void TestSingleFunctionMultipleArgsParsing()
        {
            var parser = new Parser();
            var libast = parser.ParseLibrary("macro signledice($arg1, $arg2) { roll D6 }");
            var expectedScript = new Script
            {
                Statements = new List<Statement>
                {
                    new ExpressionStatement
                    {
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration
                            {
                                Faces = new ConstantScalar { Value = 6 },
                                Number = new ConstantScalar { Value = 1 }
                            },
                            Aggregation = AggregationType.Sum,
                        }
                    }
                }
            };
            var expectedLibAst = new LibraryTree
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionImplementation
                    {
                        Name = "signledice",
                        Arguments = new List<string> { "arg1", "arg2" },
                        Script = expectedScript,
                    }
                }
            };
            Helpers.CompareObjects(expectedLibAst, libast);
        }

        [Fact]
        public void TestSingleFunctionAssignParsing()
        {
            var parser = new Parser();
            var libast = parser.ParseLibrary("macro signledice($faces) { var $test<-roll D6+$faces }");
            var expectedScript = new Script
            {
                Statements = new List<Statement>
                {
                    new AssignementStatement
                    {
                        VariableName = "test",
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration
                            {
                                Faces = new ConstantScalar { Value = 6 },
                                Number = new ConstantScalar { Value = 1 }
                            },
                            Aggregation = AggregationType.Sum,
                            SumBonus = new SumBonusDeclaration
                            {
                                Scalar = new VariableScalar { VariableName = "faces" },
                                Sign = SignType.Plus
                            }
                        }
                    }
                }
            };
            var expectedLibAst = new LibraryTree
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionImplementation
                    {
                        Name = "signledice",
                        Arguments = new List<string> { "faces" },
                        Script = expectedScript,
                    }
                }
            };
            Helpers.CompareObjects(expectedLibAst, libast);
        }

        [Fact]
        public void TestSingleFunctionMultipleStatementsParsing()
        {
            var parser = new Parser();
            var libast = parser.ParseLibrary(@"
            macro signledice($faces) {
                var $res <- roll D6+$faces;
                match $res ((<4, ""head""), (default, ""tails""))
            }
            ");
            var expectedScript = new Script
            {
                Statements = new List<Statement>
                {
                    new AssignementStatement
                    {
                        VariableName = "res",
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration
                            {
                                Faces = new ConstantScalar { Value = 6 },
                                Number = new ConstantScalar { Value = 1 }
                            },
                            Aggregation = AggregationType.Sum,
                            SumBonus = new SumBonusDeclaration
                            {
                                Scalar = new VariableScalar { VariableName = "faces" },
                                Sign = SignType.Plus
                            }
                        }
                    },
                    new RangeMappingStatement
                    {
                        Scalar = new VariableScalar { VariableName = "res" },
                        Ranges = new List<RangeDeclaration>
                        {
                            new RangeDeclaration
                            {
                                Filter = new FilterOption { Type = FilterType.Smaller, Scalar = new ConstantScalar { Value = 4 } },
                                Value = "head"
                            },
                            new RangeDeclaration
                            {
                                Filter = new FilterOption { Type = FilterType.None },
                                Value = "tails"
                            }
                        }
                    }
                }
            };
            var expectedLibAst = new LibraryTree
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionImplementation
                    {
                        Name = "signledice",
                        Arguments = new List<string> { "faces" },
                        Script = expectedScript,
                    }
                }
            };
            Helpers.CompareObjects(expectedLibAst, libast);
        }

        [Fact]
        public void TestMultipleFunctionsMultipleStatementsParsing()
        {
            var parser = new Parser();
            var libast = parser.ParseLibrary(@"
            macro signledice($faces) {
                var $res <- roll D6+$faces;
                match $res ((<4, ""head""), (default, ""tails""))
            }

            macro multipledice($bonus) {
                roll 2D8+$bonus;
            }
            ");
            var expectedScript1 = new Script
            {
                Statements = new List<Statement>
                {
                    new AssignementStatement
                    {
                        VariableName = "res",
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration
                            {
                                Faces = new ConstantScalar { Value = 6 },
                                Number = new ConstantScalar { Value = 1 }
                            },
                            Aggregation = AggregationType.Sum,
                            SumBonus = new SumBonusDeclaration
                            {
                                Scalar = new VariableScalar { VariableName = "faces" },
                                Sign = SignType.Plus
                            }
                        }
                    },
                    new RangeMappingStatement
                    {
                        Scalar = new VariableScalar { VariableName = "res" },
                        Ranges = new List<RangeDeclaration>
                        {
                            new RangeDeclaration
                            {
                                Filter = new FilterOption { Type = FilterType.Smaller, Scalar = new ConstantScalar { Value = 4 } },
                                Value = "head"
                            },
                            new RangeDeclaration
                            {
                                Filter = new FilterOption { Type = FilterType.None },
                                Value = "tails"
                            }
                        }
                    }
                }
            };
            var expectedScript2 = new Script
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
                                Number = new ConstantScalar { Value = 2 }
                            },
                            Aggregation = AggregationType.Sum,
                            SumBonus = new SumBonusDeclaration
                            {
                                Scalar = new VariableScalar { VariableName = "bonus" },
                                Sign = SignType.Plus
                            }
                        }
                    },
                }
            };
            var expectedLibAst = new LibraryTree
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionImplementation
                    {
                        Name = "signledice",
                        Arguments = new List<string> { "faces" },
                        Script = expectedScript1,
                    },
                    new FunctionImplementation
                    {
                        Name = "multipledice",
                        Arguments = new List<string> { "bonus" },
                        Script = expectedScript2,
                    }

                }
            };
            Helpers.CompareObjects(expectedLibAst, libast);
        }

        [Fact]
        public void TestPartialApplicationParsing()
        {
            var parser = new Parser();
            var libast = parser.ParseLibrary(@"
            macro multipledice($faces, $bonus) {
                roll 2D$faces+$bonus;
            }

            macro specialized($bonus) <- apply multipledice(4, $bonus)
            ");
            var expectedScript1 = new Script
            {
                Statements = new List<Statement>
                {
                    new ExpressionStatement
                    {
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration
                            {
                                Faces = new VariableScalar { VariableName = "faces" },
                                Number = new ConstantScalar { Value = 2 }
                            },
                            Aggregation = AggregationType.Sum,
                            SumBonus = new SumBonusDeclaration
                            {
                                Scalar = new VariableScalar { VariableName = "bonus" },
                                Sign = SignType.Plus
                            }
                        }
                    },
                }
            };
            var expectedLibAst = new LibraryTree
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionImplementation
                    {
                        Name = "multipledice",
                        Arguments = new List<string> { "faces", "bonus" },
                        Script = expectedScript1,
                    },
                    new PartialApplicationDeclaration
                    {
                        Name = "specialized",
                        Arguments = new List<string> { "bonus" },
                        AppliedFunction = "multipledice",
                        ProvidedValues = new List<Scalar>
                        {
                            new ConstantScalar { Value = 4 },
                            new VariableScalar { VariableName = "bonus" }
                        }
                    }
                }
            };
            Helpers.CompareObjects(expectedLibAst, libast);
        }
    }
}