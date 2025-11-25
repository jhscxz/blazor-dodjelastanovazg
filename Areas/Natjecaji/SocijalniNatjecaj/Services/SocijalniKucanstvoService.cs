using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.Exceptions;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services
{
    public class SocijalniKucanstvoService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<SocijalniKucanstvoService> logger) : ISocijalniKucanstvoService
    {
        public async Task<SocijalniKucanstvoPodaciDto> UpdateKucanstvoPodaciAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
        {
            logger.LogInformation("Updating kućanstvo podaci for zahtjev {Id}", zahtjevId);

            await using var context = await contextFactory.CreateDbContextAsync();
            var zahtjev = await context.SocijalniNatjecajZahtjevi
                              .Include(z => z.Natjecaj)
                              .Include(z => z.KucanstvoPodaci)
                              .ThenInclude(k => k!.Prihod)
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
            var podaci = zahtjev.KucanstvoPodaci;

            if (podaci == null)
            {
                podaci = new SocijalniNatjecajKucanstvoPodaci { ZahtjevId = zahtjevId };
                context.SocijalniNatjecajKucanstvoPodaci.Add(podaci);
                await context.SaveChangesAsync();
                logger.LogInformation("Created new kućanstvo podaci for zahtjev {Id}", zahtjevId);
            }

            var iznosPrihoda = dto.Prihod?.UkupniPrihodKucanstva ?? throw new ValidationException("Prihod nije naveden.");

            if (podaci.Prihod != null)
            {
                podaci.Prihod.UkupniPrihodKucanstva = iznosPrihoda;
                context.Entry(podaci.Prihod).State = EntityState.Modified;
            }

            podaci.PrebivanjeOd = dto.PrebivanjeOd!.Value;
            podaci.StambeniStatusKucanstva = dto.StambeniStatusKucanstva!.Value;
            podaci.SastavKucanstva = dto.SastavKucanstva!.Value;

            context.Entry(podaci).State = EntityState.Modified;

            await context.SaveChangesAsync();
            logger.LogInformation("Saved kućanstvo podaci for zahtjev {Id}", zahtjevId);

            return podaci.ToDto();
        }

        public async Task<SocijalniKucanstvoPodaciDto?> GetAsync(long zahtjevId)
        {
            logger.LogInformation("Fetching kućanstvo podaci for zahtjev {Id}", zahtjevId);

            await using var context = await contextFactory.CreateDbContextAsync();
            var podaci = await context.SocijalniNatjecajKucanstvoPodaci
                .Include(p => p.Prihod)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ZahtjevId == zahtjevId);

            logger.LogInformation(podaci != null
                ? "kućanstvo podaci found for zahtjev {Id}"
                : "kućanstvo podaci not found for zahtjev {Id}", zahtjevId);

            return podaci?.ToDto();
        }
    }
}