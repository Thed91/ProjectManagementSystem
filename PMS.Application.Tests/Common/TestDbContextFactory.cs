using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Persistence.Data;

namespace PMS.Application.Tests.Common;

public static class TestDbContextFactory
{
    public static IApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }

    public static void Destroy(IApplicationDbContext context)
    {
        if (context is ApplicationDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
    }
}
