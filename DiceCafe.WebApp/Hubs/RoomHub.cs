using DiceSharp;
using DiceCafe.WebApp.Rooms.Contracts;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DiceCafe.WebApp.Hubs
{
    public class RoomHub : Hub
    {
        public IRoomRepository RoomRepository { get; }

        public RoomHub(IRoomRepository roomRepository)
        {
            RoomRepository = roomRepository;
        }

        async public Task Log(string roomId, string message)
        {
            await Clients.Group(roomId).SendAsync("Log", message);
        }

        async public Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        async public Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }
    }
}