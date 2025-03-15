using DTO;

namespace Interfaces;

public interface IFriendService
{
    Task<bool> AddFriendAsync(FriendDTO friend);
}