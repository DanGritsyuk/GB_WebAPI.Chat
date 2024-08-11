using Microsoft.Extensions.Logging;
using Chat.BLL.Logic.Contracts.Users;
using Chat.Common.Entities.User.InputModels;
using Chat.Common.Entities.User.DB;
using Chat.DAL.Repository.Contracts;
using Chat.DAL.Repository.EF;
using System.Transactions;
using EmailService.Common.Contracts;
using Chat.BLL.Logic.Contracts.Notififcation;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HireWise.BLL.Logic.Services;
using HireWise.JWT;

namespace Chat.BLL.Logic.Users
{
    public class UserLogic : IUserLogic
    {

        private readonly IUserRepository _userRepository;
        private readonly IEFUserRepository _eFUserRepository;
        private readonly INotificationLogic _notification;
        private readonly JWTSettings _jwtSettings;
        private readonly PasswordService _passwordService;

        private readonly ILogger<UserLogic> _logger;

        public UserLogic(
            IUserRepository userRepository,
            IEFUserRepository eFUserRepository,
            INotificationLogic notification,
            JWTSettings jwtSettings,
            PasswordService passwordService,
        ILogger<UserLogic> logger)
        {
            _userRepository = userRepository;
            _eFUserRepository = eFUserRepository;
            _notification = notification;
            _jwtSettings = jwtSettings;
            _passwordService = passwordService;

            _logger = logger;
        }

        public async Task CreateUserAsync(UserInputModel userInputModel)
        {
            try
            {
                using(var transaction = new TransactionScope())
                ValidateUser(userInputModel);

                var user = new User
                {
                    Name = userInputModel.Name,
                    Email = userInputModel.Email,
                    Login = userInputModel.Login,
                    Password = _passwordService.HashPassword(userInputModel.Password),
                    Surname = userInputModel.Surname
                };

                await _eFUserRepository.Create(user);
                _logger.LogInformation($"Id user: {user.Id}");

                var emailMsg = new EmailServiceMessage("Вы зарегистрированы", "Поздравляю, Вы зарегестрированны!", user.Email);
                await _notification.SendAsync(emailMsg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при создании User");
                throw;
            }
        }


        public async Task<List<User>> Get()
        {
            return await _eFUserRepository.GetAllAsync();
        }

        public async Task<User> Get(Guid id)
        {
            var user = await _eFUserRepository.GetAsync(id);
            user.Name = "Tom";
            await _eFUserRepository.SomeWorkAsync();
            return user;
        }

        public async Task<IResult> GetJwtAsync(string login, string password)
        {
            // находим пользователя 
            var user = await _eFUserRepository.GetAsync(Guid.NewGuid());
            // если пользователь не найден или пароль не корректный, отправляем статусный код 401
            if (user is null
                || !_passwordService.VerifyPassword(password, user.Password))
                return Results.Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
            claims: claims,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(_jwtSettings.ExpiryMinutes)),
                signingCredentials: new SigningCredentials(_jwtSettings.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
             );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            _logger.LogInformation($"Generated token for user: {login}");

            // формируем ответ
            var response = new
            {
                access_token = encodedJwt,
                username = user.Login
            };

            return Results.Json(response);
        }

        private void ValidateUser(UserInputModel user)
        {
            List<string> exceptionsMessages = new List<string>();

            if (user == null)
            {
                exceptionsMessages.Add("User не может быть null");
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                exceptionsMessages.Add("Namee не может быть null или пустым");
            }

            if (exceptionsMessages.Any())
            {
                foreach (var exception in exceptionsMessages)
                {
                    _logger.LogError(exception);
                }
                throw new ArgumentException();
            }
        }
    }
}
