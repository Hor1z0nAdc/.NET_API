using FirstProject.Models.Domain;
using FirstProject.Queries;

namespace FirstProject.StorageRepositories
{
    public interface IProjectStorageRepository
    {
        public Task<List<Project>> GetAllAsync();

        public Task<Project> GetByIdAsync(Guid id);

        public Task<Project> InsertSingleAsync(Project project);

        public Task<Project?> UpdateByIdAsync(Guid id, Project project);

        public Task<Project> DeleteByIdAsync(Guid id);
    }
}
