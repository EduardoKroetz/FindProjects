using AutoMapper;
using FindProjects.Application.DTOs.Projects;
using FindProjects.Core.Entities;

namespace FindProjects.Application.DTOs;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Project, GetProjectDto>().ReverseMap();
    }
}