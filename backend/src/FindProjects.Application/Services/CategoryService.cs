using AutoMapper;
using FindProjects.Application.DTOs;
using FindProjects.Application.DTOs.Categories;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Entities;
using FindProjects.Core.Repositories;

namespace FindProjects.Application.Services;

public class CategoryService : ICategoryService
{
    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    

    public async Task<ResultDto<int>> CreateCategoryAsync(EditorCategoryDto editorCategoryDto)
    {
        var categoryExists = await _categoryRepository.GetByName(editorCategoryDto.Name);
        if (categoryExists != null)
        {
            return ResultDto<int>.BadResult("Essa categoria já existe");
        }

        var category = new Category(editorCategoryDto.Name);
        await _categoryRepository.AddAsync(category);

        return ResultDto<int>.SuccessResult(category.Id);
    }

    public async Task<ResultDto<IEnumerable<GetCategoryDto>>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var categoriesDto = _mapper.Map<IEnumerable<GetCategoryDto>>(categories);
        return ResultDto<IEnumerable<GetCategoryDto>>.SuccessResult(categoriesDto);
    }
}