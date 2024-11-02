using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Projects;
using FindProjects.Application.DTOs.Responses;

namespace FindProjects.Application.Services.Interfaces;

public interface IProjectService
{
    Task<ResultDto<CreateProjectResponse>> CreateProjectAsync(EditorProjectDto editorProjectDto, string userId);
}