namespace DiceSharp.Implementation.SyntaxTree
{
    internal class RankingOption : Option
    {
        public RankingType Type { get; set; }
        public Scalar Scalar { get; set; }
    }
}