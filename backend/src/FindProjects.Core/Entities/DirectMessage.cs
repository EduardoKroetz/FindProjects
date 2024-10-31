using FindProjects.Core.Entities.Base;

namespace FindProjects.Core.Entities;

public class DirectMessage : MessageBase
{
    public int ReceiverId { get; set; }
    public User? Receiver { get; set; }
}