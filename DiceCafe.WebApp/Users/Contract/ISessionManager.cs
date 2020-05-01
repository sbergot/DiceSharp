using System.Threading.Tasks;

namespace DiceCafe.WebApp.Users.Contract
{
    public interface ISessionManager
    {
        User GetCurrentUser();
        Task SigninAsync(User user);

        Task SignoutAsync();
    }
}