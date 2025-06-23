
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        var service = new SocijalniZahtjevWriteService(factoryMock.Object, Mock.Of<IAuditService>(), Mock.Of<ILogger<SocijalniZahtjevWriteService>>());
        var zahtjev = new SocijalniNatjecajZahtjev { NatjecajId = 1 };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(zahtjev));
        await using var assertContext = CreateContext(dbName);
        Assert.Empty(assertContext.SocijalniNatjecajZahtjevi);
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
        var service = new SocijalniKucanstvoService(factoryMock.Object, Mock.Of<ILogger<SocijalniKucanstvoService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateKucanstvoPodaciAsync(1, new SocijalniKucanstvoPodaciDto()));
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
        var service = new SocijalniBodovniPodaciService(factoryMock.Object, Mock.Of<ILogger<SocijalniBodovniPodaciService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(1, new SocijalniNatjecajBodovniPodaciDto()));
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
        var service = new SocijalniClanService(factoryMock.Object, Mock.Of<ILogger<SocijalniClanService>>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddClanAsync(new SocijalniNatjecajClan
        {
            ZahtjevId = 1,
            Zahtjev = null
        }));

        await using var assertContext = CreateContext(dbName);
        Assert.Empty(assertContext.SocijalniNatjecajClanovi);
    }
}