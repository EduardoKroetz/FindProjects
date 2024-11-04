using System.Security.Claims;
using FindProjects.Application.DTOs;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Entities;
using FindProjects.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FindProjects.Application.Services;

public class ContributorService : IContributorService
{
    public ContributorService(ILogger<ContributorService> logger, IContributorRepository contributorRepository, UserManager<User> userManager, IProjectRepository projectRepository)
    {
        _logger = logger;
        _contributorRepository = contributorRepository;
        _userManager = userManager;
        _projectRepository = projectRepository;
    }

    private readonly ILogger<ContributorService> _logger;
    private readonly IContributorRepository _contributorRepository;
    private readonly UserManager<User> _userManager;
    private readonly IProjectRepository _projectRepository;
    
    public async Task<ResultDto<int>> CreateContributorAsync(int projectId, string userId, ClaimsPrincipal authenticatedUserClaims)
    {
        //Buscar usuário
        var authenticatedUser = await _userManager.GetUserAsync(authenticatedUserClaims);
        if (authenticatedUser == null)
        {
            return ResultDto<int>.BadResult("Usuário não encontrado", 404);
        }

        //Buscar projeto
        var project = await _projectRepository.GetProjectWithRelations(projectId);
        if (project == null)
        {
            _logger.LogWarning($"Projeto {projectId} não encontrado");
            return ResultDto<int>.BadResult("Projeto não encontrado", 404);
        }

        //Verificar se o usuário possui permissão para deletar o projeto
        if (authenticatedUser.Id != project.UserId)
        {
            _logger.LogWarning($"Usuário {authenticatedUser.Id} não possui permissão para alterar o projeto {projectId}");
            return ResultDto<int>.BadResult("Você não tem permissão para acessar esse recurso", 403);
        }
        
        //Buscar e verificar o usuário contribuidor
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return ResultDto<int>.BadResult($"Usuário {userId} não encontrado", 404);
        }

        //Verificar se o usuário já é um contribuidor do projeto
        if (project.Contributors.Any(x => x.UserId == userId))
        {
            _logger.LogWarning($"O usuário {userId} já é um contribuidor do projeto {projectId}");
            return ResultDto<int>.BadResult($"O usuário já é um contribuidor do projeto");
        }
        
        //Adicionar contribuidor ao projeto
        var contributor = new Contributor(userId, projectId);
        await _contributorRepository.AddAsync(contributor);
        
        _logger.LogInformation($"Usuário {userId} adicionado como contribuidor {contributor.Id} ao projeto {projectId}");
        
        return ResultDto<int>.SuccessResult(contributor.Id, 201);
    }
}