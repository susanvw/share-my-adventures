using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Infrastructure.Persistence;
using ShareMyAdventures.Infrastructure.Persistence.Interceptors;
using ShareMyAdventures.Infrastructure.Repositories;
using ShareMyAdventures.Infrastructure.Services;

namespace ShareMyAdventures.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        // Access the database connection string and replace the placeholder with the secret
        var dbPassword = configuration["DbPassword"];
        var connection = configuration.GetConnectionString("AdventureConnection").ThrowIfNullOrWhiteSpace("Connection String");


        // connection string
        var dbConnectionString = connection.Replace("{DbPassword}", dbPassword);


        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(dbConnectionString,
                   sqlOptions => { sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null); }));


        services.Configure<DataProtectionTokenProviderOptions>(opt =>
             opt.TokenLifespan = TimeSpan.FromHours(2));


        services.Configure<IdentityOptions>(options =>
        {
            // Default Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // Default Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 0;
            options.Password.RequiredUniqueChars = 1;
        });




        services.AddIdentity<Participant, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); 

        var section = configuration.GetSection(nameof(JwtOptions));
        services.Configure<JwtOptions>(section);


        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ITokenService, TokenService>();

        services.AddScoped<IAdventureRepository, AdventureRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();

        return services;
    }
}