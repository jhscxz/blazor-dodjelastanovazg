using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class SocijalniBodoviServiceTests
{
    [Fact]
    public async Task IzracunajIBodujAsync_ComputesExpectedPoints()
    {
        // Arrange: testni zahtjev s bodovnim podacima
        var zahtjev = new SocijalniNatjecajZahtjev
        {
            Id = 1,
            DatumPodnosenjaZahtjeva = new DateTime(2024, 6, 1),
            Natjecaj = new Natjecaj { ProsjekPlace = 1000m },
            Clanovi = new List<SocijalniNatjecajClan>
            {
                new()
                {
                    DatumRodjenja = new DateOnly(1990,
                        1,
                        1),
                    Srodstvo = Srodstvo.PodnositeljZahtjeva,
                    Zahtjev = null
                },
                new()
                {
                    DatumRodjenja = new DateOnly(2015,
                        1,
                        1),
                    Zahtjev = null
                }
            },
            KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.Beskucnik,
                SastavKucanstva = SastavKucanstva.SamackoKucanstvo,
                Prihod = new SocijalniPrihodi
                {
                    UkupniPrihodKucanstva = 24000m
                }
            },
            BodovniPodaci = new SocijalniNatjecajBodovniPodaci
            {
                PrimateljZajamceneMinimalneNaknade = true
            }
        };

        // Mock repo
        var repo = new Mock<ISocijalniBodoviRepository>();
        repo.Setup(r => r.GetZahtjevWithDetailsAsync(1)).ReturnsAsync(zahtjev);

        SocijalniNatjecajBodovi? spremljeno = null;
        repo.Setup(r => r.AddBodoviAsync(It.IsAny<SocijalniNatjecajBodovi>()))
            .Callback<SocijalniNatjecajBodovi>(b => spremljeno = b)
            .Returns(Task.CompletedTask);

        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Mock audit
        var audit = new Mock<IAuditService>();

        // Service
        var service = new SocijalniBodoviService(repo.Object, audit.Object);

        // Act
        await service.IzracunajIBodujAsync(1);

        // Assert
        Assert.NotNull(spremljeno);

        // Ovisno o pravilima bodovanja – ovo ćeš možda trebati ažurirati:
        Assert.Equal((ushort)71, spremljeno!.UkupnoBodova); // Ako znaš pravila, ovo je ok

        Assert.Equal(1000m, zahtjev.KucanstvoPodaci!.Prihod!.PrihodPoClanu);
        Assert.Equal(100m, zahtjev.KucanstvoPodaci.Prihod.PostotakProsjeka);
        Assert.False(zahtjev.KucanstvoPodaci.Prihod.IspunjavaUvjetPrihoda);
    }
}
