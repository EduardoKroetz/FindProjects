using FindProjects.Core.Entities.Base;

namespace FindProjects.Core.Entities;

public class ProjectMessage : MessageBase
{
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}