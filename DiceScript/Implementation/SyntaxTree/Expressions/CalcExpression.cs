namespace DiceScript.Implementation.SyntaxTree
{
    internal class CalcExpression : Expression
    {
        public Scalar LeftValue { get; set; }
        public Scalar RightValue { get; set; }
        public SignType Operator { get; set; }
        public string Name { get; set; }
    }
}