using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.Exceptions;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services
{
    public class SocijalniKucanstvoService(ApplicationDbContext context) : ISocijalniKucanstvoService
    {
        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery()
            => context.SocijalniNatjecajZahtjevi
                .Include(z => z.KucanstvoPodaci)
                .ThenInclude(k => k!.Prihod); 
        public async Task<SocijalniKucanstvoPodaciDto> UpdateKucanstvoPodaciAsync(
            long zahtjevId,
            SocijalniKucanstvoPodaciDto dto)
        {
            var zahtjev = await context.SocijalniNatjecajZahtjevi
                              .Include(z => z.KucanstvoPodaci)
                              .ThenInclude(k => k!.Prihod)
                              .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                          ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            var podaci = zahtjev.KucanstvoPodaci;

            if (podaci == null)
            {
                podaci = new SocijalniNatjecajKucanstvoPodaci { ZahtjevId = zahtjevId };
                context.SocijalniNatjecajKucanstvoPodaci.Add(podaci);
                await context.SaveChangesAsync();
            }

            // --- PRIHOD ---
            var iznosPrihoda = dto.Prihod?.UkupniPrihodKucanstva ?? throw new ValidationException("Prihod nije naveden.");

            podaci.Prihod.UkupniPrihodKucanstva = iznosPrihoda;
            context.Entry(podaci.Prihod).State = EntityState.Modified;

            // --- OSTALO ---
            podaci.PrebivanjeOd = dto.PrebivanjeOd!.Value;
            podaci.StambeniStatusKucanstva = dto.StambeniStatusKucanstva!.Value;
            podaci.SastavKucanstva = dto.SastavKucanstva!.Value;

            context.Entry(podaci).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return podaci.ToDto();
        }

        public async Task<SocijalniKucanstvoPodaciDto?> GetAsync(long zahtjevId)
        {
            var podaci = await context.SocijalniNatjecajKucanstvoPodaci
                .Include(p => p.Prihod)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ZahtjevId == zahtjevId);

            return podaci?.ToDto();
        }

    }
}