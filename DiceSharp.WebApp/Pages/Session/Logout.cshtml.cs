namespace DiceSharp.Pages
{
    using System.Threading.Tasks;
    using DiceSharp.WebApp.Users.Contract;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class LogoutModel : PageModel
    {
        private ILogger<LogoutModel> Logger { get; }
        private ISessionManager SessionManager { get; }

        public LogoutModel(
            ILogger<LogoutModel> logger,
            ISessionManager sessionManager)
        {
            Logger = logger;
            SessionManager = sessionManager;
        }

        async public Task<ActionResult> OnGet()
        {
            await SessionManager.SignoutAsync();
            return Redirect("logoutconfirmation");
        }
    }
}
