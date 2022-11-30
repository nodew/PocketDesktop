using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Pocket.Client.Core.Data;

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