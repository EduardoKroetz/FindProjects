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
        var result = await _projectService.CreateProjectAsync(editorProjectDto, User);
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
    
    [HttpPut("{id:int}"), Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAsync(int id, EditorProjectDto editorProjectDto)
    {
        var result = await _projectService.UpdateProjectAsync(id, editorProjectDto, User);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }

        return NoContent();
    }
    
    [HttpDelete("{id:int}"), Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id, User);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }

        return NoContent();
    }
    
    [HttpPatch("finish/{id:int}"), Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> FinishAsync(int id)
    {
        var result = await _projectService.FinishProjectAsync(id, User);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }

        return NoContent();
    }
}