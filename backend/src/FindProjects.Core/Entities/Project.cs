using FindProjects.Core.Entities.Base;
using FindProjects.Core.Enums;

namespace FindProjects.Core.Entities;

public class Project : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    public User? User { get; set; }
    public EProjectStatus Status { get; set; }
    public DateTime DeadLine { get; set; }
    public bool HasBudget { get; set; }
    public decimal? Budget { get; set; }
    public int MaxContributors { get; set; } = 999;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ProjectLink { get; set; }
    
    public ICollection<Skill> Skills { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Contributor> Contributors { get; set; } = [];
    public ICollection<ProjectMessage> ProjectMessages { get; set; } = [];
}