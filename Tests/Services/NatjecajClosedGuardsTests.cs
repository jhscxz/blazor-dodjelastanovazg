using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class NatjecajClosedGuardsTests
{
    private static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    private static ApplicationDbContext CreateContext() =>
        CreateContext(Guid.NewGuid().ToString());

    private static Mock<IDbContextFactory<ApplicationDbContext>> CreateFactory(string dbName)
    {
        var factoryMock = new Mock<IDbContextFactory<ApplicationDbContext>>();
        factoryMock.Setup(f => f.CreateDbContext()).Returns(() => CreateContext(dbName));
        return factoryMock;
    }

    [Fact]
    public async Task CreateAsync_ClosedNatjecaj_Throws()
    {
        var dbName = Guid.NewGuid().ToString();
        await using (var context = CreateContext(dbName))
        {
            context.Natjecaji.Add(new Natjecaj { Id = 1, Zakljucen = 2 });
            await context.SaveChangesAsync();
        }

        var factoryMock = CreateFactory(dbName);
        var audit = new Mock<IAuditService>();
        var logger = new Mock<ILogger<SocijalniZahtjevWriteService>>();
        var service = new SocijalniZahtjevWriteService(factoryMock.Object, audit.Object, logger.Object);
        var zahtjev = new SocijalniNatjecajZahtjev { NatjecajId = 1 };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(zahtjev));
        await using var assertContext = CreateContext(dbName);
        Assert.Empty(assertContext.SocijalniNatjecajZahtjevi);
        logger.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("zaključen")),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateKucanstvoPodaciAsync_ClosedNatjecaj_Throws()
    {
        var dbName = Guid.NewGuid().ToString();
        await using (var context = CreateContext(dbName))
        {
            var natjecaj = new Natjecaj { Id = 1, Zakljucen = 2 };
            var zahtjev = new SocijalniNatjecajZahtjev { Id = 1, Natjecaj = natjecaj, NatjecajId = natjecaj.Id };
            context.Natjecaji.Add(natjecaj);
            context.SocijalniNatjecajZahtjevi.Add(zahtjev);
            await context.SaveChangesAsync();
        }

        var factoryMock = CreateFactory(dbName);
        var logger = new Mock<ILogger<SocijalniKucanstvoService>>();
        var service = new SocijalniKucanstvoService(factoryMock.Object, logger.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateKucanstvoPodaciAsync(1, new SocijalniKucanstvoPodaciDto()));
        logger.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("zaključen")),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateBodovniPodaci_ClosedNatjecaj_Throws()
    {
        var dbName = Guid.NewGuid().ToString();
        await using (var context = CreateContext(dbName))
        {
            var natjecaj = new Natjecaj { Id = 1, Zakljucen = 2 };
            var zahtjev = new SocijalniNatjecajZahtjev
            {
                Id = 1,
                Natjecaj = natjecaj,
                NatjecajId = natjecaj.Id,
                BodovniPodaci = new SocijalniNatjecajBodovniPodaci()
            };

            context.Natjecaji.Add(natjecaj);
            context.SocijalniNatjecajZahtjevi.Add(zahtjev);
            await context.SaveChangesAsync();
        }

        var factoryMock = CreateFactory(dbName);
        var logger = new Mock<ILogger<SocijalniBodovniPodaciService>>();
        var service = new SocijalniBodovniPodaciService(factoryMock.Object, logger.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(1, new SocijalniNatjecajBodovniPodaciDto()));
        logger.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("zaključen")),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task AddClanAsync_ClosedNatjecaj_Throws()
    {
        var dbName = Guid.NewGuid().ToString();
        await using (var context = CreateContext(dbName))
        {
            var natjecaj = new Natjecaj { Id = 1, Zakljucen = 2 };
            var zahtjev = new SocijalniNatjecajZahtjev { Id = 1, Natjecaj = natjecaj, NatjecajId = natjecaj.Id };
            context.Natjecaji.Add(natjecaj);
            context.SocijalniNatjecajZahtjevi.Add(zahtjev);
            await context.SaveChangesAsync();
        }

        var factoryMock = CreateFactory(dbName);
        var logger = new Mock<ILogger<SocijalniClanService>>();
        var service = new SocijalniClanService(factoryMock.Object, logger.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddClanAsync(new SocijalniNatjecajClan
        {
            ZahtjevId = 1,
            Zahtjev = null
        }));
        await using var assertContext = CreateContext(dbName);
        Assert.Empty(assertContext.SocijalniNatjecajClanovi);
        logger.Verify(l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("zaključen")),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }
}
