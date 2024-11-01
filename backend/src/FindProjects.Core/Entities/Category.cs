using FindProjects.Core.Entities.Base;

namespace FindProjects.Core.Entities;

public class Category : Entity
{
    public string Name { get; set; }
    public ICollection<Project> Projects { get; set; } = [];
    public ICollection<Announcement> Announcements { get; set; } = [];

}