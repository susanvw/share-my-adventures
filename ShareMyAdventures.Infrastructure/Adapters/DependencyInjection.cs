using Common.Adapter.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace ShareMyAdventures.Infrastructure.Adapters;

public static class DependencyInjection
{

    public static IServiceCollection AddSms(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        string? apiKey = configuration["Twilio"];

        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("SendGrid API key is not configured.");
        }

        services.AddSendGrid(options =>
        {
            options.ApiKey = apiKey;
        });

        services.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.Options));
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}
