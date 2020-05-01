using DiceCafe.WebApp.Rooms.Contracts;

namespace DiceCafe.WebApp.Rooms.Contracts
{
    public interface IRoomRepository
    {
        Room Create();
        Room Get(string id);
        bool Exists(string id);
    }
}