using System.Collections.Generic;
using DiceScript.Contracts;
using DiceCafe.WebApp.Users.Contract;

namespace DiceCafe.WebApp.Rooms.Contracts
{
    public class RoomState
    {
        public RoomState(string id)
        {
            Id = id;
        }
        public string Id { get; }
        public IList<User> Users { get; } = new List<User>();
        public List<ResultModel> Results { get; set; } = new List<ResultModel>();
        public IReadOnlyCollection<FunctionSpec> Functions { get; set; } = new List<FunctionSpec>();
    }
}