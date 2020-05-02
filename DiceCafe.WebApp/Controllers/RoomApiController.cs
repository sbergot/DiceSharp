using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;

using DiceCafe.WebApp.Hubs;
using DiceCafe.WebApp.Rooms.Contracts;
using DiceCafe.WebApp.Users.Contract;
using DiceCafe.WebApp.ViewModels;
using DiceScript.Contracts;

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
        async public Task<IActionResult> Library(string roomId, [FromBody]string library)
        {
            var normalisedRoomId = roomId.ToUpperInvariant();
            if (!RoomRepository.Exists(normalisedRoomId))
            {
                return NotFound();
            }
            var room = RoomRepository.Get(normalisedRoomId);
            var limitations = new DiceScript.Contracts.Limitations
            {
                MaxProgramSize = 500,
                MaxRollNbr = 100
            };
            var builder = new DiceScript.Builder(limitations);
            room.Library = builder.BuildLib(library);
            room.State.Functions = room.Library.GetFunctionList();
            await RoomHub.Update(room);

            return Ok();
        }

        [HttpPost]
        [Route("api/room/{roomId}/[action]")]
        async public Task<IActionResult> Run(string roomId, FunctionCallViewModel fcall)
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

            if (!room.Library.GetFunctionList().Any(f => f.Name == fcall.Name))
            {
                return NotFound();
            }

            var results = room.Library.Call(fcall.Name, fcall.Arguments);
            room.State.Results.AddRange(results.Select(r => new ResultModel
            {
                Result = r,
                ResultType = r is RollResult ? ResultType.Roll : ResultType.Print,
                Created = DateTime.UtcNow,
                User = SessionManager.GetCurrentUser()
            }));
            await RoomHub.Update(room);

            return Ok();
        }
    }
}
