using Swashbuckle.AspNetCore.Annotations;

namespace DTO;

public class AddFriendWithWishDTO
{
    [SwaggerSchema("Данные о друге")]
    public FriendDTO Friend { get; set; }

    [SwaggerSchema("Интересы и пожелания")]
    public AddIntAndPozhDto Pozh { get; set; }
}