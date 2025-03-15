using DTO;

namespace Interfaces;

public interface IFriendService
{
    Task<bool> AddFriendAsync(FriendDTO dto);
    Task<bool> RemoveFriendAsync(RemoveFriendDTO dto);
    Task <bool> UpdateFriendAsync(FriendDTO dto);
    Task<WishIdDTO> GetWishIdAsync(AppIDFriendDTO dto);
    Task<bool> AddWishAndInterest(InterestAndWishDTO dto);
    Task<List<FriendDTO>> GetFriendsAsync(AppIdDTO dto);
}