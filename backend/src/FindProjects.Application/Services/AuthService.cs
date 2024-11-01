using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Responses;
using FindProjects.Application.DTOs.Users;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FindProjects.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthService> _logger;
    private readonly ITokenService _tokenService;
 
    public AuthService(UserManager<User> userManager, ILogger<AuthService> logger, ITokenService tokenService)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenService = tokenService;
    }
    
    public async Task<ResultDto<RegisterUserResponse>> RegisterAsync(RegisterUserDto registerUserDto)
    {
        //Verificar se o e-mail já está cadastrado
        var userExists = await _userManager.FindByEmailAsync(registerUserDto.Email);
        if (userExists != null)
        {
            _logger.LogError("E-mail já está cadastrado");
            return ResultDto<RegisterUserResponse>.BadResult("Esse e-mail já está cadastrado");
        }

        var user = new User
        {
            Email = registerUserDto.Email,
            UserName = registerUserDto.UserName,
            ProfileDescription = registerUserDto.ProfileDescription ?? string.Empty
        };
        
        //Salvar o usuário no banco de dados
        var result = await _userManager.CreateAsync(user, registerUserDto.Password);
        if (!result.Succeeded)
        {
            _logger.LogError(result.Errors.First().Description);
            return ResultDto<RegisterUserResponse>.BadResult("Não foi possível criar o usuário");
        }
        
        //Gerar e retornar token jwt
        var token = _tokenService.GenerateToken(user);
        _logger.LogInformation("Usuário registrado e token gerado com sucesso");
        return ResultDto<RegisterUserResponse>.SuccessResult(new RegisterUserResponse(token, user.Id), 201);
    }

    public async Task<ResultDto<string>> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
        if (user == null)
        {
            _logger.LogError($"Usuário com e-mail {loginUserDto.Email} não encontrado");
            return ResultDto<string>.BadResult("E-mail ou senha inválidos");
        }
        
        var verifyHashedPassword = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);
        if (verifyHashedPassword == PasswordVerificationResult.Failed)
        {
            _logger.LogError($"Senha {loginUserDto.Password} inválida para o e-mail {loginUserDto.Email}");
            return ResultDto<string>.BadResult("E-mail ou senha inválidos");
        }

        var token = _tokenService.GenerateToken(user);
        _logger.LogInformation($"Login realizado com sucesso para o usuário {loginUserDto.Email}");
        return ResultDto<string>.SuccessResult(token);
    }
}