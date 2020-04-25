namespace DiceSharp.Implementation.SyntaxTree
{
    internal class FilterExpression : OptionExpression
    {
        public FilterType Type { get; set; }
        public int Scalar { get; set; }
    }
}