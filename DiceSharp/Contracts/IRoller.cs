using System.Collections.Generic;

namespace DiceSharp.Contracts
{
    public interface IRoller
    {
        IList<Roll> Roll(string rollquery);
    }
}