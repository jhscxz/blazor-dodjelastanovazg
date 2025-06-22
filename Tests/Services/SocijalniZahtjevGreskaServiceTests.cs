using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class SocijalniZahtjevGreskaServiceTests
{
    private static DbContextOptions<ApplicationDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private static Mock<IDbContextFactory<ApplicationDbContext>> CreateFactory(DbContextOptions<ApplicationDbContext> options)
    {
        var factory = new Mock<IDbContextFactory<ApplicationDbContext>>();
        factory.Setup(f => f.CreateDbContext()).Returns(() => new ApplicationDbContext(options));
        return factory;
    }

    [Fact]
    public async Task ObradiGreskeAsync_SavesFoundErrors()
    {
        // Arrange
        var options = CreateOptions();

        await using (var initContext = new ApplicationDbContext(options))
        {
            initContext.SocijalniNatjecajBodovnaGreske.Add(new SocijalniNatjecajBodovnaGreska
            {
                ZahtjevId = 1,
                Kod = "OLD",
                Poruka = "old"
            });
            await initContext.SaveChangesAsync();
        }

        var greske = new List<SocijalniNatjecajBodovnaGreska>
        {
            new() { ZahtjevId = 1, Kod = "NEW1", Poruka = "p1" },
            new() { ZahtjevId = 1, Kod = "NEW2", Poruka = "p2" }
        };

        var greskaService = new Mock<ISocijalniBodovnaGreskaService>();
        greskaService.Setup(g => g.PronadiGreskeAsync(It.IsAny<SocijalniNatjecajZahtjev>()))
            .ReturnsAsync(greske);

        var factoryMock = CreateFactory(options);
        var audit = new Mock<IAuditService>();
        var logger = new Mock<ILogger<SocijalniZahtjevGreskaService>>();
        var service = new SocijalniZahtjevGreskaService(factoryMock.Object, greskaService.Object, audit.Object, logger.Object);

        var zahtjev = new SocijalniNatjecajZahtjev { Id = 1, ManualniRezultatObrade = RezultatObrade.Osnovan };

        // Act
        await service.ObradiGreskeAsync(zahtjev);

        // Assert
        await using var assertContext = new ApplicationDbContext(options);
        var saved = await assertContext.SocijalniNatjecajBodovnaGreske
            .Where(g => g.ZahtjevId == 1)
            .ToListAsync();

        Assert.Equal(2, saved.Count);
        Assert.All(saved, g => Assert.NotEqual("OLD", g.Kod));
    }
}