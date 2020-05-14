using System.Threading.Tasks;

namespace DiceCafe.WebApp.Rooms.Contracts
{
    public interface IResultPublisher
    {
        Task Publish(Room room, ResultGroup results);
    }
}