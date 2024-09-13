using FirstProject.Data;
using FirstProject.Models.Domain;
using FirstProject.Models.DTO;
using FirstProject.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace FirstProject.StorageRepositories.Implementation
{
    public class SqlProjectTaskStorageRepository : IProjectTaskStorageRepository
    {
        private readonly ProjectDbContext _dbContext;

        public SqlProjectTaskStorageRepository(ProjectDbContext projectDbContext)
        {
            _dbContext = projectDbContext;
        }

        public async Task<ProjectTask> DeleteByIdAsync(Guid id)
        {
            var task = await _dbContext.Tasks.SingleOrDefaultAsync(item => item.Id == id);

            if (task == null)
            {
                return null;
            }

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

            return task;
        }


        public async Task<List<ProjectTask>> GetAllAsync(ProjectTasksQueryParameters? projectsQueryParameters)
        {
            var projectTasks =  _dbContext.Tasks.Include("Project").AsQueryable();

            if (!string.IsNullOrEmpty(projectsQueryParameters.FilterBy) && !string.IsNullOrEmpty(projectsQueryParameters.Contains))
            {
                if (projectsQueryParameters.FilterBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    projectTasks = projectTasks.Where(item => item.Title.Contains(projectsQueryParameters.Contains));
                }
            }

            return await projectTasks.ToListAsync();
        }


        public async Task<ProjectTask> GetByIdAsync(Guid id)
        {
            var task = await _dbContext.Tasks.Include(item => item.Project).SingleOrDefaultAsync(item => item.Id == id);
            return task;
        }


        public async Task<ProjectTask> InsertSingleAsync(ProjectTask projectTask)
        {
            await _dbContext.Tasks.AddAsync(projectTask);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(projectTask).Reference(c => c.Project).Load();

            return projectTask;
        }


        public async Task<ProjectTask?> UpdateByIdAsync(Guid id, ProjectTask projectTask)
        {
            var task = await _dbContext.Tasks.Include(item => item.Project).SingleOrDefaultAsync(item => item.Id == id);

            if (task == null)
            {
                return null;
            }

            task.Title = projectTask.Title;
            task.Description = projectTask.Description;
            await _dbContext.SaveChangesAsync();

            return task;
        }
    }
}
