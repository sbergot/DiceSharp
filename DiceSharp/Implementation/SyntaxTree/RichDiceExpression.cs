namespace DiceSharp.Implementation.SyntaxTree
{
    internal class RichDiceExpression : Expression
    {
        public DiceExpression Dices { get; set; }
        public FilterExpression Filter { get; set; }
        public AggregateExpression Aggregation { get; set; }
        public SumBonusExpression SumBonus { get; set; }
    }
}