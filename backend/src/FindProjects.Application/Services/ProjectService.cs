using System.Security.Claims;
using AutoMapper;
using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Projects;
using FindProjects.Application.DTOs.Responses;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Common;
using FindProjects.Core.Entities;
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
        
        //Buscar skills
        var skills = await _skillRepository.GetSkillsByIds(editorProjectDto.SkillsIds.ToList());
        
        //Buscar categorias
        var categories = await _categoryRepository.GetCategoriesByIds(editorProjectDto.CategoriesIds.ToList());
        
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
        _logger.LogInformation($"Projeto {projectId} obtido com sucesso");
        
        return ResultDto<GetProjectDto>.SuccessResult(projectDto);
    }
    
    public async Task<ResultDto<object?>> UpdateProjectAsync(int projectId, EditorProjectDto editorProjectDto, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            return ResultDto<object?>.BadResult("Usuário não encontrado", 404);
        }
        
        var project = await _projectRepository.GetById(projectId);
        if (project == null)
        {
            _logger.LogWarning($"Projeto {projectId} não encontrado");
            return ResultDto<object?>.BadResult("Projeto não encontrado", 404);
        }

        //Verificar se o usuário possui permissão para atualizar o projeto
        if (user.Id != project.UserId)
        {
            _logger.LogWarning($"Tentativa de atualização do projeto {projectId} pelo usuário {user.Id} falhou: usuário não possui permissão.");
            return ResultDto<object?>.BadResult("Você não tem permissão para acessar esse recurso", 403);
        }
        
        //Buscar skills
        var skills = await _skillRepository.GetSkillsByIds(editorProjectDto.SkillsIds.ToList());
        
        //Buscar categorias
        var categories = await _categoryRepository.GetCategoriesByIds(editorProjectDto.CategoriesIds.ToList());

        var result = project.Update
        (
            editorProjectDto.Title,
            editorProjectDto.Description,
            editorProjectDto.DeadLine,
            editorProjectDto.ProjectLink,
            editorProjectDto.HasBudget,
            editorProjectDto.Budget,
            editorProjectDto.MaxContributors
        );

        if (result.IsSuccess == false)
            return ResultDto<object?>.BadResult(result.ErrorMessage);

        project.Skills = skills;
        project.Categories = categories;
        
        await _projectRepository.UpdateAsync(project);
        
        _logger.LogInformation($"Projeto {projectId} atualizado");
        
        return ResultDto<object?>.SuccessResult(null, 204);
    }

    public async Task<ResultDto<object?>> DeleteProjectAsync(int projectId, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user == null)
        {
            return ResultDto<object?>.BadResult("Usuário não encontrado");
        }

        var project = await _projectRepository.GetById(projectId);
        if (project == null)
        {
            _logger.LogWarning($"Projeto {projectId} não encontrado");
            return ResultDto<object?>.BadResult("Projeto não encontrado");
        }

        //Verificar se o usuário possui permissão para deletar o projeto
        if (user.Id != project.UserId)
        {
            _logger.LogWarning($"Tentativa de deleção do projeto {projectId} pelo usuário {user.Id} falhou: usuário não possui permissão.");
            return ResultDto<object?>.BadResult("Você não tem permissão para acessar esse recurso", 403);
        }
        
        await _projectRepository.RemoveAsync(project);
        _logger.LogInformation($"Projeto {projectId} deletado");
        
        return ResultDto<object?>.SuccessResult(null, 204);
    }
}