using Swashbuckle.AspNetCore.Annotations;

namespace DTO;

public class AddIntAndPozhDto
{
    [SwaggerSchema("Идентификатор поздравления")]
    public int IdPozdr { get; set; }

    [SwaggerSchema("Интересы")]
    public string? Interests { get; set; }

    [SwaggerSchema("Пожелания")]
    public string? Pozhelania { get; set; }
}