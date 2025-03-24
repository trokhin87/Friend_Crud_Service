using DTO;

namespace Interfaces;

public interface IFriendService
{
    Task<bool> AddFriendAsync(FriendDTO dto);
    Task<bool> RemoveFriendAsync(DeleteFriendDto dto);
    Task <bool> UpdateFriendAsync(FriendDTO dto);
    Task<WishIdDTO> GetWishIdAsync(AppIDFriendDTO dto);
    Task<bool> AddWishAndInterestAsync(AddIntAndPozhDto dto, FriendDTO friendDto);
    Task<List<FriendDTO>> GetFriendsAsync(AppIdDTO dto);
}