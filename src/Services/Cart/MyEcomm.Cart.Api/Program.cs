using MyEcomm.Cart.Api.Clients;
using MyEcomm.Cart.Api.Endpoints;
using MyEcomm.Cart.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();
builder.Services.AddHttpClient<CatalogServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:CatalogBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(10);
});

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

app.MapCartEndpoints();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
