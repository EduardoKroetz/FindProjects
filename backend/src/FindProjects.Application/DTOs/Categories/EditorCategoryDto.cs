using System.ComponentModel.DataAnnotations;

namespace FindProjects.Application.DTOs.Categories;

public class EditorCategoryDto
{
    [Required(ErrorMessage = "Informe o nome da categoria")]
    public string Name { get; set; }
}