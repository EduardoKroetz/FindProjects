using FindProjects.API.Extensions;
using FindProjects.Application.DTOs.Projects;
using FindProjects.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindProjects.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost, Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] EditorProjectDto editorProjectDto)
    {
        var userId = User.GetUserId();
        var result = await _projectService.CreateProjectAsync(editorProjectDto, userId);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Created($"api/projects/{result.Data?.ProjectId}", result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _projectService.GetProjectByIdAsync(id);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }
    
    
}