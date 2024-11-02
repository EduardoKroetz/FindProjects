using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Projects;
using FindProjects.Application.DTOs.Responses;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Entities;
using FindProjects.Core.Enums;
using FindProjects.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace FindProjects.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly ILogger<ProjectService> _logger;
    
    public ProjectService(IProjectRepository projectRepository, ICategoryRepository categoryRepository, ISkillRepository skillRepository, ILogger<ProjectService> logger)
    {
        _projectRepository = projectRepository;
        _categoryRepository = categoryRepository;
        _skillRepository = skillRepository;
        _logger = logger;
    }

    public async Task<ResultDto<CreateProjectResponse>> CreateProjectAsync(EditorProjectDto editorProjectDto, string userId)
    {
        //Buscar skills
        var skills = await _skillRepository.GetSkillsByIds(editorProjectDto.SkillsIds.ToList());
        
        //Buscar categorias
        var categories = await _categoryRepository.GetCategoriesByIds(editorProjectDto.CategoriesIds.ToList());
        
        var project = new Project
        {
            Title = editorProjectDto.Title,
            Description = editorProjectDto.Description,
            HasBudget = editorProjectDto.HasBudget,
            MaxContributors = editorProjectDto.MaxContributors,
            DeadLine = editorProjectDto.DeadLine,
            CreatedAt = DateTime.UtcNow,
            Budget = editorProjectDto.HasBudget ? editorProjectDto.Budget : null,
            ProjectLink = editorProjectDto.ProjectLink,
            UserId = userId,
            Status = EProjectStatus.Active,
            Skills = skills,
            Categories = categories
        };

        await _projectRepository.AddAsync(project);

        return ResultDto<CreateProjectResponse>.SuccessResult(new CreateProjectResponse(project.Id), 201);
    }
}