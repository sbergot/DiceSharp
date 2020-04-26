namespace DiceSharp.Implementation.SyntaxTree
{
    internal class AssignementStatement : Statement
    {
        public string VariableName { get; set; }
        public Expression Expression { get; set; }
    }
}