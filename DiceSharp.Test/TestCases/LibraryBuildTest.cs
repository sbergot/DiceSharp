using System;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using Xunit;

namespace DiceSharp.Test
{
    public class LibraryBuildTest
    {
        [Fact]
        public void TestLibBuild()
        {
            var roller = new Runner(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            roller.Random = new Random(0);
            var lib = roller.BuildLib(@"
            function singledice($bonus) {
                var $res <- roll D6+$bonus;
                match $res ((""head""; <4), (""tails""; default))
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
            var roller = new Runner(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            roller.Random = new Random(0);
            var lib = roller.BuildLib(@"
            function singledice($bonus) {
                var $res <- roll D6+$bonus;
                match $res ((""head""; <4), (""tails""; default))
            }
            ");
            Assert.Throws<ArgumentException>(() => lib.Call("singledice", new Dictionary<string, int> { { "badarg", 3 } }));
        }
    }
}