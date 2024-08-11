using EmailService.Common.Contracts;

namespace Chat.BLL.Logic.Contracts.Notififcation
{
    public interface INotificationLogic
    {
        Task SendAsync(EmailServiceMessage message);
    }
}
