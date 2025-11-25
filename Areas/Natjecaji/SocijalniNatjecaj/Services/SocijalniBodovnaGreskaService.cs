using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;

public sealed class SocijalniBodovnaGreskaService(IDbContextFactory<ApplicationDbContext> contextFactory) : ISocijalniBodovnaGreskaService
{
    public async Task<List<SocijalniNatjecajBodovnaGreska>> GetByZahtjevIdAsync(long zahtjevId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.SocijalniNatjecajBodovnaGreske
            .Where(g => g.ZahtjevId == zahtjevId)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<SocijalniNatjecajBodovnaGreska>> PronadiGreskeAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        var errors = new Dictionary<string, SocijalniNatjecajBodovnaGreska>();
        var today = DateOnly.FromDateTime(DateTime.Today);

        // Provjere
        ProvjeriDatumPodnosenja();
        ProvjeriNekretnine();
        ProvjeriKucanstvo();
        ProvjeriPodnositelja();
        ProvjeriPrihode();
        ProvjeriSamackoKucanstvo();

        return Task.FromResult(errors.Values.ToList());
        
        void Add(string code, string message)
        {
            if (!errors.ContainsKey(code))
            {
                errors[code] = new SocijalniNatjecajBodovnaGreska
                {
                    ZahtjevId = zahtjev.Id,
                    Kod = code,
                    Poruka = message
                };
            }
        }

        
        void ProvjeriDatumPodnosenja()
        {
            var nat = zahtjev.Natjecaj;
            if (nat is null) return;

            var datum = DateOnly.FromDateTime(zahtjev.DatumPodnosenjaZahtjeva);
            if (datum < nat.DatumObjave || datum > nat.RokZaPrijavu)
                Add("DAT-001", "Zahtjev je unesen izvan roka natječaja.");
        }

        void ProvjeriNekretnine()
        {
            if (zahtjev.PosjedujeNekretninuZg)
                Add("NEK-001", "Podnositelj ne smije imati useljivu nekretninu u Zagrebu ili ZG županiji.");
        }

        void ProvjeriKucanstvo()
        {
            var prebivanjeOd = zahtjev.KucanstvoPodaci?.PrebivanjeOd;

            if (prebivanjeOd is { } date && date != DateOnly.MinValue)
            {
                if (today < date.AddYears(3))
                    Add("PRE-001", "Prebivalište kućanstva kraće od 3 godine.");
            }
            else
            {
                Add("PRE-002", "Nije unesen podatak o prebivalištu kućanstva.");
            }

            if ((int?)zahtjev.KucanstvoPodaci?.StambeniStatusKucanstva is 0 or null)
                Add("STA-001", "Nije odabran stambeni status kućanstva.");

            if ((int?)zahtjev.KucanstvoPodaci?.SastavKucanstva is 0 or null)
                Add("SAS-001", "Nije odabran sastav kućanstva.");
        }

        void ProvjeriPodnositelja()
        {
            var p = zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);

            if (p == null || p.DatumRodjenja == DateOnly.MinValue)
            {
                Add("POD-001", "Podnositelj nema unesen datum rođenja.");
            }
            else if (today < p.DatumRodjenja.AddYears(18))
            {
                Add("POD-002", "Podnositelj zahtjeva ne može biti maloljetna osoba.");
            }

            foreach (var clan in zahtjev.Clanovi.Where(c => string.IsNullOrWhiteSpace(c.Oib)))
            {
                Add("OIB-001", $"Član '{clan.ImePrezime}' nema unesen OIB.");
            }

            var preb = zahtjev.KucanstvoPodaci?.PrebivanjeOd ?? DateOnly.MinValue;
            if (preb != DateOnly.MinValue && today < preb.AddYears(3))
            {
                Add("PRE-003", "Podnositelj ima prebivalište kraće od 3 godine.");
            }
        }

        void ProvjeriPrihode()
        {
            var prihod = zahtjev.KucanstvoPodaci?.Prihod;

            if (prihod == null)
            {
                Add("PRI-001", "Prihod nije unesen.");
                return;
            }

            var prosjekPlace = zahtjev.Natjecaj?.ProsjekPlace ?? 0m;
            if (prosjekPlace <= 0)
            {
                Add("PRI-003", "Prosječna plaća nije definirana u natječaju.");
                return;
            }

            var brojClanova = Math.Max(zahtjev.Clanovi.Count, 1);
            var prihodPoClanu = prihod.UkupniPrihodKucanstva / brojClanova / 12m;
            var jeSamacko = zahtjev.KucanstvoPodaci?.SastavKucanstva == SastavKucanstva.SamackoKucanstvo;
            var limit = prosjekPlace * (jeSamacko ? 0.50m : 0.30m);

            if (prihodPoClanu > limit)
                Add("PRI-002", $"Mjesečni prihod po članu ({prihodPoClanu:C}) prelazi dopušteni ({limit:C}).");
        }

        void ProvjeriSamackoKucanstvo()
        {
            if (zahtjev.KucanstvoPodaci?.SastavKucanstva != SastavKucanstva.SamackoKucanstvo)
                return;

            if (zahtjev.Clanovi.Count > 1)
                Add("SAM-001", "Samačko kućanstvo ne može imati više od jednog člana.");

            if (zahtjev.BodovniPodaci?.BrojMaloljetnihKorisnikaInvalidnine > 0)
                Add("SAM-002", "Samačko kućanstvo ne može imati maloljetnog korisnika invalidnine.");

            if (zahtjev.BodovniPodaci?.StatusRoditeljaNjegovatelja is true)
                Add("SAM-003", "Samačko kućanstvo ne može imati status roditelja njegovatelja.");

            if (zahtjev.BodovniPodaci?.BrojUzdrzavanePunoljetneDjece > 0)
                Add("SAM-004", "Samačko kućanstvo ne može imati uzdržavanu punoljetnu djecu.");

            if (zahtjev.BodovniPodaci?.BrojOsobaUAlternativnojSkrbi > 0)
                Add("SAM-005", "Samačko kućanstvo ne može imati osobu iz alternativne skrbi.");

            if (zahtjev.BodovniPodaci?.BrojOdraslihKorisnikaInvalidnine > 0)
                Add("SAM-006", "Samačko kućanstvo ne može imati odraslog korisnika invalidnine.");
        }

    }
}