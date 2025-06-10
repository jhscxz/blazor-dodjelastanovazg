using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniBodovniPodaciService : ISocijalniBodovniPodaciService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserContextService _userContext;

    public SocijalniBodovniPodaciService(ApplicationDbContext context, IUserContextService userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<SocijalniNatjecajBodovniPodaciDto> GetAsync(long zahtjevId)
    {
        var entity = await _context.SocijalniNatjecajBodovniPodaci
                         .Include(x => x.CreatedByUser)
                         .Include(x => x.UpdatedByUser)
                         .FirstOrDefaultAsync(x => x.ZahtjevId == zahtjevId)
                     ?? throw new Exception("Bodovni podaci nisu pronađeni.");

        return new SocijalniNatjecajBodovniPodaciDto
        {
            ZahtjevId = entity.ZahtjevId,
            BrojUzdrzavanePunoljetneDjece = entity.BrojUzdrzavanePunoljetneDjece,
            PrimateljZajamceneMinimalneNaknade = entity.PrimateljZajamceneMinimalneNaknade,
            StatusRoditeljaNjegovatelja = entity.StatusRoditeljaNjegovatelja,
            KorisnikDoplatkaZaPomoc = entity.KorisnikDoplatkaZaPomoc,
            BrojOdraslihKorisnikaInvalidnine = entity.BrojOdraslihKorisnikaInvalidnine,
            BrojMaloljetnihKorisnikaInvalidnine = entity.BrojMaloljetnihKorisnikaInvalidnine,
            ZrtvaObiteljskogNasilja = entity.ZrtvaObiteljskogNasilja,
            BrojOsobaUAlternativnojSkrbi = entity.BrojOsobaUAlternativnojSkrbi,
            BrojMjeseciObranaSuvereniteta = entity.BrojMjeseciObranaSuvereniteta,
            BrojClanovaZrtavaSeksualnogNasilja = entity.BrojClanovaZrtavaSeksualnogNasilja,
            BrojCivilnihStradalnika = entity.BrojCivilnihStradalnika,

            CreatedAt = entity.CreatedAt,
            CreatedBy = entity.CreatedBy,
            CreatedByUserName = entity.CreatedByUser?.UserName,
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = entity.UpdatedBy,
            UpdatedByUserName = entity.UpdatedByUser?.UserName,
        };
    }

    public async Task UpdateAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
    {
        var zahtjev = await _context.SocijalniNatjecajZahtjevi
            .Include(z => z.BodovniPodaci)
            .FirstOrDefaultAsync(z => z.Id == zahtjevId)
            ?? throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

        var b = zahtjev.BodovniPodaci ?? throw new Exception("Bodovni podaci nisu definirani.");

        b.BrojUzdrzavanePunoljetneDjece = dto.BrojUzdrzavanePunoljetneDjece;
        b.PrimateljZajamceneMinimalneNaknade = dto.PrimateljZajamceneMinimalneNaknade;
        b.StatusRoditeljaNjegovatelja = dto.StatusRoditeljaNjegovatelja;
        b.KorisnikDoplatkaZaPomoc = dto.KorisnikDoplatkaZaPomoc;
        b.BrojOdraslihKorisnikaInvalidnine = dto.BrojOdraslihKorisnikaInvalidnine;
        b.BrojMaloljetnihKorisnikaInvalidnine = dto.BrojMaloljetnihKorisnikaInvalidnine;
        b.ZrtvaObiteljskogNasilja = dto.ZrtvaObiteljskogNasilja;
        b.BrojOsobaUAlternativnojSkrbi = dto.BrojOsobaUAlternativnojSkrbi;
        b.BrojMjeseciObranaSuvereniteta = dto.BrojMjeseciObranaSuvereniteta;
        b.BrojClanovaZrtavaSeksualnogNasilja = dto.BrojClanovaZrtavaSeksualnogNasilja;
        b.BrojCivilnihStradalnika = dto.BrojCivilnihStradalnika;

        // ✅ Audit samo na zahtjevu
        AuditHelper.ApplyAudit(zahtjev, _userContext.GetCurrentUserId(), isCreate: false);
    }
}
