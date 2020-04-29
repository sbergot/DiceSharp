namespace DiceSharp.Pages
{
    using System.Threading.Tasks;
    using DiceSharp.WebApp.Users.Contract;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private ILogger<LoginModel> Logger { get; }
        private ISessionManager SessionManager { get; }

        public LoginModel(
            ILogger<LoginModel> logger,
            ISessionManager sessionManager)
        {
            Logger = logger;
            SessionManager = sessionManager;
        }

        public void OnGet() { }

        async public Task OnPost(string username, string returnurl)
        {
            var user = DiceSharp.WebApp.Users.User.Create(username);
            await SessionManager.SigninAsync(user);
        }
    }
}
