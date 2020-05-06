using System;
using DiceScript.Contracts;

namespace DiceScript.Implementation
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

        public Dice Roll(int faces, bool exploding)
        {
            if (faces < 2)
            {
                throw new InvalidScriptException("Faces number must be 2 or above");
            }
            var roll = RollOnce(faces);
            if (exploding)
            {
                var finalValue = ExplodeDice(roll.Faces, roll.Result, roll.Result);
                roll.Result = finalValue;
            }
            return roll;
        }

        private int ExplodeDice(int faces, int lastResult, int total)
        {
            if (lastResult < faces)
            {
                return total;
            }

            var newResult = RollOnce(faces).Result;

            return ExplodeDice(faces, newResult, total + newResult);
        }

        private Dice RollOnce(int faces)
        {
            if (RollNbr >= MaxRollNbr)
            {
                throw new LimitException($"Reached maximum roll number: {MaxRollNbr}");
            }
            RollNbr++;
            return new Dice
            {
                Result = Random.Next(1, faces + 1),
                Faces = faces,
                Valid = true,
            };
        }
    }
}