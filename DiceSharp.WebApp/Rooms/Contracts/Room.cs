using System;
using System.Threading;

namespace DiceSharp.Rooms.Contracts
{
    public class Room
    {
        public string Id { get; set; }
        public RoomState State { get; set; }
        public DateTime LastUpdate { get; set; }
        public SemaphoreSlim Lock { get; } = new SemaphoreSlim(1, 1);
    }
}