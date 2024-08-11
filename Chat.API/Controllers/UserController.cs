using Microsoft.AspNetCore.Mvc;
using Chat.BLL.Logic.Contracts.Users;
using Chat.BLL.Logic.Users;
using Chat.Common.Entities.User.DB;
using Chat.Common.Entities.User.InputModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chat.Api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;
        //private readonly IAuthLogic _authLogic;

        private readonly ILogger<UserController> _logger;

        public UserController(IUserLogic userLogic,
            //IAuthLogic authorizationLogic,
            ILogger<UserController> logger)
        {
            _userLogic = userLogic;

            _logger = logger;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _userLogic.Get();
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task PostAsync([FromBody] UserInputModel user)
        {
            await _userLogic.CreateUserAsync(user);
        }


        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(Guid id)
        {
            return await _userLogic.Get(id);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task PutAsync(int id, [FromBody] string value)
        {
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserInputModel request)
        {
            if (request == null || string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
            {
                var errorText = "Login and password must be provided.";
                _logger.LogError(errorText);
                return BadRequest(errorText);
            }

            var token = await _userLogic.GetJwtAsync(request.Login, request.Password);

            return Ok(token);
        }
    }
}
