using System;
using System.Threading.Tasks;
using DiceCafe.WebApp.Rooms.Contracts;

namespace DiceCafe.WebApp.Rooms
{
    public static class RoomHelpers
    {
        async public static Task WithRoomLock(Room room, Func<Task> action)
        {
            await room.Lock.WaitAsync();
            try
            {
                await action();
            }
            finally
            {
                room.Lock.Release();
            }
        }

        async public static Task WithRoomLock(Room room, Action action)
        {
            await room.Lock.WaitAsync();
            try
            {
                action();
            }
            finally
            {
                room.Lock.Release();
            }
        }
    }
}