using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;

public class SocijalniBodoviService(
    IDbContextFactory<ApplicationDbContext> contextFactory,
    ISocijalniBodoviRepository repository,
    IAuditService auditService,
    ILogger<SocijalniBodoviService> logger)
    : ISocijalniBodoviService
{
    public async Task IzracunajIBodujAsync(long zahtjevId)
    {
        logger.LogInformation("Započinjem bodovanje zahtjeva {ZahtjevId}", zahtjevId);
        await using var context = await contextFactory.CreateDbContextAsync();
        var zahtjev = await repository.GetZahtjevWithDetailsAsync(context, zahtjevId);
        if (zahtjev == null || zahtjev.KucanstvoPodaci == null || zahtjev.BodovniPodaci == null)
        {
            logger.LogError("Podaci nisu potpuni za bodovanje zahtjeva {ZahtjevId}", zahtjevId);
            throw new InvalidOperationException("Podaci nisu potpuni za bodovanje.");
        }

        var kucanstvo = zahtjev.KucanstvoPodaci;
        var bodovni = zahtjev.BodovniPodaci;
        var clanovi = zahtjev.Clanovi;

        int brojClanova = clanovi.Count;
        int brojMaloljetnih = clanovi.Count(c => c.DatumRodjenja.AddYears(18) > DateOnly.FromDateTime(zahtjev.DatumPodnosenjaZahtjeva));
        int brojPunoljetnihUzdrzavanih = bodovni.BrojUzdrzavanePunoljetneDjece;

        var bodovi = zahtjev.Bodovi ?? new SocijalniNatjecajBodovi { Id = zahtjevId };

        bodovi.BodoviStambeniStatus = kucanstvo.StambeniStatusKucanstva switch
        {
            StambeniStatusKucanstva.SlobodniNajam => 12,
            StambeniStatusKucanstva.KodRoditelja => 9,
            StambeniStatusKucanstva.OrganiziranoStanovanje => 15,
            StambeniStatusKucanstva.Beskucnik => 30,
            _ => 0
        };

        bodovi.BodoviSastavKucanstva = kucanstvo.SastavKucanstva switch
        {
            SastavKucanstva.SamohraniRoditelj => 20,
            SastavKucanstva.JednoroditeljskaObitelj => 15,
            SastavKucanstva.SamackoKucanstvo => 15,
            _ => 0
        };

        bodovi.BodoviPoClanu = (byte)(brojClanova * 3);
        bodovi.BodoviMaloljetni = (byte)(brojMaloljetnih * 5);
        bodovi.BodoviPunoljetniUzdrzavani = (byte)(brojPunoljetnihUzdrzavanih * 3);
        bodovi.BodoviZajamcenaNaknada = bodovni.PrimateljZajamceneMinimalneNaknade ? (byte)15 : (byte)0;
        bodovi.BodoviNjegovatelj = bodovni.StatusRoditeljaNjegovatelja ? (byte)12 : (byte)0;
        bodovi.BodoviDoplatakZaNjegu = bodovni.KorisnikDoplatkaZaPomoc ? (byte)9 : (byte)0;
        bodovi.BodoviOdraslihInvalidnina = (byte)(bodovni.BrojOdraslihKorisnikaInvalidnine * 15);
        bodovi.BodoviMaloljetnihInvalidnina = (byte)(bodovni.BrojMaloljetnihKorisnikaInvalidnine * 20);
        bodovi.BodoviZrtvaNasilja = bodovni.ZrtvaObiteljskogNasilja ? (byte)10 : (byte)0;
        bodovi.BodoviAlternativnaSkrb = (byte)(bodovni.BrojOsobaUAlternativnojSkrbi * 10);

        var podnositelj = clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
        bool hasValidDatum = podnositelj is { DatumRodjenja.Year: > 1900 };
        bool isIznad55 = hasValidDatum && podnositelj!.DatumRodjenja.AddYears(55) <= DateOnly.FromDateTime(zahtjev.DatumPodnosenjaZahtjeva);
        bodovi.BodoviIznad55 = hasValidDatum ? (isIznad55 ? (byte)15 : (byte)0) : (byte)0;

        bodovi.BodoviObrana = Math.Min(bodovni.BrojMjeseciObranaSuvereniteta * 0.5f, 20f);
        bodovi.BodoviSeksualnoNasilje = (byte)(bodovni.BrojClanovaZrtavaSeksualnogNasilja * 10);
        bodovi.BodoviCivilniStradalnici = (byte)(bodovni.BrojCivilnihStradalnika * 8);

        bodovi.UkupnoBodova =
            bodovi.BodoviStambeniStatus
            + bodovi.BodoviSastavKucanstva
            + bodovi.BodoviPoClanu
            + bodovi.BodoviMaloljetni
            + bodovi.BodoviPunoljetniUzdrzavani
            + bodovi.BodoviZajamcenaNaknada
            + bodovi.BodoviNjegovatelj
            + bodovi.BodoviDoplatakZaNjegu
            + bodovi.BodoviOdraslihInvalidnina
            + bodovi.BodoviMaloljetnihInvalidnina
            + bodovi.BodoviZrtvaNasilja
            + bodovi.BodoviAlternativnaSkrb
            + bodovi.BodoviIznad55
            + bodovi.BodoviObrana
            + bodovi.BodoviSeksualnoNasilje
            + bodovi.BodoviCivilniStradalnici;

        var prihod = kucanstvo.Prihod;
        if (prihod == null)
        {
            logger.LogError("Prihod nije inicijaliziran za zahtjev {ZahtjevId}", zahtjevId);
            throw new InvalidOperationException("Prihod nije inicijaliziran.");
        }

        var prosjek = zahtjev.Natjecaj?.ProsjekPlace ?? 0m;

        if (prosjek > 0m && brojClanova > 0)
        {
            var mjesecniPrihodKucanstva = prihod.UkupniPrihodKucanstva / 12m;
            prihod.PrihodPoClanu = mjesecniPrihodKucanstva / brojClanova;
            prihod.PostotakProsjeka = prihod.PrihodPoClanu / prosjek * 100m;

            var prag = brojClanova == 1 ? 50m : 30m;
            prihod.IspunjavaUvjetPrihoda = prihod.PostotakProsjeka <= prag;

            auditService.ApplyAudit(prihod, false);
        }

        if (zahtjev.Bodovi == null)
        {
            auditService.ApplyAudit(bodovi, true);
            await repository.AddBodoviAsync(context, bodovi);
        }
        else
        {
            auditService.ApplyAudit(bodovi, false);
            
            zahtjev.Bodovi.BodoviStambeniStatus = bodovi.BodoviStambeniStatus;
            zahtjev.Bodovi.BodoviSastavKucanstva = bodovi.BodoviSastavKucanstva;
            zahtjev.Bodovi.BodoviPoClanu = bodovi.BodoviPoClanu;
            zahtjev.Bodovi.BodoviMaloljetni = bodovi.BodoviMaloljetni;
            zahtjev.Bodovi.BodoviPunoljetniUzdrzavani = bodovi.BodoviPunoljetniUzdrzavani;
            zahtjev.Bodovi.BodoviZajamcenaNaknada = bodovi.BodoviZajamcenaNaknada;
            zahtjev.Bodovi.BodoviNjegovatelj = bodovi.BodoviNjegovatelj;
            zahtjev.Bodovi.BodoviDoplatakZaNjegu = bodovi.BodoviDoplatakZaNjegu;
            zahtjev.Bodovi.BodoviOdraslihInvalidnina = bodovi.BodoviOdraslihInvalidnina;
            zahtjev.Bodovi.BodoviMaloljetnihInvalidnina = bodovi.BodoviMaloljetnihInvalidnina;
            zahtjev.Bodovi.BodoviZrtvaNasilja = bodovi.BodoviZrtvaNasilja;
            zahtjev.Bodovi.BodoviAlternativnaSkrb = bodovi.BodoviAlternativnaSkrb;
            zahtjev.Bodovi.BodoviIznad55 = bodovi.BodoviIznad55;
            zahtjev.Bodovi.BodoviObrana = bodovi.BodoviObrana;
            zahtjev.Bodovi.BodoviSeksualnoNasilje = bodovi.BodoviSeksualnoNasilje;
            zahtjev.Bodovi.BodoviCivilniStradalnici = bodovi.BodoviCivilniStradalnici;
            zahtjev.Bodovi.UkupnoBodova = bodovi.UkupnoBodova;
        }

        await context.SaveChangesAsync();
        logger.LogInformation("Završeno bodovanje zahtjeva {ZahtjevId} - ukupno {Bodovi} bodova", zahtjevId, bodovi.UkupnoBodova);
    }

    public async Task<SocijalniNatjecajBodovi?> GetByIdAsync(long zahtjevId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await repository.GetZahtjevWithDetailsAsync(context, zahtjevId)
            .ContinueWith(t => t.Result?.Bodovi);
    }

    public async Task<List<SocijalniNatjecajBodovi>> GetForZahtjeviAsync(List<long> zahtjevIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var list = new List<SocijalniNatjecajBodovi>();
        foreach (var id in zahtjevIds)
        {
            var zahtjev = await repository.GetZahtjevWithDetailsAsync(context, id);
            if (zahtjev?.Bodovi != null)
                list.Add(zahtjev.Bodovi);
        }

        return list;
    }
}