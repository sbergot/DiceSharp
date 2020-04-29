using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Rooms;
using DiceSharp.Rooms.Contracts;
using DiceSharp.WebApp.Rooms.Contracts;

namespace DiceSharp.WebApp.Rooms
{
    public class RoomRepository : IRoomRepository
    {
        private IDictionary<string, Room> Rooms { get; } = new ConcurrentDictionary<string, Room>();
        private Random Random { get; } = new Random();

        public RoomRepository()
        { }

        private string NewRoomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(
                Enumerable.Repeat(chars, 4)
                    .Select(s => s[Random.Next(s.Length)])
                    .ToArray()
            );
        }

        public Room Create()
        {
            CleanupOldRooms();
            if (Rooms.Keys.Count > 1000)
            {
                throw new InvalidOperationException("too many active rooms");
            }
            var room = new Room
            {
                Id = NewRoomId(),
                LastUpdate = DateTime.UtcNow,
                State = new RoomState(),
            };
            Rooms[room.Id] = room;
            return room;
        }

        public Room Get(string id)
        {
            return Rooms[id];
        }

        public bool Exists(string id)
        {
            return Rooms.ContainsKey(id);
        }

        private void CleanupOldRooms()
        {
            var limit = DateTime.UtcNow.AddDays(-7);
            foreach (var room in Rooms.Values)
            {
                if (room.LastUpdate < limit)
                {
                    Rooms.Remove(room.Id);
                }
            }
        }
    }
}