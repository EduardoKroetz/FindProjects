using System.Security.Claims;
using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Projects;
using FindProjects.Application.DTOs.Responses;

namespace FindProjects.Application.Services.Interfaces;

public interface IProjectService
{
    Task<ResultDto<CreateProjectResponse>> CreateProjectAsync(EditorProjectDto editorProjectDto, ClaimsPrincipal claimsPrincipal);
    Task<ResultDto<GetProjectDto>> GetProjectByIdAsync(int projectId);

    Task<ResultDto<object?>> UpdateProjectAsync(int projectId, EditorProjectDto editorProjectDto, ClaimsPrincipal claimsPrincipal);
}