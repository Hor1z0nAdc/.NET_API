using AutoMapper;
using FirstProject.Models.Domain;
using FirstProject.Models.DTO;
using FirstProject.Queries;
using FirstProject.StorageRepositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectStorageRepository sqlProjectStorageRepository;
        private readonly IMapper mapper;

        public ProjectsController(IProjectStorageRepository sqlProjectTaskStorageRepository, IMapper mapper)
        {
            this.sqlProjectStorageRepository = sqlProjectTaskStorageRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var projects= await sqlProjectStorageRepository.GetAllAsync();
                var projectDtos = mapper.Map<List<ProjectDto>>(projects);

                return Ok(new { Message = "got all projects", Data = projectDtos });
            }

            catch (DbUpdateException exception)
            {
                return StatusCode(500, new { Message = "Error occured during the database query " });
            }

            catch (Exception exception)
            {
                return StatusCode(500, new { Message = "Error occured during the processing of the request " + exception });
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var project = await sqlProjectStorageRepository.GetByIdAsync(id);

                if (project == null)
                {
                    return NotFound(new { Message = $"Not found with {id} id" });
                }

                var projectDto = mapper.Map<ProjectDto>(project);

                return Ok(new { Message = $"retrieved by {id} id", Data = projectDto });
            }

            catch (DbUpdateException exception)
            {
                return StatusCode(500, new { Message = "Error occured during the fetch operation" });
            }

            catch (Exception exception)
            {
                return StatusCode(500, new { Message = "Error occured during the processing of the request" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertSingle([FromBody] InsertProjectDto insertProjectDto)
        {
            var project = mapper.Map<Project>(insertProjectDto);
            /*new ProjectTask()
            {
                Title = insertProjectTaskDto.Title,
                Description = insertProjectTaskDto.Description
            };*/

            project = await sqlProjectStorageRepository.InsertSingleAsync(project);

            if (project == null)
            {
                return NotFound(new { Message = "Insertion failed" });
            }

            var projectDto = mapper.Map<ProjectDto>(project);

            return Created("/projects/" + project.Id, new { Message = "Project created", Data = projectDto });
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateById([FromBody] InsertProjectDto insertProjectDto, [FromRoute] Guid id)
        {

            try
            {
                var project= mapper.Map<Project>(insertProjectDto);

                project = await sqlProjectStorageRepository.UpdateByIdAsync(id, project);

                if (project== null)
                {
                    return NotFound(new { Message = "Update failed" });
                }

                var projectDto = mapper.Map<ProjectDto>(project);

                return Ok(new { Message = "Project updated", Data = projectDto });
            }

            catch (DbUpdateException exception)
            {
                return StatusCode(500, new { Message = "Error occured during the database update" });
            }

            catch (Exception exception)
            {
                return StatusCode(500, new { Message = "Error occured during the processing of the request" });
            }
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            try
            {
                var project = await sqlProjectStorageRepository.DeleteByIdAsync(id);

                if (project == null)
                {
                    return NotFound(new { Message = "Delete failed" });
                }

                return Ok(new { Message = $"Deleted by {id} id" });
            }

            catch (DbUpdateException exception)
            {
                return StatusCode(500, new { Message = "Error occured during the database update" });
            }

            catch (Exception exception)
            {
                return StatusCode(500, new { Message = "Error occured during the processing of the request" });
            }
        }
    }
}
