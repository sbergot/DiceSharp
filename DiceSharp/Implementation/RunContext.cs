using System.Collections.Generic;

namespace DiceSharp.Implementation
{
    internal class RunContext
    {
        public Dictionary<string, int> Variables { get; set; }
        public DiceRoller DiceRoller { get; set; }
    }
}