namespace DiceScript.Implementation.SyntaxTree
{
    internal class AssignementStatement : Statement
    {
        public AssignementType Type { get; set; }
        public string VariableName { get; set; }
        public Expression Expression { get; set; }
    }
}