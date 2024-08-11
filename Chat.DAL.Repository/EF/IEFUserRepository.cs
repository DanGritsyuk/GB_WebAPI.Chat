using Chat.Common.Entities.User.DB;

namespace Chat.DAL.Repository.EF
{
    public interface IEFUserRepository
    {
        Task Create(User user);
        Task<List<User>> GetAllAsync();
        Task<User?> GetAsync(Guid userId);
        Task SomeWorkAsync();
    }
}
