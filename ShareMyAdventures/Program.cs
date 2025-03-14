using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using ShareMyAdventures;
using ShareMyAdventures.Application;
using ShareMyAdventures.Application.Common.Middleware;
using ShareMyAdventures.Infrastructure;
using ShareMyAdventures.Infrastructure.Persistence;
using ShareMyAdventures.Infrastructure.SignalR;
using System.Diagnostics;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("LogDefaultConnection");

if (connectionString != null) {
    // Access the database connection string and replace the placeholder with the secret
    var dbPassword = builder.Configuration["DbPassword"];
    // connection string
    connectionString = connectionString.Replace("{DbPassword}", dbPassword);

    Log.Logger = new LoggerConfiguration()
     .WriteTo.Console(new RenderedCompactJsonFormatter())
     .Enrich.FromLogContext()
        .WriteTo.Conditional(x => x.Level == LogEventLevel.Error, conf => conf.MSSqlServer(connectionString,
       new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Error" }))
          .WriteTo.Conditional(x => x.Level == LogEventLevel.Warning, conf => conf.MSSqlServer(connectionString,
              new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Warning" }))
           .WriteTo.Conditional(x => x.Level == LogEventLevel.Debug, conf => conf.MSSqlServer(connectionString,
               new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Debug" }))
           .WriteTo.Conditional(x => x.Level == LogEventLevel.Information, conf => conf.MSSqlServer(connectionString,
               new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Information" }))
           .WriteTo.Conditional(x => x.Level == LogEventLevel.Verbose, conf => conf.MSSqlServer(connectionString,
              new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Verbose" }))
        .CreateBootstrapLogger();
}

Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddEmail(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddWebUIServices(builder.Configuration);

var MyAllowSpecificOrigins = "_websiteOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:8100");
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();

                          policy.AllowCredentials(); // Allow credentials (cookies)
                          policy.WithMethods("OPTIONS");
                      });
});

builder.Services.AddSignalR();


var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>(); 
app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ValidationMiddleware>();



// register routes
app.UseRouting();


using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dataContext.Database.MigrateAsync();

    await dataContext.Database.EnsureCreatedAsync();

    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
    initializer.SeedEnums();
    await initializer.SeedDefaultUserAsync();

}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

app.UseHealthChecks("/health");
app.UseHttpsRedirection();


app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./v1/swagger.json", "Share my Adventures");
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();

app.UseCors(MyAllowSpecificOrigins);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");

app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true;

    // Attach additional properties to the request completion event
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("UserId", httpContext.User?.FindFirstValue(ClaimTypes.Name));
        diagnosticContext.Set("IP", httpContext.Connection.RemoteIpAddress?.ToString());

        options.GetLevel = (httpContext, elapsed, ex) =>
        {
            // Your condition for determining whether to log the request or not
            if (httpContext.Response.StatusCode >= 400)
            {
                return LogEventLevel.Error;
            }
            return LogEventLevel.Information;
        };
    };

});

await app.RunAsync();