using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class SocijalniBodoviServiceTests
{
    private static (SocijalniBodoviService Service, Func<SocijalniNatjecajBodovi?> GetSpremljeni)
        CreateService(SocijalniNatjecajZahtjev zahtjev)
    {
        SocijalniNatjecajBodovi? spremljeni = null;

        var repo = new Mock<ISocijalniBodoviRepository>();
        repo.Setup(r => r.GetZahtjevWithDetailsAsync(It.IsAny<ApplicationDbContext>(), 1))
            .ReturnsAsync(zahtjev);

        repo.Setup(r => r.AddBodoviAsync(It.IsAny<ApplicationDbContext>(), It.IsAny<SocijalniNatjecajBodovi>()))
            .Callback<ApplicationDbContext, SocijalniNatjecajBodovi>((_, b) => spremljeni = b)
            .Returns(Task.CompletedTask);

        repo.Setup(r => r.SaveChangesAsync(It.IsAny<ApplicationDbContext>()))
            .Returns(Task.CompletedTask);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        var factory = new Mock<IDbContextFactory<ApplicationDbContext>>();
        factory.Setup(f => f.CreateDbContext()).Returns(context);

        var audit = new Mock<IAuditService>();
        var logger = new Mock<ILogger<SocijalniBodoviService>>();

        var service = new SocijalniBodoviService(factory.Object, repo.Object, audit.Object, logger.Object);
        return (service, () => spremljeni);
    }

    [Fact]
    public async Task Test_Beskucnik_Samacko_55Plus_Zajamcena()
    {
        var clan = new SocijalniNatjecajClan
        {
            DatumRodjenja = new DateOnly(1960,
                1,
                1),
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            Zahtjev = null,
        };

        var zahtjev = new SocijalniNatjecajZahtjev
        {
            Id = 1,
            DatumPodnosenjaZahtjeva = new DateTime(2024, 6, 1),
            Natjecaj = new Natjecaj { ProsjekPlace = 1000 },
            Clanovi = [clan],
            KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.Beskucnik,
                SastavKucanstva = SastavKucanstva.SamackoKucanstvo,
                Prihod = new SocijalniPrihodi { UkupniPrihodKucanstva = 0 }
            },
            BodovniPodaci = new SocijalniNatjecajBodovniPodaci
            {
                PrimateljZajamceneMinimalneNaknade = true
            }
        };

        var (service, getSpremljeni) = CreateService(zahtjev);
        await service.IzracunajIBodujAsync(1);
        Assert.Equal(78, getSpremljeni()!.UkupnoBodova);
    }

    [Fact]
    public async Task Test_Slobodni_Jednoroditeljska_Maloljetni()
    {
        var clanovi = new List<SocijalniNatjecajClan>
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
                Srodstvo = Srodstvo.UzdrzavaniClan,
                Zahtjev = null
            },
            new()
            {
                DatumRodjenja = new DateOnly(1990,
                    1,
                    1),
                Srodstvo = Srodstvo.BracniDrug,
                Zahtjev = null
            }
        };

        var zahtjev = new SocijalniNatjecajZahtjev
        {
            Id = 1,
            DatumPodnosenjaZahtjeva = new DateTime(2024, 6, 1),
            Natjecaj = new Natjecaj { ProsjekPlace = 1000 },
            Clanovi = clanovi,
            KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.SlobodniNajam,
                SastavKucanstva = SastavKucanstva.JednoroditeljskaObitelj,
                Prihod = new SocijalniPrihodi { UkupniPrihodKucanstva = 18000 }
            },
            BodovniPodaci = new SocijalniNatjecajBodovniPodaci()
        };

        var (service, getSpremljeni) = CreateService(zahtjev);
        await service.IzracunajIBodujAsync(1);
        Assert.Equal(41, getSpremljeni()!.UkupnoBodova);
    }

    [Fact]
    public async Task Test_Branitelj_Civilni_SeksualnoNasilje()
    {
        var clan = new SocijalniNatjecajClan
        {
            DatumRodjenja = new DateOnly(1990,
                1,
                1),
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            Zahtjev = null
        };

        var zahtjev = new SocijalniNatjecajZahtjev
        {
            Id = 1,
            DatumPodnosenjaZahtjeva = new DateTime(2024, 6, 1),
            Natjecaj = new Natjecaj { ProsjekPlace = 1000 },
            Clanovi = [clan, clan],
            KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.Beskucnik,
                SastavKucanstva = SastavKucanstva.SamackoKucanstvo,
                Prihod = new SocijalniPrihodi { UkupniPrihodKucanstva = 12000 }
            },
            BodovniPodaci = new SocijalniNatjecajBodovniPodaci
            {
                BrojMjeseciObranaSuvereniteta = 60,
                BrojClanovaZrtavaSeksualnogNasilja = 1,
                BrojCivilnihStradalnika = 2
            }
        };

        var (service, getSpremljeni) = CreateService(zahtjev);
        await service.IzracunajIBodujAsync(1);
        Assert.Equal(97, getSpremljeni()!.UkupnoBodova);
    }

    [Fact]
    public async Task Test_Invalidi_Doplatak_Njegovatelj()
    {
        var zahtjev = new SocijalniNatjecajZahtjev
        {
            Id = 1,
            DatumPodnosenjaZahtjeva = new DateTime(2024, 6, 1),
            Natjecaj = new Natjecaj { ProsjekPlace = 1000 },
            Clanovi = new List<SocijalniNatjecajClan>
            {
                new()
                {
                    DatumRodjenja = new DateOnly(2010,
                        1,
                        1),
                    Srodstvo = Srodstvo.PodnositeljZahtjeva,
                    Zahtjev = null
                },
                new()
                {
                    DatumRodjenja = new DateOnly(1995,
                        1,
                        1),
                    Srodstvo = Srodstvo.BracniDrug,
                    Zahtjev = null
                },
                new()
                {
                    DatumRodjenja = new DateOnly(2010,
                        1,
                        1),
                    Srodstvo = Srodstvo.UzdrzavaniClan,
                    Zahtjev = null
                },
                new()
                {
                    DatumRodjenja = new DateOnly(1990,
                        1,
                        1),
                    Srodstvo = Srodstvo.UzdrzavaniClan,
                    Zahtjev = null
                }
            },
            KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.KodRoditelja,
                SastavKucanstva = SastavKucanstva.JednoroditeljskaObitelj,
                Prihod = new SocijalniPrihodi { UkupniPrihodKucanstva = 12000 }
            },
            BodovniPodaci = new SocijalniNatjecajBodovniPodaci
            {
                BrojOdraslihKorisnikaInvalidnine = 2,
                BrojMaloljetnihKorisnikaInvalidnine = 1,
                KorisnikDoplatkaZaPomoc = true,
                StatusRoditeljaNjegovatelja = true,
                BrojUzdrzavanePunoljetneDjece = 1,
                PrimateljZajamceneMinimalneNaknade = true
            }
        };

        var (service, getSpremljeni) = CreateService(zahtjev);
        await service.IzracunajIBodujAsync(1);
        Assert.Equal(135, getSpremljeni()!.UkupnoBodova);
    }
}
