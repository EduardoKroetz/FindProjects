using FindProjects.Core.Enums;

namespace FindProjects.Application.DTOs.Projects;

public class GetProjectDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    public string Status { get; set; }
    public DateTime? DeadLine { get; set; }
    public bool HasBudget { get; set; }
    public decimal? Budget { get; set; }
    public int MaxContributors { get; set; } = 999;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ProjectLink { get; set; }
}