using AutoMapper;
using FirstProject.Data;
using FirstProject.Models.Domain;
using FirstProject.Models.DTO;
using FirstProject.Queries;
using FirstProject.StorageRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FirstProject.Controllers
{
    //The ModelState.IsValid property is only executed, if ApiController attribute is
    //missing. It is a form of Explicit model validation.

    //If Implicit Model validation fails, as a result of ApiController attribute
    //It will return a response with a dictionary of model property errors.
    //So if the validation fails, the controller won't execute the controller business logic.

    [Route("[controller]")]
    [ApiController]
    public class ProjectTasksController : ControllerBase
    {
        private readonly IProjectTaskStorageRepository sqlProjectTaskStorageRepository;
        private readonly IMapper mapper;

        public ProjectTasksController(IProjectTaskStorageRepository sqlProjectTaskStorageRepository, IMapper mapper)
        {
            this.sqlProjectTaskStorageRepository = sqlProjectTaskStorageRepository;
            this.mapper = mapper;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProjectTasksQueryParameters? projectTasksQueryParameters)
        {   
            try
            {
                var projectTasks = await sqlProjectTaskStorageRepository.GetAllAsync(projectTasksQueryParameters);
                var projectTaskDtos = mapper.Map<List<ProjectTaskDto>>(projectTasks);

                return Ok(new { Message = "got all project tasks", Data = projectTaskDtos });
            }

            catch (DbUpdateException exception)
            {
                return StatusCode(500, new { Message = "Error occured during the database insert" });
            }

            catch (Exception exception)
            {
                return StatusCode(500, new { Message = "Error occured during the processing of the request" });
            }
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var task = await sqlProjectTaskStorageRepository.GetByIdAsync(id);

                if (task == null)
                {
                    return NotFound(new { Message = $"Not found with {id} id" });
                }

                var taskDto = mapper.Map<ProjectTaskDto>(task);

                return Ok(new { Message = $"retrieved by {id} id", Data = taskDto});
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
        public async Task<IActionResult> InsertSingle([FromBody] InsertProjectTaskDto insertProjectTaskDto)
        {
            var projectTask = mapper.Map<ProjectTask>(insertProjectTaskDto);
            /*new ProjectTask()
            {
                Title = insertProjectTaskDto.Title,
                Description = insertProjectTaskDto.Description
            };*/

            projectTask = await sqlProjectTaskStorageRepository.InsertSingleAsync(projectTask);

            if (projectTask == null)
            {
                return NotFound(new { Message = "Insertion failed" });
            }

            var projectTaskDto = mapper.Map<ProjectTaskDto>(projectTask);

            return Created("/projecttasks/" + projectTask.Id, new { Message = "Task created", Data = projectTaskDto });
        }

        /*[HttpPost]
        public async Task<IActionResult> InsertSingle([FromBody] ProjectTask ProjectTask)
        {
           if(!ModelState.IsValid)
           {
                return BadRequest(new { Message = "Implicit validation failed",
                    errors = ModelState.Values.SelectMany(item => item.Errors)
                    errorCount = ModelState.ErrorCount
                });
           }

            return Content("Valid model");
        }*/

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateById([FromBody] InsertProjectTaskDto insertProjectTaskDto,[FromRoute] Guid id) {

            try
            {
                var projectTask = mapper.Map<ProjectTask>(insertProjectTaskDto);

                projectTask = await sqlProjectTaskStorageRepository.UpdateByIdAsync(id, projectTask);

                if (projectTask == null)
                {
                    return NotFound(new { Message = "Update failed" });
                }

                var projectTaskDto = mapper.Map<ProjectTaskDto>(projectTask);

                return Ok(new { Message = "Project task updated", Data = projectTaskDto });
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
                var projectTask = await sqlProjectTaskStorageRepository.DeleteByIdAsync(id);

                if (projectTask == null)
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
