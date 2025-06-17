using Microsoft.EntityFrameworkCore;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.SocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.Exceptions;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services
{
    public class SocijalniKucanstvoService(ApplicationDbContext _context) : ISocijalniKucanstvoService
    {
        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery()
            => _context.SocijalniNatjecajZahtjevi
                .Include(z => z.KucanstvoPodaci)
                .ThenInclude(k => k!.Prihod); 
        public async Task<SocijalniKucanstvoPodaciDto> UpdateKucanstvoPodaciAsync(
        long zahtjevId,
        SocijalniKucanstvoPodaciDto dto)
    {
        // 1) Učitaj zahtjev s kućanstvom i prihodom
        var zahtjev = await _context.SocijalniNatjecajZahtjevi
            .Include(z => z.KucanstvoPodaci)
                .ThenInclude(k => k.Prihod)
            .FirstOrDefaultAsync(z => z.Id == zahtjevId)
            ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

        // 2) Ako nema kućanstva, kreiraj i spremi ga da dobije ID
        var podaci = zahtjev.KucanstvoPodaci;
        if (podaci == null)
        {
            podaci = new SocijalniNatjecajKucanstvoPodaci { ZahtjevId = zahtjevId };
            _context.SocijalniNatjecajKucanstvoPodaci.Add(podaci);
            await _context.SaveChangesAsync();
        }

        // 3) Ako nema prihoda, kreiraj ga; inače ažuriraj samo prihod iz DTO-a
        if (podaci.Prihod == null)
        {
            podaci.Prihod = new SocijalniPrihodi
            {
                Id = podaci.Id,
                UkupniPrihodKucanstva = dto.UkupniPrihodKucanstva!.Value,
                PrihodPoClanu        = 0m,
                IspunjavaUvjetPrihoda = false,
                PostotakProsjeka      = null
            };
            _context.SocijalniPrihodi.Add(podaci.Prihod);
        }
        else
        {
            podaci.Prihod.UkupniPrihodKucanstva = dto.UkupniPrihodKucanstva!.Value;
            _context.Entry(podaci.Prihod).State = EntityState.Modified;
        }

        // 4) Ažuriraj ostala polja kućanstva
        podaci.PrebivanjeOd            = dto.PrebivanjeOd!.Value;
        podaci.StambeniStatusKucanstva = dto.StambeniStatusKucanstva!.Value;
        podaci.SastavKucanstva         = dto.SastavKucanstva!.Value;
        _context.Entry(podaci).State = EntityState.Modified;

        // 5) Spremi sve promjene
        await _context.SaveChangesAsync();

        // 6) Vrati ažurirani DTO
        return podaci.ToDto();
    }

    }
}