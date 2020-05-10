namespace DiceScript.Implementation.SyntaxTree
{
    internal class AggregationExpression : Expression
    {
        public VariableScalar Variable { get; set; }
        public FilterOption Filter { get; set; }
        public RankingOption Ranking { get; set; }
        public AggregationType Aggregation { get; set; }
        public SumBonusDeclaration SumBonus { get; set; }
        public bool Exploding { get; set; }
        public string Name { get; set; }
    }
}