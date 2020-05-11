namespace DiceScript.Implementation.SyntaxTree
{
    internal class AggregationExpression : Expression
    {
        public VariableScalar Variable { get; set; }
        public OptionGroup Options { get; set; }
        public string Name { get; set; }
    }
}