using FindProjects.Core.Entities.Base;

namespace FindProjects.Core.Entities;

public class Skill : Entity
{
    public string Name { get; set; }
    public ICollection<Project> Projects { get; set; } = [];
}