using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;

using DiceCafe.WebApp.Hubs;
using DiceCafe.WebApp.Rooms.Contracts;
using DiceCafe.WebApp.Rooms;
using DiceCafe.WebApp.Users.Contract;
using System.Collections.Generic;

namespace DiceCafe.WebApp.Controllers
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

        [HttpPost]
        [Route("api/room/{roomId}/[action]")]
        async public Task<IActionResult> Library(string roomId, string library)
        {
            var normalisedRoomId = roomId.ToUpperInvariant();
            if (!RoomRepository.Exists(normalisedRoomId))
            {
                return NotFound();
            }
            var room = RoomRepository.Get(normalisedRoomId);
            var limitations = new DiceSharp.Contracts.Limitations
            {
                MaxProgramSize = 500,
                MaxRollNbr = 100
            };
            var builder = new DiceSharp.Builder(limitations);
            room.Library = builder.BuildLib(library);
            room.State.Functions = room.Library.GetFunctionList();
            await RoomHub.Update(room);

            return Ok();
        }

        [HttpPost]
        [Route("api/room/{roomId}/[action]/{name}")]
        async public Task<IActionResult> Run(string roomId, string name, Dictionary<string, int> args)
        {
            var normalisedRoomId = roomId.ToUpperInvariant();
            if (!RoomRepository.Exists(normalisedRoomId))
            {
                return NotFound();
            }

            var room = RoomRepository.Get(normalisedRoomId);
            if (room.Library == null)
            {
                return BadRequest();
            }

            if (!room.Library.GetFunctionList().Any(f => f.Name == name))
            {
                return NotFound();
            }

            var result = room.Library.Call(name, args);
            room.State.Results.AddRange(result);
            await RoomHub.Update(room);

            return Ok();
        }
    }
}
