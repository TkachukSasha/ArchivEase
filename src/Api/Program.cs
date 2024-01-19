using Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCore(builder.Configuration);

var app = builder.Build();

app
    .UseAuthentication()
    .UseRouting()
    .UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}