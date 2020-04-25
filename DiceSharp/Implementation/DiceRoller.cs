using System;
using DiceSharp.Contracts;

namespace DiceSharp.Implementation
{
    public class DiceRoller
    {
        public Random Random { get; }
        public int MaxRollNbr { get; }
        public int RollNbr { get; private set; } = 0;

        public DiceRoller(int maxRollNbr) : this(maxRollNbr, new Random())
        {
        }

        public DiceRoller(int maxRollNbr, Random random)
        {
            MaxRollNbr = maxRollNbr;
            Random = random;
        }

        public Dice Roll(int faces)
        {
            if (RollNbr >= MaxRollNbr)
            {
                throw new LimitException($"Reached maximum roll number: {MaxRollNbr}");
            }
            RollNbr++;
            return new Dice
            {
                Result = Random.Next(1, faces),
                Faces = faces,
                Valid = true,
            };
        }

    }
}