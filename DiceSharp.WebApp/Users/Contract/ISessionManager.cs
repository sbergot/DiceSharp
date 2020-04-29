using System.Threading.Tasks;

namespace DiceSharp.WebApp.Users.Contract
{
    public interface ISessionManager
    {
        User GetCurrentUser();
        Task SigninAsync(User user);

        Task SignoutAsync();
    }
}