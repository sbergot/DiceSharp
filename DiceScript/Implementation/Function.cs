using System;
using System.Collections.Generic;
using DiceScript.Contracts;

namespace DiceScript.Implementation
{
    internal class Function
    {
        public FunctionSpec Spec { get; set; }
        public Func<DiceRoller, Dictionary<string, int>, IList<Result>> Run { get; set; }
    }
}