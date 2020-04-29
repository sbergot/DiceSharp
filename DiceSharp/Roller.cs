using System;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using DiceSharp.Implementation;
using DiceSharp.Implementation.Parsing;

namespace DiceSharp
{
    public class Runner : IRunner
    {
        public Random Random { get; set; } = new Random();
        public Limitations Limitations { get; }

        public Runner(Limitations limitations)
        {
            Limitations = limitations;
        }

        public IList<Result> Roll(string rollquery)
        {
            if (rollquery.Length > Limitations.MaxProgramSize)
            {
                throw new LimitException($"program of size {rollquery.Length} above limit {Limitations.MaxProgramSize}");
            }
            var parser = new Parser();
            var compiler = new Compiler();
            var program = compiler.Compile(parser.ParseScript(rollquery));
            return program(new DiceRoller(Limitations.MaxRollNbr, Random));
        }
    }
}
