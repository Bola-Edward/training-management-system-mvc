using TrainingManagementSystem.Data;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace TrainingManagementSystem.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        private new readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllInstructorsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Instructor")
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }
    }
}
