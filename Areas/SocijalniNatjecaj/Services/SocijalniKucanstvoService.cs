using Microsoft.EntityFrameworkCore;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services
{
    public class SocijalniKucanstvoService : ISocijalniKucanstvoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContext;

        public SocijalniKucanstvoService(ApplicationDbContext context, IUserContextService userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery()
            => _context.SocijalniNatjecajZahtjevi
                       .Include(z => z.KucanstvoPodaci);

        public async Task<SocijalniKucanstvoPodaciDto> UpdateKucanstvoPodaciAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
        {
            var zahtjev = await BaseZahtjevQuery()
                .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            var podaci = zahtjev.KucanstvoPodaci;
            if (podaci == null)
            {
                podaci = new SocijalniNatjecajKucanstvoPodaci { ZahtjevId = zahtjevId };
                _context.SocijalniNatjecajKucanstvoPodaci.Add(podaci);
            }

            // mapiramo nove vrijednosti
            podaci.UkupniPrihodKucanstva   = dto.UkupniPrihodKucanstva!.Value;
            podaci.PrebivanjeOd            = dto.PrebivanjeOd!.Value;
            podaci.StambeniStatusKucanstva = dto.StambeniStatusKucanstva!.Value;
            podaci.SastavKucanstva         = dto.SastavKucanstva!.Value;

            // audit samo na entitetu kućanstva
            //AuditHelper.ApplyAudit(podaci, _userContext.GetCurrentUserId(), isCreate: false);

            await _context.SaveChangesAsync();

            // vratimo osvježeni DTO
            return podaci.ToDto();
        }
    }
}
