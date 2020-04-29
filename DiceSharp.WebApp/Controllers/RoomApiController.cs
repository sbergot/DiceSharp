using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;

using DiceSharp.WebApp.Hubs;
using DiceSharp.WebApp.Rooms.Contracts;
using DiceSharp.WebApp.Rooms;
using DiceSharp.WebApp.Users.Contract;

namespace DiceSharp.WebApp.Controllers
{
    [ApiController]
    public class RoomApiController : Controller
    {
        private ILogger<RoomController> Logger { get; }
        private IRoomRepository RoomRepository { get; }
        private IHubContext<RoomHub> RoomHub { get; }
        private ISessionManager SessionManager { get; }
        private static Random Random { get; } = new Random();

        public RoomApiController(
            ILogger<RoomController> logger,
            IRoomRepository roomRepository,
            IHubContext<RoomHub> roomHub,
            ISessionManager sessionManager)
        {
            Logger = logger;
            RoomRepository = roomRepository;
            RoomHub = roomHub;
            SessionManager = sessionManager;
        }
    }
}
