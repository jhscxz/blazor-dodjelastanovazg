using DodjelaStanovaZG.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DodjelaStanovaZG.Tests.Services;

public static class TestDb
{
    public static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    public static Mock<IDbContextFactory<ApplicationDbContext>> CreateFactory(string dbName)
    {
        var factory = new Mock<IDbContextFactory<ApplicationDbContext>>();
        factory.Setup(f => f.CreateDbContext()).Returns(() => CreateContext(dbName));
        factory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => CreateContext(dbName));
        return factory;
    }
}