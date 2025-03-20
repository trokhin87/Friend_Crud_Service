namespace DTO;

public class DeleteFriendDto
{
    public Guid AppId { get; set; }
    public string FriendUsername { get; set; } = null!; 
}