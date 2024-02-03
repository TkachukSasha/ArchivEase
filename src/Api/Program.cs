using Api.Common.Cors;
using Api.Middlewares;
using Core;
using Serilog;
using SharedKernel;
using SharedKernel.Files;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

var options = builder.Services.ConfigureFileOptions(builder.Configuration);

builder.Services.AddCore(builder.Configuration);

builder.Services.AddCorsPolicy(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = Extensions
                    .SafeCreateStaticFileProvider(Path.Combine(Directory.GetCurrentDirectory(), options.Path)),

    RequestPath = new PathString(options.RequestPath)
});

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app
    .UseCorsPolicy()
    .UseAuthentication()
    .UseRouting()
    .UseAuthorization();

app.MapControllers();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSerilogRequestLogging();

app.Run();

public partial class Program
{
}