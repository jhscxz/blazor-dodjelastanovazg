using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.Exceptions;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services
{
    public class SocijalniBodovniPodaciService(
        ApplicationDbContext context,
        ILogger<SocijalniBodovniPodaciService> logger) : ISocijalniBodovniPodaciService
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
            logger.LogDebug("Dohvaćanje bodovnih podataka za zahtjev {ZahtjevId}", zahtjevId);
            
            var entity = await context.SocijalniNatjecajBodovniPodaci
                               .Include(b => b.CreatedByUser)
                               .Include(b => b.UpdatedByUser)
                               .FirstOrDefaultAsync(b => b.ZahtjevId == zahtjevId)
                           ?? throw new NotFoundException("Bodovni podaci nisu pronađeni.");

            return entity.ToDto();
        }

        public async Task<SocijalniNatjecajBodovniPodaciDto> UpdateAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
        {
            logger.LogDebug("Ažuriranje bodovnih podataka za zahtjev {ZahtjevId}", zahtjevId);
            
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