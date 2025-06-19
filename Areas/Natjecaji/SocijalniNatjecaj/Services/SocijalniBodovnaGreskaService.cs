using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;

public class SocijalniBodovnaGreskaService(ApplicationDbContext context)
    : ISocijalniBodovnaGreskaService
{
    public async Task<List<SocijalniNatjecajBodovnaGreska>> GetByZahtjevIdAsync(long zahtjevId) =>
        await context.SocijalniNatjecajBodovnaGreske
            .Where(g => g.ZahtjevId == zahtjevId)
            .ToListAsync();

    public Task<List<SocijalniNatjecajBodovnaGreska>> PronadiGreskeAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        var greske = new List<SocijalniNatjecajBodovnaGreska>();
        var danas = DateOnly.FromDateTime(DateTime.Today);

        void Dodaj(string kod, string poruka) => greske.Add(new SocijalniNatjecajBodovnaGreska
        {
            ZahtjevId = zahtjev.Id,
            Kod = kod,
            Poruka = poruka
        });

        ProvjeriKucanstvo();
        ProvjeriPodnositelja();
        ProvjeriPrihode();
        ProvjeriSamackoKucanstvo();

        return Task.FromResult(greske);

        // -----------------------  Pomoćne funkcije  -----------------------

        void ProvjeriKucanstvo()
        {
            var prebivanjeOd = zahtjev.KucanstvoPodaci?.PrebivanjeOd;

            if (prebivanjeOd is { } datum && datum != DateOnly.MinValue)
            {
                if (danas.Year - datum.Year < 3)
                    Dodaj("PRE-001", "Prebivalište kućanstva kraće od 3 godine.");
            }
            else
            {
                Dodaj("PRE-002", "Nije unesen podatak o prebivalištu kućanstva.");
            }

            if ((int?)(zahtjev.KucanstvoPodaci?.StambeniStatusKucanstva) is 0 or null)
                Dodaj("STA-001", "Nije odabran stambeni status kućanstva.");

            if ((int?)(zahtjev.KucanstvoPodaci?.SastavKucanstva) is 0 or null)
                Dodaj("SAS-001", "Nije odabran sastav kućanstva.");
        }

        void ProvjeriPodnositelja()
        {
            var p = zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);

            if (p == null || p.DatumRodjenja == DateOnly.MinValue)
            {
                Dodaj("POD-001", "Podnositelj nema unesen ispravan datum rođenja.");
            }
            else if (p.DatumRodjenja.AddYears(18) > danas)
            {
                Dodaj("POD-001", "Podnositelj zahtjeva ne može biti maloljetna osoba.");
            }

            if (string.IsNullOrWhiteSpace(p?.Oib))
                Dodaj("POD-002", "Podnositelj nema unesen OIB.");

            foreach (var clan in zahtjev.Clanovi.Where(c => string.IsNullOrWhiteSpace(c.Oib)))
                Dodaj("OIB-001", $"Član '{clan.ImePrezime}' nema unesen OIB.");

            var preb = p?.Zahtjev?.KucanstvoPodaci?.PrebivanjeOd ?? DateOnly.MinValue;
            if (preb != DateOnly.MinValue && danas.Year - preb.Year < 3)
                Dodaj("PRE-003", "Podnositelj ima prebivalište kraće od 3 godine.");
        }

        void ProvjeriPrihode()
        {
            var prihod = zahtjev.KucanstvoPodaci?.Prihod;
            if (prihod is null)
            {
                Dodaj("PRI-001", "Nije unesen podatak o ukupnom prihodu kućanstva.");
                return;
            }

            // Prosječna plaća definirana u natječaju (npr. 1 445 €)
            var prosjekPlace = zahtjev.Natjecaj?.ProsjekPlace ?? 0m;
            if (prosjekPlace <= 0)
            {
                Dodaj("PRI-003", "Prosječna plaća nije definirana u natječaju.");
                return;
            }

            var brojClanova = Math.Max(zahtjev.Clanovi.Count, 1); // safety-guard
            var prihodPoClanuMjesecno = prihod.UkupniPrihodKucanstva / brojClanova / 12m;

            var jeSamacko = zahtjev.KucanstvoPodaci?.SastavKucanstva
                            == SastavKucanstva.SamackoKucanstvo;

            var limit = prosjekPlace * (jeSamacko ? 0.50m : 0.30m);

            if (prihodPoClanuMjesecno > limit)
                Dodaj("PRI-002",
                    $"Mjesečni prihod po članu ({prihodPoClanuMjesecno:C}) " +
                    $"prelazi dopušteni ({limit:C}).");
        }


        void ProvjeriSamackoKucanstvo()
        {
            if (zahtjev.KucanstvoPodaci?.SastavKucanstva != SastavKucanstva.SamackoKucanstvo)
                return;

            if (zahtjev.Clanovi.Count > 1)
                Dodaj("SAM-001", "Samačko kućanstvo ne može imati više od jednog člana.");

            if (zahtjev.BodovniPodaci?.BrojMaloljetnihKorisnikaInvalidnine > 0)
                Dodaj("SAM-002", "Samačko kućanstvo ne može imati maloljetnog korisnika invalidnine.");

            if (zahtjev.BodovniPodaci?.StatusRoditeljaNjegovatelja == true)
                Dodaj("SAM-003", "Samačko kućanstvo ne može imati status roditelja njegovatelja.");

            if (zahtjev.BodovniPodaci?.BrojUzdrzavanePunoljetneDjece > 0)
                Dodaj("SAM-004", "Samačko kućanstvo ne može imati uzdržavanu punoljetnu djecu.");

            if (zahtjev.BodovniPodaci?.BrojOsobaUAlternativnojSkrbi > 0)
                Dodaj("SAM-005", "Samačko kućanstvo ne može imati osobu iz alternativne skrbi.");

            if (zahtjev.BodovniPodaci?.BrojOdraslihKorisnikaInvalidnine > 0)
                Dodaj("SAM-006", "Samačko kućanstvo ne može imati odraslog korisnika invalidnine.");
        }
    }
}