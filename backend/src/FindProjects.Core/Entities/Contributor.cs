using FindProjects.Core.Entities.Base;

namespace FindProjects.Core.Entities;

public class Contributor : Entity
{
    public Contributor(string userId, int projectId)
    {
        UserId = userId;
        ProjectId = projectId;
    }

    public string UserId { get; set; }
    public User? User { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}