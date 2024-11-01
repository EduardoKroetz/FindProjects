using System.ComponentModel.DataAnnotations;

namespace FindProjects.Application.DTOs.Users;

public class RegisterUserDto
{
    [Required(ErrorMessage = "Informe o nome de usuário")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Informe o e-mail")]
    [EmailAddress(ErrorMessage = "Informe o e-mail em um formato correto")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Informe a senha")]
    [MinLength(6, ErrorMessage = "A senha deve possuir no mínimo 6 caracteres")]
    [MaxLength(32, ErrorMessage = "A senha deve possuir no máximo 32 caracteres")]
    public string Password { get; set; }
    
    public string? ProfileDescription { get; set; }
}