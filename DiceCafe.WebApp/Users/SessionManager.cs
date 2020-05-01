using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DiceCafe.WebApp.Users.Contract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace DiceCafe.WebApp.Users
{
    public class SessionManager : ISessionManager
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        private string Scheme { get; } = CookieAuthenticationDefaults.AuthenticationScheme;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public User GetCurrentUser()
        {
            var httpContext = HttpContextAccessor.HttpContext;
            var ticket = httpContext.User.Identity;
            if (ticket is ClaimsIdentity claimsIdentity)
            {
                var name = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.Name).Value;
                var id = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return new User
                {
                    Name = name,
                    Id = id,
                };
            }
            else
            {
                throw new InvalidOperationException("User is not authenticated");
            }
        }

        private ClaimsPrincipal GetPrincipal(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var identity = new ClaimsIdentity(claims, Scheme);
            return new ClaimsPrincipal(identity);
        }

        async public Task SigninAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var identity = new ClaimsIdentity(claims, Scheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            var httpContext = HttpContextAccessor.HttpContext;
            await httpContext.SignInAsync(Scheme, principal, authProperties);
        }

        async public Task SignoutAsync()
        {
            await HttpContextAccessor.HttpContext.SignOutAsync(Scheme);
        }
    }
}