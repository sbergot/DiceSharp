using System.Collections.Generic;

namespace DiceScript.Contracts
{
    public interface ILibrary
    {
        IReadOnlyCollection<FunctionSpec> GetFunctionList();
        IList<Result> Call(string name, Dictionary<string, int> arguments);
        void SetSeed(int seed);
    }
}