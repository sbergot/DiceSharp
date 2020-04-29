namespace DiceSharp.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class JoinRoomModel : PageModel
    {
        private ILogger<JoinRoomModel> Logger { get; }

        public JoinRoomModel(
            ILogger<JoinRoomModel> logger)
        {
            Logger = logger;
        }

        public void OnGet() { }

        public IActionResult OnPost(string code)
        {
            return RedirectToAction("Join", "Room", new { roomId = code });
        }
    }
}
