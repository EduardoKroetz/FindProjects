using System.ComponentModel.DataAnnotations;

namespace FindProjects.Application.DTOs.Projects;

public class EditorProjectDto
{
    [Required(ErrorMessage = "Informe o título do projeto")]
    [MaxLength(100 ,ErrorMessage = "O título deve possuir no máximo 100 caracteres")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Informe a descrição do projeto")]
    [MaxLength(3000, ErrorMessage = "A descrição deve possuir no máximo 3000 caracteres")]
    public string Description { get; set; }
    
    public DateTime? DeadLine { get; set; }
    public bool HasBudget { get; set; }
    public decimal? Budget { get; set; }
    public int MaxContributors { get; set; } = 999;
    public string? ProjectLink { get; set; }
    public int[] SkillsIds { get; set; } = [];
    public int[] CategoriesIds { get; set; } = [];
}