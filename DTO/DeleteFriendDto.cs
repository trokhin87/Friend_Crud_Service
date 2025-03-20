namespace DTO;

public class DeleteFriendDto
{
    public Guid AppId { get; set; }
    public string FriendName { get; set; } = null!; 
}