using System.Security.Claims;
using AutoMapper;
using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Projects;
using FindProjects.Application.DTOs.Responses;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Common;
using FindProjects.Core.Entities;
using FindProjects.Core.Enums;
using FindProjects.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FindProjects.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly ILogger<ProjectService> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
 
    public ProjectService(IProjectRepository projectRepository, ICategoryRepository categoryRepository, ISkillRepository skillRepository, ILogger<ProjectService> logger, IMapper mapper, UserManager<User> userManager)
    {
        _projectRepository = projectRepository;
        _categoryRepository = categoryRepository;
        _skillRepository = skillRepository;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<ResultDto<CreateProjectResponse>> CreateProjectAsync(EditorProjectDto editorProjectDto, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            return ResultDto<CreateProjectResponse>.BadResult("Usuário não encontrado");
        }
        
        //Buscar habilidades e categorias do projeto
        var skills = await _skillRepository.GetSkillsByIds(editorProjectDto.SkillsIds.ToList());
        var categories = await _categoryRepository.GetCategoriesByIds(editorProjectDto.CategoriesIds.ToList());
        
        //Criar novo projeto
        var project = new Project
        (
             editorProjectDto.Title,
             editorProjectDto.Description,
             user,
             editorProjectDto.DeadLine,
             editorProjectDto.ProjectLink,
             editorProjectDto.HasBudget,
             editorProjectDto.Budget,
             editorProjectDto.MaxContributors
        );
        await _projectRepository.AddAsync(project);
        
        _logger.LogInformation($"Projeto {project.Id} criado");
        
        return ResultDto<CreateProjectResponse>.SuccessResult(new CreateProjectResponse(project.Id), 201);
    }

    public async Task<ResultDto<GetProjectDto>> GetProjectByIdAsync(int projectId)
    {
        var project = await _projectRepository.GetById(projectId);
        if (project == null)
        {
            _logger.LogWarning($"Projeto {projectId} não encontrado");
            return ResultDto<GetProjectDto>.BadResult("Projeto não encontrado", 404);
        }

        var projectDto = _mapper.Map<GetProjectDto>(project);
        
        _logger.LogInformation($"Projeto {projectId} obtido");
        
        return ResultDto<GetProjectDto>.SuccessResult(projectDto);
    }
    
    public async Task<ResultDto<object?>> UpdateProjectAsync(int projectId, EditorProjectDto editorProjectDto, ClaimsPrincipal claimsPrincipal)
    {
        var projectResult = await GetProjectAndVerifyUserPermission(projectId, claimsPrincipal);
        if (!projectResult.Success)
        {
            return ResultDto<object?>.BadResult(projectResult.Errors, projectResult.StatusCode);
        }
        
        //Buscar habilidades e categorias do projeto
        var skills = await _skillRepository.GetSkillsByIds(editorProjectDto.SkillsIds.ToList());
        var categories = await _categoryRepository.GetCategoriesByIds(editorProjectDto.CategoriesIds.ToList());

        //Atualizar informações do projeto
        var project = projectResult.Data!;
        project.Skills = skills;
        project.Categories = categories;
        var updateResult = project.Update
        (
            editorProjectDto.Title,
            editorProjectDto.Description,
            editorProjectDto.DeadLine,
            editorProjectDto.ProjectLink,
            editorProjectDto.HasBudget,
            editorProjectDto.Budget,
            editorProjectDto.MaxContributors
        );
        
        //Verificar atualização
        if (updateResult.IsSuccess == false)
            return ResultDto<object?>.BadResult(updateResult.ErrorMessage!);
        
        //Salvar atualização
        await _projectRepository.UpdateAsync(project);
        
        _logger.LogInformation($"Projeto {projectId} atualizado");
        
        return ResultDto<object?>.SuccessResult(null, 204);
    }

    public async Task<ResultDto<object?>> DeleteProjectAsync(int projectId, ClaimsPrincipal claimsPrincipal)
    {
        var projectResult = await GetProjectAndVerifyUserPermission(projectId, claimsPrincipal);
        if (!projectResult.Success)
        {
            return ResultDto<object?>.BadResult(projectResult.Errors, projectResult.StatusCode);
        }

        //Remover projeto
        var project = projectResult.Data!;
        await _projectRepository.RemoveAsync(project);
        
        _logger.LogInformation($"Projeto {projectId} deletado");
        
        return ResultDto<object?>.SuccessResult(null, 204);
    }
    
    public async Task<ResultDto<object?>> FinishProjectAsync(int projectId, ClaimsPrincipal claimsPrincipal)
    {
        var projectResult = await GetProjectAndVerifyUserPermission(projectId, claimsPrincipal);
        if (!projectResult.Success)
        {
            return ResultDto<object?>.BadResult(projectResult.Errors, projectResult.StatusCode);
        }
        
        //Atualizar status para 'Finished'
        var project = projectResult.Data!;
        project.UpdateStatus(EProjectStatus.Finished);
        await _projectRepository.UpdateAsync(project);
        
        _logger.LogInformation($"Projeto {projectId} finalizado");
        
        return ResultDto<object?>.SuccessResult(null, 204);
    }
    
    
    /// <summary>
    /// Buscar e verificar o usuário autenticado e sua permissão de alteração no projeto
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="claimsPrincipal"></param>
    /// <returns></returns>
    private async Task<ResultDto<Project>> GetProjectAndVerifyUserPermission(int projectId, ClaimsPrincipal claimsPrincipal)
    {
        //Buscar usuário
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            return ResultDto<Project>.BadResult("Usuário não encontrado", 404);
        }

        //Buscar projeto
        var project = await _projectRepository.GetById(projectId);
        if (project == null)
        {
            _logger.LogWarning($"Projeto {projectId} não encontrado");
            return ResultDto<Project>.BadResult("Projeto não encontrado", 404);
        }

        //Verificar se o usuário possui permissão para deletar o projeto
        if (user.Id != project.UserId)
        {
            _logger.LogWarning($"Usuário {user.Id} não possui permissão para alterar o projeto {projectId}");
            return ResultDto<Project>.BadResult("Você não tem permissão para acessar esse recurso", 403);
        }

        return ResultDto<Project>.SuccessResult(project);
    }
}