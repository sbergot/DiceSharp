using System;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;

namespace DiceSharp.Implementation
{
    class Library : ILibrary
    {
        public Dictionary<string, Function> Functions { get; set; }

        public Random Random { get; set; } = new Random();
        public int MaxRollNumber { get; }

        public Library(int maxRollNumber)
        {
            MaxRollNumber = maxRollNumber;
            Functions = new Dictionary<string, Function>();
        }

        public IList<Result> Call(string name, Dictionary<string, int> arguments)
        {
            var func = Functions[name];
            foreach (var arg in func.Spec.Arguments)
            {
                if (!arguments.ContainsKey(arg))
                {
                    throw new ArgumentException($"Missing argument: {arg}");
                }
            }
            var diceRoller = new DiceRoller(MaxRollNumber, Random);
            return func.Run(diceRoller, arguments);
        }

        public IReadOnlyCollection<FunctionSpec> GetFunctionList()
        {
            return Functions.Values.Select(f => f.Spec).ToList();
        }

        public void SetSeed(int seed)
        {
            Random = new Random(seed);
        }
    }
}