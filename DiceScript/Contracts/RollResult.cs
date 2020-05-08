using System.Collections.Generic;

namespace DiceScript.Contracts
{
    public class RollResult : ValueResult
    {
        public RollDescription Description { get; set; }
        public List<Dice> Dices { get; set; }
        public string Name { get; set; }
    }
}