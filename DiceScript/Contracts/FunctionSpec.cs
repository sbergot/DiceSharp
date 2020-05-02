using System.Collections.Generic;

namespace DiceScript.Contracts
{
    public class FunctionSpec
    {
        public string Name { get; set; }
        public IList<string> Arguments { get; set; }
    }
}