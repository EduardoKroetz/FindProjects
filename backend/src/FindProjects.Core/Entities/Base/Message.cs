namespace FindProjects.Core.Entities.Base;

public class MessageBase : Entity
{
    public int SenderId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}