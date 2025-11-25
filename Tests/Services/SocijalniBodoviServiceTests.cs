
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

        var audit = Mock.Of<IAuditService>();
        var logger = Mock.Of<ILogger<SocijalniBodoviService>>();

        var service = new SocijalniBodoviService(factory.Object, repo.Object, audit, logger);
        return (service, () => spremljeni);
    }

    private static SocijalniNatjecajZahtjev BuildZahtjev(Action<SocijalniNatjecajZahtjev> configure)
    {
        var zahtjev = new SocijalniNatjecajZahtjev
        {
            Id = 1,
            DatumPodnosenjaZahtjeva = new DateTime(2024, 6, 1),
            Natjecaj = new Natjecaj { ProsjekPlace = 1000 }
        };
        configure(zahtjev);
        return zahtjev;
    }

    [Fact]
    public async Task Test_Beskucnik_Samacko_55Plus_Zajamcena()
    {
        var zahtjev = BuildZahtjev(z =>
        {
            z.Clanovi = [new()
                {
                    DatumRodjenja = new DateOnly(1960,
                        1,
                        1),
                    Srodstvo = Srodstvo.PodnositeljZahtjeva,
                    Zahtjev = null
                }
            ];
            z.KucanstvoPodaci = new()
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.Beskucnik,
                SastavKucanstva = SastavKucanstva.SamackoKucanstvo,
                Prihod = new() { UkupniPrihodKucanstva = 0 }
            };
            z.BodovniPodaci = new() { PrimateljZajamceneMinimalneNaknade = true };
        });

        var (service, getSpremljeni) = CreateService(zahtjev);
        await service.IzracunajIBodujAsync(1);
        Assert.Equal(78, getSpremljeni()!.UkupnoBodova);
    }

    [Fact]
    public async Task Test_Slobodni_Jednoroditeljska_Maloljetni()
    {
        var zahtjev = BuildZahtjev(z =>
        {
            z.Clanovi = new List<SocijalniNatjecajClan>
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
            z.KucanstvoPodaci = new()
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.SlobodniNajam,
                SastavKucanstva = SastavKucanstva.JednoroditeljskaObitelj,
                Prihod = new() { UkupniPrihodKucanstva = 18000 }
            };
            z.BodovniPodaci = new();
        });

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

        var zahtjev = BuildZahtjev(z =>
        {
            z.Clanovi = [clan, clan];
            z.KucanstvoPodaci = new()
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.Beskucnik,
                SastavKucanstva = SastavKucanstva.SamackoKucanstvo,
                Prihod = new() { UkupniPrihodKucanstva = 12000 }
            };
            z.BodovniPodaci = new()
            {
                BrojMjeseciObranaSuvereniteta = 60,
                BrojClanovaZrtavaSeksualnogNasilja = 1,
                BrojCivilnihStradalnika = 2
            };
        });

        var (service, getSpremljeni) = CreateService(zahtjev);
        await service.IzracunajIBodujAsync(1);
        Assert.Equal(97, getSpremljeni()!.UkupnoBodova);
    }

    [Fact]
    public async Task Test_Invalidi_Doplatak_Njegovatelj()
    {
        var zahtjev = BuildZahtjev(z =>
        {
            z.Clanovi = new List<SocijalniNatjecajClan>
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
            };
            z.KucanstvoPodaci = new()
            {
                StambeniStatusKucanstva = StambeniStatusKucanstva.KodRoditelja,
                SastavKucanstva = SastavKucanstva.JednoroditeljskaObitelj,
                Prihod = new() { UkupniPrihodKucanstva = 12000 }
            };
            z.BodovniPodaci = new()
            {
                BrojOdraslihKorisnikaInvalidnine = 2,
                BrojMaloljetnihKorisnikaInvalidnine = 1,
                KorisnikDoplatkaZaPomoc = true,
                StatusRoditeljaNjegovatelja = true,
                BrojUzdrzavanePunoljetneDjece = 1,
                PrimateljZajamceneMinimalneNaknade = true
            };
        });

        var (service, getSpremljeni) = CreateService(zahtjev);
        await service.IzracunajIBodujAsync(1);
        Assert.Equal(135, getSpremljeni()!.UkupnoBodova);
    }
}