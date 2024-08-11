using Microsoft.Extensions.Logging;
using Chat.Common.Entities.User.DB;
using Chat.DAL.Repository.Contracts;

namespace Chat.DAL.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger) 
            : base(logger)
        {
            _logger = logger;
        }

        public async Task AddAsync(User user) =>
            await ExecuteWithLoggingAsync("SELECT * FROM public.post_user(@_login, @_password, @_name, @_surname)",
                new
                {
                    _login = user.Login,
                    _password = user.Password,
                    _name = user.Name,
                    _surname = user.Surname
                }, 
                "Ошибка при сохранении юзера");

        public async Task<List<User>> GetAllAsync() => 
            await QueryWithLoggingAsync<User>("SELECT userid AS Id, userlogin AS Login, userpassword AS Password FROM public.users", 
                "Ошибка при получении списка пользователей");      

        public async Task<User?> GetAsync(Guid id) =>
            await GetUserAsync($"SELECT userid AS Id, userlogin AS Login, userpassword AS Password FROM public.users WHERE userid = {id}");

        public async Task<User?> GetAsync(string login) => 
            await GetUserAsync($"SELECT userid AS Id, userlogin AS Login, userpassword AS Password FROM public.users WHERE userlogin = {login}");

        private async Task ExecuteWithLoggingAsync(string sql, object param, string errorMessage)
        {
            try
            {
                await ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMessage);
                throw;
            }
        }

        private async Task<List<T>> QueryWithLoggingAsync<T>(string sql, string errorMessage)
        {
            try
            {
                var result = await QueryAsync<T>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMessage);
                throw;
            }
        }

        private async Task<User?> GetUserAsync(string sql)
        {
            try
            {
                return await QuerySingleAsync<User>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пользователя");
                throw;
            }
        }
    }
}