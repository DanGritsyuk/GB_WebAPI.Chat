using Microsoft.AspNetCore.Http;
using Chat.Common.Entities.User.DB;
using Chat.Common.Entities.User.InputModels;

namespace Chat.BLL.Logic.Contracts.Users
{
    public interface IUserLogic
    {
        Task CreateUserAsync(UserInputModel userInputModel);

        Task<IResult> GetJwtAsync(string login, string password);

        Task<List<User>> Get();

        Task<User> Get(Guid id);
    }
}
