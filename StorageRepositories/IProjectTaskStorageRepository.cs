using FirstProject.Models.Domain;
using FirstProject.Queries;

namespace FirstProject.StorageRepositories
{
    public interface IProjectTaskStorageRepository
    {
        public Task<List<ProjectTask>> GetAllAsync(ProjectTasksQueryParameters? projectsQueryParameters);

        public Task<ProjectTask> GetByIdAsync(Guid id);

        public Task<ProjectTask> InsertSingleAsync(ProjectTask projectTask);

        public Task<ProjectTask?> UpdateByIdAsync(Guid id, ProjectTask projectTask);

        public Task<ProjectTask> DeleteByIdAsync(Guid id);
    }
}
