using MyEcomm.Catalog.Api.Endpoints;
using MyEcomm.Catalog.Api.Repositories;
using MyEcomm.Catalog.Api.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
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

var productRepository = app.Services.GetRequiredService<IProductRepository>();
ProductSeeder.SeedProducts(productRepository);

app.MapProductEndpoints();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
