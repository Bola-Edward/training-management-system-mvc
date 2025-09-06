using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Repositories.Interfaces
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllWithCourseAsync();
        Task<IEnumerable<Session>> SearchByCourseNameAsync(string courseName);
    }
}
