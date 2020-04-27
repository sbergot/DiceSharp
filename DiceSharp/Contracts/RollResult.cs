using System.Collections.Generic;

namespace DiceSharp.Contracts
{
    public class RollResult : Result
    {
        public List<Dice> Dices { get; set; }
        public int Result { get; set; }
        public string Name { get; set; }
    }
}