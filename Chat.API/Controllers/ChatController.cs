using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        // Временное хранилище для сообщений
        private static readonly List<string> Messages = new List<string>();

        // Получить все сообщения
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetMessages()
        {
            return Ok(Messages);
        }

        // Отправить сообщение
        [HttpPost]
        public ActionResult SendMessage([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Сообщение не может быть пустым.");
            }

            Messages.Add(message);
            return Ok("Сообщение отправлено.");
        }
    }
}