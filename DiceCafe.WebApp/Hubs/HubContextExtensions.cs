using System.Threading.Tasks;
using DiceCafe.WebApp.Rooms.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace DiceCafe.WebApp.Hubs
{
    public static class HubContextExtensions
    {
        async public static Task Update(this IHubContext<RoomHub> hubContext, Room room)
        {
            await hubContext.Clients.Group(room.Id).SendAsync("Update", room.State);
        }
    }
}