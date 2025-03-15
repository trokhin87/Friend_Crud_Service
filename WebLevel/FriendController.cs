using Bussines.Services;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebLevel;
[ApiController]
[Route("api/[controller]")]
public class FriendController:ControllerBase
{
    private readonly Service _friendService;

    public FriendController(Service friendService)
    {
        _friendService = friendService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddFriend([FromBody] FriendDTO dto)
    {
        var result=await _friendService.AddFriendAsync(dto);
        return result ? Ok(new { message = "Friend added successfully" }) : BadRequest(new { message = "Failed to add friend" });
    }
    [HttpDelete("delete")]
    public async Task<IActionResult> RemoveFriend([FromBody] RemoveFriendDTO removeFriendDto)
    {
        var result = await _friendService.RemoveFriendAsync(removeFriendDto);
        return result ? Ok(new { message = "Friend removed successfully" }) : BadRequest(new { message = "Failed to remove friend" });
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateFriend([FromBody] FriendDTO friendDto)
    {
        var result = await _friendService.UpdateFriendAsync(friendDto);
        return result ? Ok(new { message = "Friend updated successfully" }) : BadRequest(new { message = "Failed to update friend" });
    }
    [HttpGet("list/{appId:guid}")]
    public async Task<IActionResult> GetFriendsByAppId(Guid appId)
    {
        var friends = await _friendService.GetFriendsAsync(new AppIdDTO { Id = appId });
        if (friends.Count == 0)
        {
            return NotFound(new { message = "No friends found for the provided appId" });
        }
        return Ok(friends);
    }

    [HttpPost("addWithWish")]
    public async Task<IActionResult> AddFriendWithWish(FriendDTO dto)
    {
        var addFriendResult = await _friendService.AddFriendAsync(dto);
        if (!addFriendResult)
        {
            return BadRequest(new { message = "Failed to add friend" });
        }

        // Шаг 2: Получение WishId по Username и AppID
        var wishIdDto = await _friendService.GetWishIdAsync(new AppIDFriendDTO
        {
            Username = dto.FriendUsername, // Предполагаем, что FriendDTO содержит поле Username
            AppID = dto.AppId // И поле AppID
        });

        if (wishIdDto == null)
        {
            return NotFound(new { message = "Wish ID not found" });
        }

        // Шаг 3: Добавление интересов и пожеланий
        var interestAndWishDto = new InterestAndWishDTO
        {
            IdWish = wishIdDto.IdWish, // Предполагаем, что WishIdDTO содержит поле WishId
            Interest = dto.FriendUsername,
            Wish = wishIdDto.IdWish;
        };

        var addWishResult = await _friendService.AddWishAndInterest(interestAndWishDto);
        if (!addWishResult)
        {
            return BadRequest(new { message = "Failed to add wish and interest" });
        }

        // Возвращаем успешный результат
        return Ok(new { message = "Friend added, wish ID retrieved, and wish/interest added successfully" });
    }
    
    
}