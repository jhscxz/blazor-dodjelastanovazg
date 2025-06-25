using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class SocijalniBodovnaGreskaServiceTests
{
    [Fact]
    public async Task PronadiGreskeAsync_DateOutsideRange_ReturnsError()
    {
        var service = new SocijalniBodovnaGreskaService(Mock.Of<IDbContextFactory<ApplicationDbContext>>());

        var zahtjev = new SocijalniNatjecajZahtjev
        {
            DatumPodnosenjaZahtjeva = new DateTime(2023, 12, 31),
            Natjecaj = new Natjecaj
            {
                DatumObjave = new DateOnly(2024, 1, 1),
                RokZaPrijavu = new DateOnly(2024, 1, 31)
            }
        };

        var greske = await service.PronadiGreskeAsync(zahtjev);

        Assert.Contains(greske, g => g.Kod == "DAT-001");
    }
    
    [Fact]
    public async Task PronadiGreskeAsync_PosjedujeNekretninu_ReturnsError()
    {
        var service = new SocijalniBodovnaGreskaService(Mock.Of<IDbContextFactory<ApplicationDbContext>>());

        var zahtjev = new SocijalniNatjecajZahtjev { PosjedujeNekretninuZg = true };

        var greske = await service.PronadiGreskeAsync(zahtjev);

        Assert.Contains(greske, g => g.Kod == "NEK-001");
    }

}