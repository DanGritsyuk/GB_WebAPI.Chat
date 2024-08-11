using Confluent.Kafka;
using EmailService.Common.Contracts;
using HireWise.BLL.Logic.Services;
using Chat.BLL.Logic.Contracts.Kafka;
using Chat.BLL.Logic.Contracts.Notififcation;
using Chat.BLL.Logic.Contracts.Users;
using Chat.BLL.Logic.Kafka;
using Chat.BLL.Logic.Notification;
using Chat.BLL.Logic.Users;
using Chat.Common.Entities.HttpClientts;
using Chat.DAL.Repository;
using Chat.DAL.Repository.Contracts;
using Chat.DAL.Repository.EF;

namespace Chat.Api.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection ConfigureBLLDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<INotificationLogic, NotificationLogic>();

            services.AddScoped<IKafkaProducer<Ignore, EmailServiceMessage>>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<KafkaProducer<Ignore, EmailServiceMessage>>>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                var bootstrapServers = configuration.GetValue<string>("KafkaConfig:BootstrapServers");

                return new KafkaProducer<Ignore, EmailServiceMessage>(logger, bootstrapServers);
            });

            return services;
        }

        public static IServiceCollection ConfigureDALDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEFUserRepository, EFUserRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddScoped<PasswordService>();
            services.AddSingleton<PasswordHasher>();

            return services;
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddHttpClient(
                HttpClientNames.EMAIL_SERVICE,
                client =>
                {  
                    client.BaseAddress = new Uri("https://localhost:7212/api/EmailSender/send");
                });

            return services;
        }
    }
}
