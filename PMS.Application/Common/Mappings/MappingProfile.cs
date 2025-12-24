using AutoMapper;
using PMS.Application.Projects.DTOs;
using PMS.Application.ProjectTasks.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<ProjectTask, ProjectTaskDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
    }

}