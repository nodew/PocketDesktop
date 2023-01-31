using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace PocketClient.Core.Data;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PocketDbContext>
{
    public PocketDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<PocketDbContext>()
            .UseSqlite("Data Source=Pocket.db")
            .Options;

        return new PocketDbContext(options);
    }
}