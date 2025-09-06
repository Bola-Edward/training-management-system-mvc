using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Repositories.Interfaces
{
    public interface IGradeRepository : IBaseRepository<Grade>
    {
        Task<IEnumerable<Grade>> GetAllWithTraineeAndCourseAsync();
        Task<Grade?> GetByIdWithIncludesAsync(int id);
    }
}
