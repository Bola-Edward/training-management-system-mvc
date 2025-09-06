using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Data;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Repositories.Interfaces;

namespace TrainingManagementSystem.Repositories.Implementations
{
    public class GradeRepository : BaseRepository<Grade>, IGradeRepository
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Grade>> GetAllWithTraineeAndCourseAsync()
        {
            return await _context.Grades
                .Include(g => g.User)
                .Include(g => g.Session)
                    .ThenInclude(s => s.Course)
                .ToListAsync();
        }

        public async Task<Grade?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Grades
                .Include(g => g.User)
                .Include(g => g.Session)
                    .ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(g => g.GradeId == id);
        }

    }
}
