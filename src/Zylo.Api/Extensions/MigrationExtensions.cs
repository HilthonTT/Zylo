using Microsoft.EntityFrameworkCore;
using Zylo.Persistence;

namespace Zylo.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ZyloDbContext dbContext = scope.ServiceProvider.GetRequiredService<ZyloDbContext>();

        dbContext.Database.Migrate();
    }
}
