using TrainingManagementSystem.Data;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace TrainingManagementSystem.Repositories.Implementations
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        private readonly ApplicationDbContext cont;

        public SessionRepository(ApplicationDbContext context) : base(context)
        {
            cont = context;
        }

        public async Task<IEnumerable<Session>> GetAllWithCourseAsync()
        {
            return await cont.Sessions
                .Include(s => s.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<Session>> SearchByCourseNameAsync(string courseName)
        {
            return await cont.Sessions
                                 .Include(s => s.Course)
                                 .Where(s => s.Course.CourseName.Contains(courseName))
                                 .ToListAsync();
        }
    }
}
