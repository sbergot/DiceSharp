using System.Collections.Generic;
using DiceSharp.Contracts;
using DiceCafe.WebApp.Users.Contract;

namespace DiceCafe.WebApp.Rooms.Contracts
{
    public class RoomState
    {
        public IList<User> Players { get; } = new List<User>();
        public List<Result> Results { get; set; } = new List<Result>();
        public IReadOnlyCollection<FunctionSpec> Functions { get; set; } = new List<FunctionSpec>();
    }
}