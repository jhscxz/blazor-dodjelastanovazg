using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services
{
    public class SocijalniClanService(ApplicationDbContext context, IUserContextService userContext)
        : ISocijalniClanService
    {
        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery(bool asNoTracking = false)
        {
            var query = context.SocijalniNatjecajZahtjevi
                                .Include(z => z.Clanovi);
            return asNoTracking ? query.AsNoTracking() : query;
        }

        public async Task<SocijalniNatjecajClanDto> AddClanAsync(SocijalniNatjecajClan noviClan)
        {
            var zahtjev = await BaseZahtjevQuery()
                                .FirstOrDefaultAsync(z => z.Id == noviClan.ZahtjevId)
                          ?? throw new NotFoundException($"Zahtjev s ID-om {noviClan.ZahtjevId} nije pronađen.");

            //AuditHelper.ApplyAudit(noviClan, userContext.GetCurrentUserId(), isCreate: true);
            context.SocijalniNatjecajClanovi.Add(noviClan);

            return noviClan.ToDto();
        }

        public async Task<SocijalniNatjecajClanDto> EditClanAsync(SocijalniNatjecajClanDto azurirani)
        {
            var zahtjev = await BaseZahtjevQuery()
                              .FirstOrDefaultAsync(z => z.Id == azurirani.ZahtjevId)
                          ?? throw new NotFoundException($"Zahtjev s ID-om {azurirani.ZahtjevId} nije pronađen.");

            var clan = zahtjev.Clanovi
                           .FirstOrDefault(c => c.Id == azurirani.Id)
                       ?? throw new NotFoundException($"Član s ID-om {azurirani.Id} nije pronađen.");

            clan.ImePrezime = azurirani.ImePrezime;
            clan.Oib = azurirani.Oib;
            clan.Srodstvo = azurirani.Srodstvo;
            clan.DatumRodjenja = azurirani.DatumRodjenja;

            //AuditHelper.ApplyAudit(clan, userContext.GetCurrentUserId(), isCreate: false);

            return clan.ToDto();
        }


        public async Task RemoveClanAsync(long zahtjevId, long clanId)
        {
            var zahtjev = await BaseZahtjevQuery()
                              .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                          ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            var clan = zahtjev.Clanovi
                           .FirstOrDefault(c => c.Id == clanId)
                       ?? throw new NotFoundException($"Član s ID-om {clanId} nije pronađen.");

            context.SocijalniNatjecajClanovi.Remove(clan);

            //AuditHelper.ApplyAudit(clan, userContext.GetCurrentUserId(), isCreate: false);

        }

    }
 
}