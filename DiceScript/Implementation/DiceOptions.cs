using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Implementation
{
    internal class DiceOptions
    {
        public FilterOption Filter { get; set; }
        public RankingOption Ranking { get; set; }
        public AggregationType Aggregation { get; set; }
        public bool Exploding { get; set; }
    }
}