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
    public class SocijalniBodovniPodaciService(ApplicationDbContext context) : ISocijalniBodovniPodaciService
    {
        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery(bool asNoTracking = false)
        {
            var query = context.SocijalniNatjecajZahtjevi
                                .Include(z => z.BodovniPodaci).ThenInclude(b => b!.CreatedByUser)
                                .Include(z => z.BodovniPodaci).ThenInclude(b => b!.UpdatedByUser);

            return asNoTracking ? query.AsNoTracking() : query;
        }

        public async Task<SocijalniNatjecajBodovniPodaciDto> GetAsync(long zahtjevId)
        {
            var entity = await context.SocijalniNatjecajBodovniPodaci
                               .Include(b => b.CreatedByUser)
                               .Include(b => b.UpdatedByUser)
                               .FirstOrDefaultAsync(b => b.ZahtjevId == zahtjevId)
                           ?? throw new NotFoundException("Bodovni podaci nisu pronađeni.");

            return entity.ToDto();
        }

        public async Task<SocijalniNatjecajBodovniPodaciDto> UpdateAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
        {
            var zahtjev = await BaseZahtjevQuery()
                              .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                          ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            var b = zahtjev.BodovniPodaci
                    ?? throw new NotFoundException("Bodovni podaci nisu definirani.");

            dto.MapOnto(b);

            await context.SaveChangesAsync();

            return b.ToDto();
        }

    }
}