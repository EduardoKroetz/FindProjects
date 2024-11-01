using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Responses;
using FindProjects.Application.DTOs.Users;

namespace FindProjects.Application.Services.Interfaces;

public interface IAuthService
{
    Task<ResultDto<RegisterUserResponse>> RegisterAsync(RegisterUserDto registerUserDto);
    Task<ResultDto<string>> LoginAsync(LoginUserDto loginUserDto);
}