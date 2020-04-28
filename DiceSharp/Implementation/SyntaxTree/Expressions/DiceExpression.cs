namespace DiceSharp.Implementation.SyntaxTree
{
    internal class DiceExpression : Expression
    {
        public DiceDeclaration Dices { get; set; }
        public FilterOption Filter { get; set; }
        public AggregationType Aggregation { get; set; }
        public SumBonusDeclaration SumBonus { get; set; }
        public bool Exploding { get; set; }
        public string Name { get; set; }
    }
}