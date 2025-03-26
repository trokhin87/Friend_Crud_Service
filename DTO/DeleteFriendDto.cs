using Swashbuckle.AspNetCore.Annotations;

namespace DTO;

public class DeleteFriendDto
{
    [SwaggerSchema("Идентификатор приложения")]
    public Guid AppId { get; set; }

    [SwaggerSchema("Имя друга")]
    public string FriendUsername { get; set; } = null!;
}