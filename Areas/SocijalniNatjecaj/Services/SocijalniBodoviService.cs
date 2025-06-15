using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniBodoviService(
    ApplicationDbContext context,
    IUserContextService userContext)
    : ISocijalniBodoviService
{
    public async Task IzracunajIBodujAsync(long zahtjevId)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi
            .Include(z => z.Natjecaj)
            .Include(z => z.Clanovi)
            .Include(z => z.KucanstvoPodaci)
                .ThenInclude(k => k.Prihod)
            .Include(z => z.BodovniPodaci)
            .Include(z => z.Bodovi)
            .FirstOrDefaultAsync(z => z.Id == zahtjevId);

        if (zahtjev == null || zahtjev.KucanstvoPodaci == null || zahtjev.BodovniPodaci == null)
            throw new InvalidOperationException("Podaci nisu potpuni za bodovanje.");

        var kucanstvo = zahtjev.KucanstvoPodaci;
        var bodovni    = zahtjev.BodovniPodaci;
        var clanovi    = zahtjev.Clanovi;
        // broj članova kućanstva
        int brojClanova = clanovi.Count;
        int brojMaloljetnih = clanovi.Count(c => c.DatumRodjenja.AddYears(18) > DateOnly.FromDateTime(zahtjev.DatumPodnosenjaZahtjeva));
        int brojPunoljetnihUzdrzavanih = bodovni.BrojUzdrzavanePunoljetneDjece;

        var bodovi = zahtjev.Bodovi ?? new SocijalniNatjecajBodovi { Id = zahtjevId };

        // Stambeni status
        bodovi.BodoviStambeniStatus = kucanstvo.StambeniStatusKucanstva switch
        {
            StambeniStatusKucanstva.SlobodniNajam         => 12,
            StambeniStatusKucanstva.KodRoditelja          => 9,
            StambeniStatusKucanstva.OrganiziranoStanovanje=> 15,
            StambeniStatusKucanstva.Beskucnik             => 30,
            _                                             => 0
        };

        // Sastav kućanstva
        bodovi.BodoviSastavKucanstva = kucanstvo.SastavKucanstva switch
        {
            SastavKucanstva.SamohraniRoditelj        => 20,
            SastavKucanstva.JednoroditeljskaObitelj => 15,
            SastavKucanstva.SamackoKucanstvo         => 15,
            _                                        => 0
        };

        // Ostali bodovi
        bodovi.BodoviPoClanu            = (byte)(brojClanova * 3);
        bodovi.BodoviMaloljetni        = (byte)(brojMaloljetnih * 5);
        bodovi.BodoviPunoljetniUzdrzavani = (byte)(brojPunoljetnihUzdrzavanih * 3);
        bodovi.BodoviZajamcenaNaknada   = bodovni.PrimateljZajamceneMinimalneNaknade ? (byte)15 : (byte)0;
        bodovi.BodoviNjegovatelj       = bodovni.StatusRoditeljaNjegovatelja           ? (byte)12 : (byte)0;
        bodovi.BodoviDoplatakZaNjegu    = bodovni.KorisnikDoplatkaZaPomoc              ? (byte)9  : (byte)0;
        bodovi.BodoviOdraslihInvalidnina   = (byte)(bodovni.BrojOdraslihKorisnikaInvalidnine * 15);
        bodovi.BodoviMaloljetnihInvalidnina = (byte)(bodovni.BrojMaloljetnihKorisnikaInvalidnine * 20);
        bodovi.BodoviZrtvaNasilja       = bodovni.ZrtvaObiteljskogNasilja               ? (byte)10 : (byte)0;
        bodovi.BodoviAlternativnaSkrb   = (byte)(bodovni.BrojOsobaUAlternativnojSkrbi * 10);

        // Iznad 55
        var podnositelj = clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
        bool hasValidDatum = podnositelj != null && podnositelj.DatumRodjenja.Year > 1900;
        bool isIznad55     = hasValidDatum && podnositelj!.DatumRodjenja.AddYears(55) <= DateOnly.FromDateTime(zahtjev.DatumPodnosenjaZahtjeva);
        bodovi.BodoviIznad55 = hasValidDatum
            ? (isIznad55 ? (byte)15 : (byte)0)
            : (byte)0;

        bodovi.BodoviObrana             = Math.Min(bodovni.BrojMjeseciObranaSuvereniteta * 0.5f, 20f);
        bodovi.BodoviSeksualnoNasilje   = (byte)(bodovni.BrojClanovaZrtavaSeksualnogNasilja * 10);
        bodovi.BodoviCivilniStradalnici = (byte)(bodovni.BrojCivilnihStradalnika * 8);

        // Ukupno bodova
        bodovi.UkupnoBodova = (ushort)(
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
            + bodovi.BodoviCivilniStradalnici
        );

        // ➕ Izračun prihoda u odnosu na prosjek (mjesečno)
        var prihod = zahtjev.KucanstvoPodaci.Prihod
                     ?? throw new InvalidOperationException("Prihod nije inicijaliziran.");
        var prosjek = zahtjev.Natjecaj?.ProsjekPlace ?? 0m;

        if (prosjek > 0m && brojClanova > 0)
        {
            // 1) Mjesečni prihod kućanstva
            var mjesecniPrihodKucanstva = prihod.UkupniPrihodKucanstva / 12m;

            // 2) Mjesečni prihod po članu kućanstva
            prihod.PrihodPoClanu = mjesecniPrihodKucanstva / brojClanova;

            // 3) Postotak = (mjesečni prihod **po članu** / mjesečni prosjek) * 100
            prihod.PostotakProsjeka = prihod.PrihodPoClanu / prosjek * 100m;

            // 4) Prag: 50% za jednoročlano, 30% inače
            var prag = brojClanova == 1 ? 50m : 30m;
            prihod.IspunjavaUvjetPrihoda = prihod.PostotakProsjeka <= prag;

            AuditHelper.ApplyAudit(prihod, userContext.GetCurrentUserId(), isCreate: false);
        }

        // Spremi ili update bodova
        if (zahtjev.Bodovi == null)
        {
            AuditHelper.ApplyAudit(bodovi, userContext.GetCurrentUserId(), isCreate: true);
            context.SocijalniNatjecajBodovi.Add(bodovi);
        }
        else
        {
            AuditHelper.ApplyAudit(bodovi, userContext.GetCurrentUserId(), isCreate: false);
            context.Entry(zahtjev.Bodovi).CurrentValues.SetValues(bodovi);
        }

        await context.SaveChangesAsync();
    }

    public async Task<SocijalniNatjecajBodovi?> GetByIdAsync(long zahtjevId)
    {
        return await context.SocijalniNatjecajBodovi
            .AsNoTracking()
            .Include(b => b.Zahtjev)
                .ThenInclude(z => z.Clanovi)
            .Include(b => b.Zahtjev)
                .ThenInclude(z => z.KucanstvoPodaci)
                    .ThenInclude(k => k!.Prihod)
            .FirstOrDefaultAsync(b => b.ZahtjevId == zahtjevId);
    }
}
