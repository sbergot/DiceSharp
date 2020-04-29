using System.Collections.Generic;
using DiceSharp.Implementation;
using DiceSharp.Implementation.Parsing;
using DiceSharp.Implementation.SyntaxTree;
using Xunit;

namespace DiceSharp.Test.TestCases
{
    public class LibraryParserTest
    {
        [Fact]
        public void TestSingleFunctionParsing()
        {
            var parser = new Parser();
            var libast = parser.ParseLibrary("function signledice($faces) { roll D6+$faces }");
            var expectedScript = new Script
            {
                Statements = new List<Statement>
                {
                    new ExpressionStatement
                    {
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration { Faces = 6, Number = 1 },
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
            var expectedLibAst = new Library
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionDeclaration
                    {
                        Name = "signledice",
                        Variables = new List<string> { "faces" },
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
            var libast = parser.ParseLibrary("function signledice($faces) { var $test<-roll D6+$faces }");
            var expectedScript = new Script
            {
                Statements = new List<Statement>
                {
                    new AssignementStatement
                    {
                        VariableName = "test",
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration { Faces = 6, Number = 1 },
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
            var expectedLibAst = new Library
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionDeclaration
                    {
                        Name = "signledice",
                        Variables = new List<string> { "faces" },
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
            var libast = parser.ParseLibrary("function signledice($faces) { var $res <- roll D6+$faces; range $res ((\"head\"; <4), (\"tails\"; default))}");
            var expectedScript = new Script
            {
                Statements = new List<Statement>
                {
                    new AssignementStatement
                    {
                        VariableName = "res",
                        Expression = new DiceExpression
                        {
                            Dices = new DiceDeclaration { Faces = 6, Number = 1 },
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
            var expectedLibAst = new Library
            {
                Functions = new List<FunctionDeclaration>
                {
                    new FunctionDeclaration
                    {
                        Name = "signledice",
                        Variables = new List<string> { "faces" },
                        Script = expectedScript,
                    }
                }
            };
            Helpers.CompareObjects(expectedLibAst, libast);
        }
    }
}