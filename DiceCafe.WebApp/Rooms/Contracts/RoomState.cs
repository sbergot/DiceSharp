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
        public User Creator { get; set; }
        public string LibraryScript { get; set; }
        public string DiscordWebHook { get; set; }
        public IList<User> Users { get; } = new List<User>();
        public List<ResultGroup> Results { get; set; } = new List<ResultGroup>();
        public IReadOnlyCollection<FunctionSpec> Functions { get; set; } = new List<FunctionSpec>();
    }
}