using Microsoft.EntityFrameworkCore;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services
{
    public class SocijalniKucanstvoService : ISocijalniKucanstvoService
    {
        private readonly ApplicationDbContext _context;

        public SocijalniKucanstvoService(ApplicationDbContext context)
        {
            _context = context;
        }

        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery()
            => _context.SocijalniNatjecajZahtjevi.Include(z => z.KucanstvoPodaci);

        public async Task<SocijalniKucanstvoPodaciDto> UpdateKucanstvoPodaciAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
        {
            var zahtjev = await BaseZahtjevQuery()
                              .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                          ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            var podaci = zahtjev.KucanstvoPodaci ?? new SocijalniNatjecajKucanstvoPodaci { ZahtjevId = zahtjevId };

            podaci.UkupniPrihodKucanstva = dto.UkupniPrihodKucanstva!.Value;
            podaci.PrebivanjeOd = dto.PrebivanjeOd!.Value;
            podaci.StambeniStatusKucanstva = dto.StambeniStatusKucanstva!.Value;
            podaci.SastavKucanstva = dto.SastavKucanstva!.Value;

            if (zahtjev.KucanstvoPodaci == null)
                _context.SocijalniNatjecajKucanstvoPodaci.Add(podaci);

            await _context.SaveChangesAsync();
            return podaci.ToDto();
        }
    }
}