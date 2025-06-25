using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.Exceptions;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services
{
    public class SocijalniBodovniPodaciService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<SocijalniBodovniPodaciService> logger) : ISocijalniBodovniPodaciService
    {
        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery(ApplicationDbContext context, bool asNoTracking = false)
        {
            var query = context.SocijalniNatjecajZahtjevi
                                .Include(z => z.BodovniPodaci).ThenInclude(b => b!.CreatedByUser)
                                .Include(z => z.BodovniPodaci).ThenInclude(b => b!.UpdatedByUser)
                                .Include(z => z.Natjecaj);

            return asNoTracking ? query.AsNoTracking() : query;
        }

        public async Task<SocijalniNatjecajBodovniPodaciDto> GetAsync(long zahtjevId)
        {
            logger.LogDebug("Dohvaćanje bodovnih podataka za zahtjev {ZahtjevId}", zahtjevId);
            
            await using var context = await contextFactory.CreateDbContextAsync();
            
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
            
            await using var context = await contextFactory.CreateDbContextAsync();
            var zahtjev = await BaseZahtjevQuery(context)
                              .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                          ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            var isClosed = await context.Natjecaji
                .Where(n => n.Id == zahtjev.NatjecajId)
                .Select(n => n.IsClosed)
                .FirstAsync();
            if (isClosed)
            {
                logger.LogWarning("Natječaj {NatjecajId} je zaključen i izmjene nisu moguće", zahtjev.NatjecajId);
                throw new InvalidOperationException($"Natječaj {zahtjev.NatjecajId} je zaključen i izmjene nisu moguće");
            }
            
            var b = zahtjev.BodovniPodaci
                    ?? throw new NotFoundException("Bodovni podaci nisu definirani.");

            dto.MapOnto(b);

            await context.SaveChangesAsync();

            return b.ToDto();
        }

    }
}