using Database;
using Microsoft.EntityFrameworkCore;

namespace Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder application)
    {
        using IServiceScope scope = application.ApplicationServices.CreateScope();

        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();
    }
}
