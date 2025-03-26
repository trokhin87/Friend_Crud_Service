using Swashbuckle.AspNetCore.Annotations;

namespace DTO;

public class FriendDTO
{
    [SwaggerSchema("Идентификатор приложения")]
    public Guid AppId { get; set; }

    [SwaggerSchema("Имя пользователя")]
    public string FriendUsername { get; set; } = null!;

    [SwaggerSchema("Полное имя друга")]
    public string FriendName { get; set; } = null!;

    [SwaggerSchema("Дата рождения")]
    public DateOnly DateBirth { get; set; }
}