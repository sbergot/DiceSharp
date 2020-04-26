namespace DiceSharp.Implementation.SyntaxTree
{
    internal class RichDiceExpression : Expression
    {
        public DiceDeclaration Dices { get; set; }
        public FilterOption Filter { get; set; }
        public AggregationType Aggregation { get; set; }
        public SumBonusExpression SumBonus { get; set; }
    }
}