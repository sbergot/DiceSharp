using System.Collections.Generic;

namespace DiceSharp.Contracts
{
    public class Roll
    {
        public List<Dice> Dices { get; set; }
        public int Result { get; set; }
    }
}