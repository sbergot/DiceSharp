using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using DiceSharp.WebApp.Rooms.Contracts;
using DiceSharp.WebApp.ViewModels;
using DiceSharp.WebApp.Hubs;
using DiceSharp.WebApp.Rooms;
using DiceSharp.WebApp.Users;
using DiceSharp.WebApp.Users.Contract;

namespace DiceSharp.WebApp.Controllers
{
    public class RoomController : Controller
    {
        private ILogger<RoomController> Logger { get; }
        private IRoomRepository RoomRepository { get; }
        private ISessionManager SessionManager { get; }
        private IHubContext<RoomHub> RoomHub { get; }

        public RoomController(
            ILogger<RoomController> logger,
            IRoomRepository roomRepository,
            ISessionManager sessionManager,
            IHubContext<RoomHub> roomHub)
        {
            Logger = logger;
            RoomRepository = roomRepository;
            SessionManager = sessionManager;
            RoomHub = roomHub;
        }

        [Route("[controller]/[action]")]
        async public Task<IActionResult> New()
        {
            var room = RoomRepository.Create();
            await RoomHelpers.WithRoomLock(room, () =>
            {
                room.State.Players.Add(SessionManager.GetCurrentUser());
            });
            return RedirectToAction("Index", new { roomId = room.Id });
        }

        [Route("[controller]/{roomId}")]
        public IActionResult Index(string roomId)
        {
            if (!RoomRepository.Exists(roomId))
            {
                return RoomNotFound();
            }
            var room = RoomRepository.Get(roomId);
            string scheme = Request.HttpContext.Request.Scheme;
            return View(new RoomViewModel
            {
                Room = room,
                PlayerId = SessionManager.GetCurrentUser().Id,
            });
        }

        [Route("[controller]/{roomId}/[action]")]
        async public Task<IActionResult> Join(string roomId)
        {
            var normalisedRoomId = roomId.ToUpperInvariant();
            if (!RoomRepository.Exists(normalisedRoomId))
            {
                return RoomNotFound();
            }
            var room = RoomRepository.Get(normalisedRoomId);

            await RoomHelpers.WithRoomLock(room, async () =>
            {
                User player = SessionManager.GetCurrentUser();
                room.State.Players.Add(player);
                await RoomHub.Update(room);
                var message = $"{player.Name} a rejoint la salle";
                await RoomHub.Log(normalisedRoomId, message);
            });

            return RedirectToAction("Index", new { roomId = room.Id });
        }

        [Route("[controller]/{roomId}/[action]")]
        async public Task<IActionResult> Quit(string roomId)
        {
            if (!RoomRepository.Exists(roomId))
            {
                return RoomNotFound();
            }
            var room = RoomRepository.Get(roomId);

            await RoomHelpers.WithRoomLock(room, async () =>
            {
                User player = SessionManager.GetCurrentUser();
                room.State.Players.Remove(player);
                await RoomHub.Update(room);
                var message = $"{player.Name} a quitté la salle";
                await RoomHub.Log(roomId, message);
            });
            return Redirect("/");
        }

        private IActionResult RoomNotFound()
        {
            var result = View("RoomNotFound");
            result.StatusCode = 404;
            return result;
        }
    }
}
