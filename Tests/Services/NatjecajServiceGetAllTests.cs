using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class NatjecajServiceGetAllTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllDtos()
    {
        var dbName = Guid.NewGuid().ToString();
        await using (var context = TestDb.CreateContext(dbName))
        {
            context.Natjecaji.AddRange([
                new Natjecaj { Id = 1, Klasa = 1, PriustiviIliSocijalni = 1, Zakljucen = 1 },
                new Natjecaj { Id = 2, Klasa = 2, PriustiviIliSocijalni = 2, Zakljucen = 1 }
            ]);
            await context.SaveChangesAsync();
        }

        await using var read = TestDb.CreateContext(dbName);
        var data = await read.Natjecaji.ToListAsync();

        var repo = new Mock<INatjecajRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

        var logger = new Mock<ILogger<NatjecajService>>();
        var service = new NatjecajService(repo.Object, logger.Object);

        var result = (await service.GetAllAsync()).ToList();

        Assert.Equal(data.Count, result.Count);
        repo.Verify(r => r.GetAllAsync(), Times.Once);
        logger.Verify(l => l.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Dohvaćanje svih natječaja")),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }
}