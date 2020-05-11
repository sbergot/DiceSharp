namespace DiceScript.Implementation.SyntaxTree
{
    internal class DiceExpression : Expression
    {
        public DiceDeclaration Dices { get; set; }
        public OptionGroup Options { get; set; }
        public SumBonusDeclaration SumBonus { get; set; }
        public string Name { get; set; }
    }
}