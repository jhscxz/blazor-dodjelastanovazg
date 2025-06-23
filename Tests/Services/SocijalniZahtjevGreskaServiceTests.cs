using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
    public async Task ObradiGreskeAsync_SetsRezultatObradeToGreska_WhenErrorsExist()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Seed
        await using (var seed = new ApplicationDbContext(options))
        {
            seed.SocijalniNatjecajZahtjevi.Add(new SocijalniNatjecajZahtjev
            {
                Id = 1,
                ManualniRezultatObrade = RezultatObrade.Osnovan,
                RezultatObrade = RezultatObrade.Osnovan
            });
            await seed.SaveChangesAsync();
        }

        var greskaService = new Mock<ISocijalniBodovnaGreskaService>();
        greskaService.Setup(g => g.PronadiGreskeAsync(It.IsAny<SocijalniNatjecajZahtjev>()))
            .ReturnsAsync([new SocijalniNatjecajBodovnaGreska { ZahtjevId = 1, Kod = "E1", Poruka = "p" }]);

        var factory = Mock.Of<IDbContextFactory<ApplicationDbContext>>(f => f.CreateDbContext() == new ApplicationDbContext(options));
        var audit = Mock.Of<IAuditService>();
        var logger = Mock.Of<ILogger<SocijalniZahtjevGreskaService>>();

        var service = new SocijalniZahtjevGreskaService(factory, greskaService.Object, audit, logger);

        await service.ObradiGreskeAsync(new SocijalniNatjecajZahtjev { Id = 1, ManualniRezultatObrade = RezultatObrade.Osnovan });

        await using var assertContext = new ApplicationDbContext(options);
        var saved = await assertContext.SocijalniNatjecajZahtjevi.FindAsync(1L);

        Assert.NotNull(saved);
        Assert.Equal(RezultatObrade.Greška, saved!.RezultatObrade);
    }
    
    [Theory]
    [InlineData(RezultatObrade.Nepotpun)]
    [InlineData(RezultatObrade.Neosnovan)]
    public async Task ObradiGreskeAsync_PreservesManualResult_WhenNotOsnovan(RezultatObrade manual)
    {
        // Arrange
        var options = CreateOptions();

        await using (var init = new ApplicationDbContext(options))
        {
            init.SocijalniNatjecajZahtjevi.Add(new SocijalniNatjecajZahtjev
            {
                Id = 1,
                ManualniRezultatObrade = manual,
                RezultatObrade = manual
            });
            await init.SaveChangesAsync();
        }

        var greske = new List<SocijalniNatjecajBodovnaGreska>
        {
            new() { ZahtjevId = 1, Kod = "E1", Poruka = "p" }
        };

        var greskaService = new Mock<ISocijalniBodovnaGreskaService>();
        greskaService.Setup(g => g.PronadiGreskeAsync(It.IsAny<SocijalniNatjecajZahtjev>()))
            .ReturnsAsync(greske);

        var factoryMock = CreateFactory(options);
        var audit = new Mock<IAuditService>();
        var logger = new Mock<ILogger<SocijalniZahtjevGreskaService>>();
        var service = new SocijalniZahtjevGreskaService(factoryMock.Object, greskaService.Object, audit.Object, logger.Object);

        var zahtjev = new SocijalniNatjecajZahtjev { Id = 1, ManualniRezultatObrade = manual };

        // Act
        await service.ObradiGreskeAsync(zahtjev);

        // Assert
        await using var assertContext = new ApplicationDbContext(options);
        var saved = await assertContext.SocijalniNatjecajZahtjevi.FindAsync(1L);
        Assert.NotNull(saved);
        Assert.Equal(manual, saved!.RezultatObrade);
    }
}