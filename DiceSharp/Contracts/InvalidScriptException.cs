namespace DiceSharp.Contracts
{
    [System.Serializable]
    public class InvalidScriptException : System.Exception
    {
        public InvalidScriptException() { }
        public InvalidScriptException(string message) : base(message) { }
        public InvalidScriptException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidScriptException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}