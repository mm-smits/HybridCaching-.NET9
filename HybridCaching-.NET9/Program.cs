using HybridCaching.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add MemoryCache and Redis Distributed Cache
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
// Add SQL Server Distributed Cache
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("CacheConnection");
    options.SchemaName = "dbo";
    options.TableName = "CacheItem";
    options.DefaultSlidingExpiration = TimeSpan.FromMinutes(10); // Cache expiration time
});
builder.Services.AddSingleton<HybridCacheService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hybrid Cache Demo is running");
app.Run();
