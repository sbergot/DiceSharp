using System;
using System.Collections.Generic;
using System.Linq;
using DiceScript.Contracts;
using Xunit;

namespace DiceScript.Test
{
    public class LibraryBuildTest
    {
        [Fact]
        public void TestLibBuild()
        {
            var builder = new Builder(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            builder.Random = new Random(0);
            var lib = builder.BuildLib(@"
            function singledice($bonus) {
                var $res <- roll D6+$bonus;
                match $res ((<4, ""head""), (default, ""tails""))
            }
            ");
            var funcs = lib.GetFunctionList();
            var expectedFuncs = new List<FunctionSpec>
            {
                new FunctionSpec { Name = "singledice", Arguments = new List<string> { "bonus" } }
            };
            Helpers.CompareObjects(expectedFuncs, funcs);
            lib.SetSeed(0);
            var results = lib.Call("singledice", new Dictionary<string, int> { { "bonus", 3 } });
            var expectedResults = new List<Result>
            {
                new RollResult
                {
                    Description = new RollDescription { Faces = 6, Number = 1, Bonus = 3, Exploding = false },
                    Dices = new List<Dice>
                    {
                        new Dice { Faces = 6, Result = 5, Valid = true }
                    },
                    Result = 8
                },
                new PrintResult { Value = "tails" }
            };
            Helpers.CompareObjects(expectedResults, results);
        }

        [Fact]
        public void TestArgMissing()
        {
            var builder = new Builder(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            builder.Random = new Random(0);
            var lib = builder.BuildLib(@"
            function singledice($bonus) {
                var $res <- roll D6+$bonus;
                match $res ((<4, ""head""), (default, ""tails""))
            }
            ");
            Assert.Throws<ArgumentException>(() => lib.Call("singledice", new Dictionary<string, int> { { "badarg", 3 } }));
        }

        [Fact]
        public void TestCustomDice()
        {
            var Builder = new Builder(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            Builder.Random = new Random(0);
            var lib = Builder.BuildLib(@"
            function customDice($nbr, $faces, $bonus) {
                roll $nbr$D$faces+$bonus;
            }
            ");
            var funcs = lib.GetFunctionList();
            var expectedFuncs = new List<FunctionSpec>
            {
                new FunctionSpec
                {
                    Name = "customDice",
                    Arguments = new List<string>
                    {
                        "nbr",
                        "faces",
                        "bonus",
                    }
                }
            };
            Helpers.CompareObjects(expectedFuncs, funcs);
            lib.SetSeed(0);
            var results = lib.Call(
                "customDice",
                new Dictionary<string, int>
                {
                    { "nbr", 2 },
                    { "faces", 8 },
                    { "bonus", 3 }
                });
            var expectedResults = new List<Result>
            {
                new RollResult
                {
                    Description = new RollDescription { Faces = 8, Number = 2, Bonus = 3, Exploding = false },
                    Dices = new List<Dice>
                    {
                        new Dice { Faces = 8, Result = 6, Valid = true },
                        new Dice { Faces = 8, Result = 7, Valid = true }
                    },
                    Result = 16
                },
            };
            Helpers.CompareObjects(expectedResults, results);
        }

        [Fact]
        public void TestPartialApplication()
        {
            var Builder = new Builder(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            Builder.Random = new Random(0);
            var lib = Builder.BuildLib(@"
            function multipledice($faces, $bonus) {
                roll 2D$faces+$bonus;
            }

            function specialized($bonus) <- apply multipledice(4, $bonus)
            ");
            var funcs = lib.GetFunctionList();
            var expectedFuncs = new List<FunctionSpec>
            {
                new FunctionSpec
                {
                    Name = "multipledice",
                    Arguments = new List<string>
                    {
                        "faces",
                        "bonus",
                    }
                },
                new FunctionSpec
                {
                    Name = "specialized",
                    Arguments = new List<string>
                    {
                        "bonus",
                    }
                },
            };
            Helpers.CompareObjects(expectedFuncs, funcs);
            lib.SetSeed(0);
            var results = lib.Call(
                "specialized",
                new Dictionary<string, int>
                {
                    { "bonus", 3 }
                });
            var expectedResults = new List<Result>
            {
                new RollResult
                {
                    Description = new RollDescription { Faces = 4, Number = 2, Bonus = 3, Exploding = false },
                    Dices = new List<Dice>
                    {
                        new Dice { Faces = 4, Result = 3, Valid = true },
                        new Dice { Faces = 4, Result = 4, Valid = true }
                    },
                    Result = 10
                },
            };
            Helpers.CompareObjects(expectedResults, results);
        }
    }
}