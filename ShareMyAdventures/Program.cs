using Common.Adapter.Email; // Email service adapter
using Microsoft.EntityFrameworkCore; // EF Core for database operations
using Serilog; // Serilog logging
using Serilog.Events; // Log event levels
using Serilog.Formatting.Compact; // Compact JSON formatter for console
using Serilog.Sinks.MSSqlServer; // MSSqlServer sink for Serilog
using ShareMyAdventures;
using ShareMyAdventures.Infrastructure; // Infrastructure services
using ShareMyAdventures.Infrastructure.SignalR; // SignalR hub
using System.Diagnostics; // Debug output
using System.Security.Claims; // Claims for user info

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog logging
var connectionString = builder.Configuration.GetConnectionString("LogDefaultConnection");
if (connectionString != null)
{
    // Replace placeholder in connection string with password from configuration
    var dbPassword = builder.Configuration["DbPassword"];
    connectionString = connectionString.Replace("{DbPassword}", dbPassword);

    // Configure Serilog with console output and conditional MSSqlServer sinks per log level
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext() // Add context info to logs
        .WriteTo.Console(new RenderedCompactJsonFormatter()) // Compact JSON output to console
        .WriteTo.Conditional(
            x => x.Level == LogEventLevel.Error,
            conf => conf.MSSqlServer(
                connectionString,
                new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Error" }))
        .WriteTo.Conditional(
            x => x.Level == LogEventLevel.Warning,
            conf => conf.MSSqlServer(
                connectionString,
                new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Warning" }))
        .WriteTo.Conditional(
            x => x.Level == LogEventLevel.Debug,
            conf => conf.MSSqlServer(
                connectionString,
                new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Debug" }))
        .WriteTo.Conditional(
            x => x.Level == LogEventLevel.Information,
            conf => conf.MSSqlServer(
                connectionString,
                new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Information" }))
        .WriteTo.Conditional(
            x => x.Level == LogEventLevel.Verbose,
            conf => conf.MSSqlServer(
                connectionString,
                new MSSqlServerSinkOptions { AutoCreateSqlTable = true, TableName = "Verbose" }))
        .CreateBootstrapLogger(); // Bootstrap logger for startup
}

// Enable Serilog self-diagnostics for debugging
Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
Serilog.Debugging.SelfLog.Enable(Console.Error);

// Clear default logging providers and use Serilog
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// Use Serilog for host logging
builder.Host.UseSerilog();

// Add services to the DI container
builder.Services
    .AddEmail(builder.Configuration) // Email services
    .AddInfrastructureServices(builder.Configuration) // Infrastructure services (e.g., DbContext, repos)
    .AddWebServices(builder.Configuration); // Web UI services (e.g., controllers, Swagger)

// Configure CORS policy
const string myAllowSpecificOrigins = "_websiteOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:8100") // Allowed origin (e.g., frontend dev server)
              .AllowAnyMethod() // Allow all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader() // Allow all headers
              .AllowCredentials(); // Allow cookies/credentials (required for SignalR with auth)
        // Note: .WithMethods("OPTIONS") is redundant with AllowAnyMethod and removed
    });
});

// Add SignalR for real-time communication
builder.Services.AddSignalR();

// Build the app
var app = builder.Build();

// Middleware pipeline configuration
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Detailed error pages in dev
    app.UseMigrationsEndPoint(); // EF Core migrations UI
}
else
{
    app.UseHsts(); // Enforce HTTPS in production (30-day default)
}

// Core middleware pipeline
app.UseHealthChecks("/health"); // Health check endpoint
app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseRouting(); // Enable endpoint routing

// Authentication and authorization middleware
app.UseAuthentication(); // Enable auth (e.g., JWT, cookies)
app.UseAuthorization(); // Enable authorization checks

// Custom middleware
//app.UseMiddleware<PerformanceMiddleware>(); // Measure request performance
//app.UseMiddleware<RequestLoggingMiddleware>(); // Log request details (consolidated from duplicate)

// Swagger for API documentation
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./v1/swagger.json", "Share My Adventures API"); // Swagger JSON endpoint
});

// Enable CORS
app.UseCors(myAllowSpecificOrigins);

// Response caching for performance
app.UseResponseCaching();

// Serilog request logging with enriched context
app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true; // Include query string in logged path

    // Enrich logs with additional request data
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.Name);

        if (userId != null)
        {
            diagnosticContext.Set("UserId", userId); // Log user ID
        }

        var ip = httpContext.Connection.RemoteIpAddress?.ToString();

        if (ip != null)
        {
            diagnosticContext.Set("IP", ip); // Log client IP
        }
    };

    // Customize log level based on response status
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        return httpContext.Response.StatusCode >= 400
            ? LogEventLevel.Error // Log as error for 400+ status codes
            : LogEventLevel.Information; // Otherwise, log as info
    };
});

// Map routes and endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"); // Default MVC route

app.MapHub<NotificationHub>("/notificationHub"); // SignalR hub endpoint

try
{
    // Run the application
    await app.RunAsync();
}
catch (Exception ex)
{
    Serilog.Log.Error(ex, "An error occurred.", ex.Message);
    throw ex;
}