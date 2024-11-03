using AutoMapper;
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
    private readonly IMapper _mapper;
    
    public ProjectService(IProjectRepository projectRepository, ICategoryRepository categoryRepository, ISkillRepository skillRepository, ILogger<ProjectService> logger, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _categoryRepository = categoryRepository;
        _skillRepository = skillRepository;
        _logger = logger;
        _mapper = mapper;
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
        _logger.LogInformation($"Project {project.Id} created");
        
        return ResultDto<CreateProjectResponse>.SuccessResult(new CreateProjectResponse(project.Id), 201);
    }

    public async Task<ResultDto<GetProjectDto>> GetProjectByIdAsync(int projectId)
    {
        var project = await _projectRepository.GetById(projectId);
        if (project == null)
        {
            _logger.LogError($"Project {projectId} not found");
            return ResultDto<GetProjectDto>.BadResult("Projeto não encontrado", 404);
        }

        var projectDto = _mapper.Map<GetProjectDto>(project);

        return ResultDto<GetProjectDto>.SuccessResult(projectDto);
    }
}