using Chat.Common.Entities.User.DB;

namespace Chat.DAL.Repository.Contracts
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User?> GetAsync(Guid id);
    }
}