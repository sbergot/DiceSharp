using System;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using DiceSharp.Implementation;
using DiceSharp.Implementation.Parsing;

namespace DiceSharp
{
    public class Runner
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
            var program = compiler.CompileScript(parser.ParseScript(rollquery));
            return program(new DiceRoller(Limitations.MaxRollNbr, Random));
        }

        public ILibrary BuildLib(string libstr)
        {
            if (libstr.Length > Limitations.MaxProgramSize)
            {
                throw new LimitException($"program of size {libstr.Length} above limit {Limitations.MaxProgramSize}");
            }
            var lib = new Library(Limitations.MaxRollNbr);
            var parser = new Parser();
            var compiler = new Compiler();
            var funcs = compiler.CompileLib(parser.ParseLibrary(libstr));
            foreach (var function in funcs)
            {
                lib.Functions[function.Spec.Name] = function;
            }
            return lib;
        }
    }
}
