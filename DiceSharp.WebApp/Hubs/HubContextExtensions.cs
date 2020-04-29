using System.Threading.Tasks;
using DiceSharp.Rooms.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace DiceSharp.WebApp.Hubs
{
    public static class HubContextExtensions
    {
        async public static Task Update(this IHubContext<RoomHub> hubContext, Room room)
        {
            await hubContext.Clients.Group(room.Id).SendAsync("Update", room);
        }

        async public static Task Log(this IHubContext<RoomHub> hubContext, string roomId, string message)
        {
            await hubContext.Clients.Group(roomId).SendAsync("Log", message);
        }
    }
}