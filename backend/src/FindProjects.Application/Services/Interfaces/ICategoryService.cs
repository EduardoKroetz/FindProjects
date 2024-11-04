using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Categories;
using FindProjects.Core.Entities;

namespace FindProjects.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<ResultDto<int>> CreateCategoryAsync(EditorCategoryDto editorCategoryDto);
    Task<ResultDto<IEnumerable<GetCategoryDto>>> GetAllCategoriesAsync();
}