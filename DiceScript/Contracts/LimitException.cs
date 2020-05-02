namespace DiceScript.Contracts
{
    [System.Serializable]
    public class LimitException : System.Exception
    {
        public LimitException() { }
        public LimitException(string message) : base(message) { }
        public LimitException(string message, System.Exception inner) : base(message, inner) { }
        protected LimitException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}