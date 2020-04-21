using System.Collections.Generic;
using DiceSharp.Contracts;

namespace DiceSharp.Implementation
{
    internal class RollContext
    {
        public List<Dice> Dices { get; set; }
        public AggregationType AggregationType { get; set; }
    }
}