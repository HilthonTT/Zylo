using Zylo.Api.Extensions;
using Zylo.Infrastructure;
using Zylo.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace Zylo.Api
{
    public partial class Program;
}
