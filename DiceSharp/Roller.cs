using System;
using System.Collections.Generic;
using DiceSharp.Contracts;
using DiceSharp.Implementation;
using DiceSharp.Implementation.Parsing;

namespace DiceSharp
{
    public class Roller : IRoller
    {
        public Random Random { get; set; } = new Random();
        public Limitations Limitations { get; }

        public Roller(Limitations limitations)
        {
            Limitations = limitations;
        }

        public IList<Roll> Roll(string rollquery)
        {
            if (rollquery.Length > Limitations.MaxProgramSize)
            {
                throw new LimitException($"program of size {rollquery.Length} above limit {Limitations.MaxProgramSize}");
            }
            var parser = new Parser();
            var compiler = new Compiler();
            var program = compiler.Compile(parser.Parse(rollquery));
            return program(new DiceRoller(Limitations.MaxRollNbr, Random));
        }
    }
}
