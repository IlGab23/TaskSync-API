using TaskSync.API.Endpoints;
using TaskSync.API.Extensions;
using TaskSync.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

await app.Services.ApplyMigrationsAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapSystemEndpoint();

app.Run();
