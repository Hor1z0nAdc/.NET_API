using AutoMapper;
using FirstProject.Models.Domain;
using FirstProject.Models.DTO;

namespace FirstProject.MappingProfiles
{
    public class ProjectTaskProfile : Profile
    {
        public ProjectTaskProfile() {
            CreateMap<ProjectTask, ProjectTaskDto>().ReverseMap();
            CreateMap<ProjectTask, InsertProjectTaskDto>().ReverseMap();
            CreateMap<ProjectTask, ProjectTaskWithoutProjectDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project, ProjectWithoutTasksDto>().ReverseMap();
            CreateMap<Project, InsertProjectDto>().ReverseMap();

            //Calling ForMember method on CreateMap() allows custom mapping
        }
    }
}
