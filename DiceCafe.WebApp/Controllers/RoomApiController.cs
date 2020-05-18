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
using DiceCafe.WebApp.Rooms;
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
        private IResultPublisher ResultPublisher { get; }
        private static Random Random { get; } = new Random();

        public RoomApiController(
            ILogger<RoomController> logger,
            IRoomRepository roomRepository,
            IHubContext<RoomHub> roomHub,
            ISessionManager sessionManager,
            IResultPublisher resultPublisher)
        {
            Logger = logger;
            RoomRepository = roomRepository;
            RoomHub = roomHub;
            SessionManager = sessionManager;
            ResultPublisher = resultPublisher;
        }

        [HttpPost]
        [Route("api/room/{roomId}/[action]")]
        async public Task<IActionResult> Configuration(string roomId, [FromBody] RoomConfigurationViewModel roomConfiguration)
        {
            var normalisedRoomId = roomId.ToUpperInvariant();
            if (!RoomRepository.Exists(normalisedRoomId))
            {
                return NotFound();
            }
            var room = RoomRepository.Get(normalisedRoomId);

            if (room.State.Creator.Id != SessionManager.GetCurrentUser().Id)
            {
                return Unauthorized("Only the creator of the room can edit the library");
            }

            await RoomHelpers.WithRoomLock(room, async () =>
            {
                if (!string.IsNullOrEmpty(roomConfiguration.LibraryScript))
                {
                    var limitations = new DiceScript.Contracts.Limitations
                    {
                        MaxProgramSize = 2000,
                        MaxRollNbr = 100
                    };
                    var builder = new DiceScript.Builder(limitations);
                    room.Library = builder.BuildLib(roomConfiguration.LibraryScript);
                    room.State.Functions = room.Library.GetFunctionList();
                    room.State.LibraryScript = roomConfiguration.LibraryScript;
                }
                room.State.DiscordWebHook = roomConfiguration.DiscordWebHook;
                await RoomHub.Update(room);
            });

            return Ok();
        }

        [HttpPost]
        [Route("api/room/{roomId}/[action]")]
        async public Task<IActionResult> Run(string roomId, FunctionCallViewModel fcall)
        {
            var normalisedRoomId = roomId.ToUpperInvariant();
            if (!RoomRepository.Exists(normalisedRoomId))
            {
                return NotFound("Room not found");
            }

            var room = RoomRepository.Get(normalisedRoomId);

            return await RoomHelpers.WithRoomLock<IActionResult>(room, async () =>
            {
                if (room.Library == null)
                {
                    return base.BadRequest();
                }

                if (!room.Library.GetFunctionList().Any(f => f.Name == fcall.Name))
                {
                    return base.NotFound("Function not found");
                }

                var results = room.Library.Call(fcall.Name, fcall.Arguments);
                ResultGroup resultGroup = GroupResults(results);
                await ResultPublisher.Publish(room, resultGroup);
                return base.Ok();
            });
        }

        private ResultGroup GroupResults(IList<Result> results)
        {
            var taggedResults = results.Select(r => new TaggedResult
            {
                Result = r,
                ResultType = GetResultType(r),
            }).ToList();
            var resultGroup = new ResultGroup
            {
                Results = taggedResults,
                Created = DateTime.UtcNow,
                User = SessionManager.GetCurrentUser()
            };
            return resultGroup;
        }

        private static ResultType GetResultType(Result r)
        {
            if (r is RollResult)
            {
                return ResultType.Roll;
            }

            if (r is PrintResult)
            {
                return ResultType.Print;
            }

            if (r is ValueResult)
            {
                return ResultType.Value;
            }

            if (r is DiceResult)
            {
                return ResultType.Dice;
            }

            throw new InvalidOperationException();
        }

        [HttpPost]
        [Route("api/[action]")]
        public IActionResult RunScript([FromBody] string script)
        {
            var limitations = new DiceScript.Contracts.Limitations
            {
                MaxProgramSize = 500,
                MaxRollNbr = 100
            };
            var builder = new DiceScript.Builder(limitations);
            var runner = builder.BuildScript(script);
            var results = runner();
            return Json(GroupResults(results));
        }
    }
}
