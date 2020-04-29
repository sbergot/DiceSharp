using System.Collections.Generic;

namespace DiceSharp.Contracts
{
    public class FunctionSpec
    {
        public string Name { get; set; }
        public IReadOnlyCollection<string> Arguments { get; set; }
    }
}