using Microsoft.EntityFrameworkCore;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services
{
    public class SocijalniBodovniPodaciService : ISocijalniBodovniPodaciService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContext;

        public SocijalniBodovniPodaciService(ApplicationDbContext context, IUserContextService userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        private IQueryable<SocijalniNatjecajZahtjev> BaseZahtjevQuery(bool asNoTracking = false)
        {
            var query = _context.SocijalniNatjecajZahtjevi
                                .Include(z => z.BodovniPodaci).ThenInclude(b => b.CreatedByUser)
                                .Include(z => z.BodovniPodaci).ThenInclude(b => b.UpdatedByUser);

            return asNoTracking ? query.AsNoTracking() : query;
        }

        public async Task<SocijalniNatjecajBodovniPodaciDto> GetAsync(long zahtjevId)
        {
            var entity = await _context.SocijalniNatjecajBodovniPodaci
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

            // map fields
            b.BrojUzdrzavanePunoljetneDjece = dto.BrojUzdrzavanePunoljetneDjece;
            b.PrimateljZajamceneMinimalneNaknade = dto.PrimateljZajamceneMinimalneNaknade;
            b.StatusRoditeljaNjegovatelja = dto.StatusRoditeljaNjegovatelja;
            b.KorisnikDoplatkaZaPomoc = dto.KorisnikDoplatkaZaPomoc;
            b.BrojOdraslihKorisnikaInvalidnine = dto.BrojOdraslihKorisnikaInvalidnine;
            b.BrojMaloljetnihKorisnikaInvalidnine = dto.BrojMaloljetnihKorisnikaInvalidnine;
            b.ZrtvaObiteljskogNasilja = dto.ZrtvaObiteljskogNasilja;
            b.BrojOsobaUAlternativnojSkrbi = dto.BrojOsobaUAlternativnojSkrbi;
            b.BrojMjeseciObranaSuvereniteta = dto.BrojMjeseciObranaSuvereniteta;
            b.BrojClanovaZrtavaSeksualnogNasilja  = dto.BrojClanovaZrtavaSeksualnogNasilja;
            b.BrojCivilnihStradalnika = dto.BrojCivilnihStradalnika;

            // audit only bodovni podaci
            // AuditHelper.ApplyAudit(b, _userContext.GetCurrentUserId(), isCreate: false);
            await _context.SaveChangesAsync();

            return b.ToDto();
        }
    }
}