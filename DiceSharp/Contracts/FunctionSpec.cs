using System.Collections.Generic;

namespace DiceSharp.Contracts
{
    public class FunctionSpec
    {
        public string Name { get; set; }
        public IList<string> Arguments { get; set; }
    }
}