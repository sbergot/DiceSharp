using System.Collections.Generic;

namespace DiceSharp.Contracts
{
    public interface IRunner
    {
        IList<Result> Roll(string rollquery);
    }
}