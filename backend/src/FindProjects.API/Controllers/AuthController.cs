using FindProjects.Application.DTOs.Users;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FindProjects.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync(RegisterUserDto registerUserDto)
    {
        var result = await _authService.RegisterAsync(registerUserDto);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }
        
        return Created($"api/users/{result.Data?.UserId}", result);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync(LoginUserDto loginUserDto)
    {
        var result = await _authService.LoginAsync(loginUserDto);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }
}