using DiceSharp.Rooms.Contracts;

namespace DiceSharp.WebApp.Rooms.Contracts
{
    public interface IRoomRepository
    {
        Room Create();
        Room Get(string id);
        bool Exists(string id);
    }
}