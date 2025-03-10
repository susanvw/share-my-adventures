
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces;
using ShareMyAdventures.Infrastructure.Persistence;
using ShareMyAdventures.Infrastructure.Services;
using System.Text;

namespace ShareMyAdventures;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseDeveloperPageExceptionFilter(); 
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUserService>();
        services.AddScoped<IDateTime, DateTimeService>(); 


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "Share My Adventures", Version = "v1" });
		});


		services
		 .AddApiVersioning(options =>
		 {
			 options.DefaultApiVersion = new ApiVersion(1, 0);
			 // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
			 options.ReportApiVersions = true;
		 }
		 )
		 .AddApiExplorer(options =>
		 {
			 // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
			 // note: the specified format code will format the version as "'v'major[.minor][-status]"
			 options.GroupNameFormat = "'v'VVV";

			 // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
			 // can also be used to control the format of the API version in route templates
			 options.SubstituteApiVersionInUrl = true;

			 //indicating whether a default version is assumed when a client does
			 // does not provide an API version.
			 options.AssumeDefaultVersionWhenUnspecified = true;
		 });

		services.AddAuthentication(options =>
		{
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(tokenOptions =>
		{
            var key = configuration["JwtOptions:SecretKey"];
			key = key.ThrowIfNullOrWhiteSpace("Jwt Key");

			tokenOptions.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
			};
		})
          .AddGoogle(googleOptions =>
          {
			  googleOptions.ClientId = configuration["Google:ClientId"];
              googleOptions.ClientSecret = configuration["Google:ClientSecret"];
          });


        services.AddAuthorization();
		 
        services.AddResponseCaching();

        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        return services;
    }
}
