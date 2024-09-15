using System.Reflection;
using Zylo.Api.Extensions;
using Zylo.Application;
using Zylo.Infrastructure;
using Zylo.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddApplication();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.MapEndpoints();

app.UseBackgroundJobs();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace Zylo.Api
{
    public partial class Program;
}
