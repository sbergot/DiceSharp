using System;
using System.Collections.Generic;
using DiceSharp.Contracts;
using DiceSharp.Implementation;

namespace DiceSharp
{
    public class Roller : IRoller
    {
        public Random Random { get; set; } = new Random();

        public IList<Roll> Roll(string rollquery)
        {
            var parser = new Parser();
            var compiler = new Compiler();
            var program = compiler.Compile(parser.Parse(rollquery));
            return program(Random);
        }
    }
}
