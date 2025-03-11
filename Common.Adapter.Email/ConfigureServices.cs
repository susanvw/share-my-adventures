using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace Common.Adapter.Email;

/// <summary>
/// Provides extension methods to configure email services in the dependency injection container.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Adds email sending services with SendGrid integration to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The configuration containing SendGrid settings.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the SendGrid configuration section is missing or invalid.</exception>
    public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var sendGridSection = configuration.GetSection("SendGrid");
        if (!sendGridSection.Exists())
        {
            throw new ArgumentException("SendGrid configuration section is missing.", nameof(configuration));
        }

        services.AddSendGrid(options =>
        {
            options.ApiKey = sendGridSection["ApiKey"] ?? throw new ArgumentException("SendGrid ApiKey is not configured.");
        });

        services.Configure<SendGridOptions>(sendGridSection);
        services.AddTransient<IEmailSender, EmailSender>();

        return services;
    }
}