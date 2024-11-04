using FindProjects.Application.DTOs.Categories;
using FindProjects.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FindProjects.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(EditorCategoryDto editorCategoryDto)
    {
        var result = await _categoryService.CreateCategoryAsync(editorCategoryDto);
        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }

        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return Ok(result);
    }
}