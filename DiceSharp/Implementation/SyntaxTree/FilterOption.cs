namespace DiceSharp.Implementation.SyntaxTree
{
    internal class FilterOption : Option
    {
        public FilterType Type { get; set; }
        public Scalar Scalar { get; set; }
    }
}