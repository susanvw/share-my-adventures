using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace Common.Adapter.Email;

public static class ConfigureServices
{

    public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        var sendGrid = configuration.GetSection("SendGrid"); 

        services.AddSendGrid(options =>
        {
            options.ApiKey = sendGrid["ApiKey"];
        });

        services.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.Options));
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}
