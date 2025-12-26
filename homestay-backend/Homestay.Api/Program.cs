using Homestay.Api.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorizationPolicies();
builder.Services.AddControllers();
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseCustomMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

