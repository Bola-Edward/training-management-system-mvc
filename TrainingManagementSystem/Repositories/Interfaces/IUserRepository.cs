using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAllInstructorsAsync();
    }
}
