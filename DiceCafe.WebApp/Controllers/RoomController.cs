using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using DiceCafe.WebApp.Rooms.Contracts;
using DiceCafe.WebApp.ViewModels;
using DiceCafe.WebApp.Hubs;
using DiceCafe.WebApp.Rooms;
using DiceCafe.WebApp.Users;
using DiceCafe.WebApp.Users.Contract;

namespace DiceCafe.WebApp.Controllers
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
                User user = SessionManager.GetCurrentUser();
                room.State.Users.Add(user);
                room.State.Creator = user;
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
                Room = room.State,
                UserId = SessionManager.GetCurrentUser().Id,
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
                User user = SessionManager.GetCurrentUser();
                room.State.Users.Add(user);
                await RoomHub.Update(room);
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
                User user = SessionManager.GetCurrentUser();
                room.State.Users.Remove(user);
                await RoomHub.Update(room);
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
