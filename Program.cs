using test_dotnet.Services;
using test_dotnet.Auth;
using test_dotnet.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// SERVICES REGISTRATION
// ========================================

// OpenAPI for documentation
builder.Services.AddOpenApi();

// Controllers with global exception handling
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// Custom services
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<JwtHelper>();

// JWT Authentication (config is in Auth/JwtConfiguration.cs)
builder.Services.AddJwtAuthentication(builder.Configuration);

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// ========================================
// MIDDLEWARE PIPELINE
// ========================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
