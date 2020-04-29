using System;
using System.Collections.Generic;
using DiceSharp.Contracts;

namespace DiceSharp.Implementation
{
    internal class Function
    {
        public FunctionSpec Spec { get; set; }
        public Func<DiceRoller, Dictionary<string, int>, IList<Result>> Run { get; set; }
    }
}