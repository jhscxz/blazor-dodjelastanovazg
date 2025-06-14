using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniBodovnaGreskaService(ApplicationDbContext context) : ISocijalniBodovnaGreskaService
{
    public async Task<List<SocijalniNatjecajBodovnaGreska>> GetByZahtjevIdAsync(long zahtjevId)
    {
        return await context.SocijalniNatjecajBodovnaGreske
            .Where(g => g.ZahtjevId == zahtjevId)
            .ToListAsync();
    }

    public Task<List<SocijalniNatjecajBodovnaGreska>> PronadiGreskeAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        var greske = new List<SocijalniNatjecajBodovnaGreska>();
        var danas = DateOnly.FromDateTime(DateTime.Today);

        // === OBVEZNI PODACI KUĆANSTVA ===

        // PRE-002: nije uneseno prebivanje
        if (zahtjev.KucanstvoPodaci?.PrebivanjeOd is { } prebivanjeOd && prebivanjeOd != DateOnly.MinValue)
        {
            // PRE-001: prebivanje kraće od 3 godine
            var brojGodina = danas.Year - prebivanjeOd.Year;
            if (brojGodina < 3)
            {
                greske.Add(NovaGreska(zahtjev.Id, "PRE-001", "Prebivalište je kraće od 3 godine."));
            }
        }
        else
        {
            greske.Add(NovaGreska(zahtjev.Id, "PRE-002", "Nije unesen podatak o prebivalištu."));
        }

        // STA-001: nije odabran stambeni status
        if (zahtjev.KucanstvoPodaci == null || (int)zahtjev.KucanstvoPodaci.StambeniStatusKucanstva == 0)
        {
            greske.Add(NovaGreska(zahtjev.Id, "STA-001", "Nije odabran stambeni status kućanstva."));
        }

        // SAS-001: nije odabran sastav kućanstva
        if (zahtjev.KucanstvoPodaci == null || (int)zahtjev.KucanstvoPodaci.SastavKucanstva == 0)
        {
            greske.Add(NovaGreska(zahtjev.Id, "SAS-001", "Nije odabran sastav kućanstva."));
        }

        // === PODNOSITELJ ===

        var podnositelj = zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
        if (podnositelj is null || podnositelj.DatumRodjenja == DateOnly.MinValue)
        {
            greske.Add(NovaGreska(zahtjev.Id, "POD-001", "Podnositelj nema unesen ispravan datum rođenja."));
        }
        else if (podnositelj.DatumRodjenja.AddYears(18) > danas)
        {
            greske.Add(NovaGreska(zahtjev.Id, "POD-001", "Podnositelj zahtjeva ne može biti maloljetna osoba."));
        }

        // === LOGIČKA PRAVILA ZA SAMAČKO KUĆANSTVO ===

        if (zahtjev.KucanstvoPodaci?.SastavKucanstva == SastavKucanstva.SamackoKucanstvo)
        {
            // SAM-001: Samac ne može imati više od jednog člana
            if (zahtjev.Clanovi.Count > 1)
            {
                greske.Add(NovaGreska(zahtjev.Id, "SAM-001", "Samačko kućanstvo ne može imati više od jednog člana."));
            }

            // SAM-002: Samac ne može imati maloljetnog korisnika invalidnine
            if (zahtjev.BodovniPodaci?.BrojMaloljetnihKorisnikaInvalidnine > 0)
            {
                greske.Add(NovaGreska(zahtjev.Id, "SAM-002", "Samačko kućanstvo ne može imati maloljetnog korisnika invalidnine."));
            }

            // SAM-003: Samac ne može imati status roditelja njegovatelja
            if (zahtjev.BodovniPodaci?.StatusRoditeljaNjegovatelja == true)
            {
                greske.Add(NovaGreska(zahtjev.Id, "SAM-003", "Samačko kućanstvo ne može imati status roditelja njegovatelja."));
            }

            // SAM-004: Samac ne može imati uzdržavanu punoljetnu djecu
            if (zahtjev.BodovniPodaci?.BrojUzdrzavanePunoljetneDjece > 0)
            {
                greske.Add(NovaGreska(zahtjev.Id, "SAM-004", "Samačko kućanstvo ne može imati uzdržavanu punoljetnu djecu."));
            }

            // SAM-005: Samac ne može biti osoba u alternativnoj skrbi
            if (zahtjev.BodovniPodaci?.BrojOsobaUAlternativnojSkrbi > 0)
            {
                greske.Add(NovaGreska(zahtjev.Id, "SAM-005", "Samačko kućanstvo ne može imati osobu iz alternativne skrbi."));
            }

            // SAM-006: Samac ne može biti odrasla osoba s invalidninom
            if (zahtjev.BodovniPodaci?.BrojOdraslihKorisnikaInvalidnine > 0)
            {
                greske.Add(NovaGreska(zahtjev.Id, "SAM-006", "Samačko kućanstvo ne može imati odraslog korisnika invalidnine."));
            }
        }

        return Task.FromResult(greske);
    }

    private static SocijalniNatjecajBodovnaGreska NovaGreska(long zahtjevId, string kod, string poruka)
    {
        return new SocijalniNatjecajBodovnaGreska
        {
            ZahtjevId = zahtjevId,
            Kod = kod,
            Poruka = poruka
        };
    }
}
