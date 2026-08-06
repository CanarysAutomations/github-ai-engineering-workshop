using MyEcomm.Identity.Api.Endpoints;
using MyEcomm.Identity.Api.Repositories;
using MyEcomm.Identity.Api.Seed;
using MyEcomm.Identity.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy => policy
        .WithOrigins("http://localhost:5173", "http://localhost:5100")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFrontend");

var userRepository = app.Services.GetRequiredService<IUserRepository>();
UserSeeder.SeedUsers(userRepository);

app.MapAuthEndpoints();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
