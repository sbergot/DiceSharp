using System;
using System.Collections.Generic;
using DiceScript.Contracts;
using DiceScript.Implementation;
using DiceScript.Implementation.Parsing;

namespace DiceScript
{
    public class Builder
    {
        public Random Random { get; set; } = new Random();
        public Limitations Limitations { get; }

        public Builder(Limitations limitations)
        {
            Limitations = limitations;
        }

        public Func<IList<Result>> BuildScript(string rollquery)
        {
            if (rollquery.Length > Limitations.MaxProgramSize)
            {
                throw new LimitException($"program of size {rollquery.Length} above limit {Limitations.MaxProgramSize}");
            }
            var parser = new Parser();
            var compiler = new Compiler();
            var program = compiler.CompileScript(parser.ParseScript(rollquery));
            var diceRoller = new DiceRoller(Limitations.MaxRollNbr, Random);
            return () => program(diceRoller);
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
