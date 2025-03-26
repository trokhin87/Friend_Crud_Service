using Bussines.Services;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebLevel;

[ApiController]
[Route("api/[controller]")]
public class FriendController : ControllerBase
{
    private readonly Service _friendService;

    public FriendController(Service friendService)
    {
        _friendService = friendService;
    }

    /// <summary>
    /// Добавить друга
    /// </summary>
    [HttpPost("add")]
    [SwaggerOperation(Summary = "Добавление друга", Description = "Добавляет нового друга.")]
    [SwaggerResponse(200, "Друг успешно добавлен")]
    [SwaggerResponse(400, "Ошибка при добавлении друга")]
    public async Task<IActionResult> AddFriend([FromBody] FriendDTO dto)
    {
        var result = await _friendService.AddFriendAsync(dto);
        return result ? Ok(new { message = "Friend added successfully" }) : BadRequest(new { message = "Failed to add friend" });
    }

    /// <summary>
    /// Удалить друга
    /// </summary>
    [HttpDelete("delete")]
    [SwaggerOperation(Summary = "Удаление друга", Description = "Удаляет друга из списка.")]
    [SwaggerResponse(200, "Друг успешно удален")]
    [SwaggerResponse(400, "Ошибка при удалении друга")]
    public async Task<IActionResult> RemoveFriend([FromBody] DeleteFriendDto deleteFriendDto)
    {
        var result = await _friendService.RemoveFriendAsync(deleteFriendDto);
        return result ? Ok(new { message = "Friend removed successfully" }) : BadRequest(new { message = "Failed to remove friend" });
    }

    /// <summary>
    /// Обновить данные друга
    /// </summary>
    [HttpPut("update")]
    [SwaggerOperation(Summary = "Обновление друга", Description = "Обновляет информацию о друге.")]
    [SwaggerResponse(200, "Информация обновлена")]
    [SwaggerResponse(400, "Ошибка при обновлении")]
    public async Task<IActionResult> UpdateFriend([FromBody] FriendDTO friendDto)
    {
        var result = await _friendService.UpdateFriendAsync(friendDto);
        return result ? Ok(new { message = "Friend updated successfully" }) : BadRequest(new { message = "Failed to update friend" });
    }

    /// <summary>
    /// Получить список друзей по appId
    /// </summary>
    [HttpGet("list/{appId:guid}")]
    [SwaggerOperation(Summary = "Получение друзей", Description = "Возвращает список друзей по AppId.")]
    [SwaggerResponse(200, "Список друзей", typeof(List<FriendDTO>))]
    [SwaggerResponse(404, "Друзья не найдены")]
    public async Task<IActionResult> GetFriendsByAppId(Guid appId)
    {
        var friends = await _friendService.GetFriendsAsync(new AppIdDTO { Id = appId });
        if (friends.Count == 0)
        {
            return NotFound(new { message = "No friends found for the provided appId" });
        }
        return Ok(friends);
    }

    /// <summary>
    /// Добавить друга с пожеланием
    /// </summary>
    [HttpPost("addWithWish")]
    [SwaggerOperation(Summary = "Добавление друга с пожеланием", Description = "Добавляет друга с пожеланием в список.")]
    [SwaggerResponse(200, "Друг добавлен")]
    [SwaggerResponse(400, "Ошибка при добавлении")]
    public async Task<IActionResult> AddFriendWithWish([FromBody] AddFriendWithWishDTO request)
    {
        var result = await _friendService.AddFriendWithWishAsync(request);
        return result ? Ok(new { message = "Friend added successfully" }) : BadRequest(new { message = "Failed to add friend with wish" });
    }
}
