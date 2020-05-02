using DiceScript.Contracts;

namespace DiceCafe.WebApp.Rooms.Contracts
{
    public class TaggedResult
    {
        public ResultType ResultType { get; set; }
        public Result Result { get; set; }
    }
}