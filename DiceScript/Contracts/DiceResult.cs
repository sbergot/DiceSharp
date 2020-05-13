using System.Collections.Generic;

namespace DiceScript.Contracts
{
    public class DiceResult : Result
    {
        public string Name { get; set; }
        public List<Dice> Dices { get; set; }
        public RollDescription Description { get; set; }
    }
}