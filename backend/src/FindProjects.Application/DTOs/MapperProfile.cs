using AutoMapper;
using FindProjects.Application.DTOs.Categories;
using FindProjects.Application.DTOs.Projects;
using FindProjects.Core.Entities;

namespace FindProjects.Application.DTOs;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Project, GetProjectDto>().ReverseMap();
        CreateMap<Category, GetCategoryDto>().ReverseMap();
    }
}