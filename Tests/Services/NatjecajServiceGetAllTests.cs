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
            context.Natjecaji.AddRange(
                new Natjecaj { Id = 1, Klasa = 1, PriustiviIliSocijalni = 1, Zakljucen = 1 },
                new Natjecaj { Id = 2, Klasa = 2, PriustiviIliSocijalni = 2, Zakljucen = 1 }
            );
            await context.SaveChangesAsync();
        }

        await using var read = TestDb.CreateContext(dbName);
        var data = await read.Natjecaji.ToListAsync();

        var repo = Mock.Of<INatjecajRepository>(r => r.GetAllAsync() == Task.FromResult(data));
        var logger = new Mock<ILogger<NatjecajService>>();

        var service = new NatjecajService(repo, logger.Object);
        var result = (await service.GetAllAsync()).ToList();

        Assert.Equal(data.Count, result.Count);
    }
}