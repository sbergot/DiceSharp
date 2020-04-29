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
            var lib = roller.BuildLib("function singledice($faces) { var $res <- roll D6+$faces; range $res ((\"head\"; <4), (\"tails\"; default))}");
            var funcs = lib.GetFunctionList();
            Assert.Equal(1, funcs.Count);
            var function = funcs.Single();
            Assert.Equal("singledice", function.Name);
            Assert.Equal(1, function.Arguments.Count);
            Assert.Equal("faces", function.Arguments.Single());
            lib.SetSeed(0);
            var results = lib.Call("singledice", new Dictionary<string, int> { { "faces", 3 } });
        }
    }
}