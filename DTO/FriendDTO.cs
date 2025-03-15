namespace DTO;

public class FriendDTO
{
    public Guid AppId { get; set; }
    public string FriendUsername { get; set; } = null!;
    public string FriendName { get; set; } = null!;
    public DateOnly DateBirth { get; set; }
}