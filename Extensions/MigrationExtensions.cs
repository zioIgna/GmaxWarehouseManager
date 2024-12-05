using Gmax.Data;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using GmaxDbContext dbContext = scope.ServiceProvider.GetRequiredService<GmaxDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
