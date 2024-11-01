using Microsoft.AspNetCore.Identity;

namespace FindProjects.Core.Entities;

public class User : IdentityUser
{
    public string ProfileDescription { get; set; }
    
    public ICollection<Project> Projects { get; set; } = [];
    public ICollection<Announcement> Announcements { get; set; } = [];
    public ICollection<ProjectMessage> ProjectMessages { get; set; } = [];
    public ICollection<DirectMessage> SentMessages { get; set; } = [];
    public ICollection<DirectMessage> ReceivedMessages { get; set; } = [];

}