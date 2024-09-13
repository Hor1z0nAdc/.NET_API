using FirstProject.Data;
using FirstProject.Models.Domain;
using FirstProject.Queries;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.StorageRepositories.Implementation
{
    public class SqlProjectStorageRepository : IProjectStorageRepository
    {
        private readonly ProjectDbContext _dbContext;

        public SqlProjectStorageRepository(ProjectDbContext projectDbContext)
        {
            _dbContext = projectDbContext;
        }

        public async Task<Project> DeleteByIdAsync(Guid id)
        {
            var project = await _dbContext.Projects.SingleOrDefaultAsync(item => item.Id == id);

            if (project == null)
            {
                return null;
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            var projects = await _dbContext.Projects.Include(item => item.Tasks).ToListAsync();
            return projects;
        }

        public async Task<Project> GetByIdAsync(Guid id)
        {
            var project = await _dbContext.Projects.Include(item => item.Tasks).SingleOrDefaultAsync(item => item.Id == id);
            return project;
        }

        public async Task<Project> InsertSingleAsync(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<Project?> UpdateByIdAsync(Guid id, Project project)
        {
            var projectSingle = await _dbContext.Projects.SingleOrDefaultAsync(item => item.Id == id);

            if (projectSingle == null)
            {
                return null;
            }

            projectSingle.NumOfDevelopers = project.NumOfDevelopers;
            projectSingle.Name = project.Name;
            await _dbContext.SaveChangesAsync();

            return projectSingle;
        }
    }
}
