using Microsoft.EntityFrameworkCore;
using Chat.Common.Entities.User.DB;

namespace Chat.DAL.Repository.EF
{
    public class EFUserRepository : IEFUserRepository
    {
        private readonly DBContext _dbContext;
        public EFUserRepository(DBContext context)
        {
            _dbContext = context;
        }

        public async Task Create(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetAsync(Guid userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task SomeWorkAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
