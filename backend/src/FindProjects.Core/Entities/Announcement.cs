using FindProjects.Core.Entities.Base;

namespace FindProjects.Core.Entities;

public class Announcement : Entity
{
    public int UserId { get; set; }
    public User? User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Skill> Skills { get; set; } = [];
}