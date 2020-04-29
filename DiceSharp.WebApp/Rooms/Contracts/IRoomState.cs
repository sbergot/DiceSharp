using System.Collections.Generic;
using DiceSharp.WebApp.Users;

namespace DiceSharp.Rooms.Contracts
{
    public class RoomState
    {
        public IList<User> Players { get; }
    }
}