using System.Threading.Tasks;
using DiceCafe.WebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiceCafe.WebApp.ViewComponents
{
    public class ProfileViewComponent : ViewComponent
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public ProfileViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            string name = null;
            var identity = HttpContextAccessor.HttpContext.User.Identity;
            if (identity.IsAuthenticated)
            {
                name = identity.Name;
            }
            IViewComponentResult result = View(new ProfileViewModel { Name = name });
            return Task.FromResult(result);
        }
    }
}