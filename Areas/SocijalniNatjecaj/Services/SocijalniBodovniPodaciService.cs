using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniBodovniPodaciService : ISocijalniBodovniPodaciService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SocijalniBodovniPodaciService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SocijalniNatjecajBodovniPodaciDto> GetAsync(long zahtjevId)
    {
        var entity = await _context.SocijalniNatjecajBodovniPodaci
            .FirstOrDefaultAsync(x => x.ZahtjevId == zahtjevId)
            ?? throw new Exception("Bodovni podaci nisu pronađeni.");

        return new SocijalniNatjecajBodovniPodaciDto()
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
            BrojCivilnihStradalnika = entity.BrojCivilnihStradalnika
        };
    }

    public async Task UpdateAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
    {
        var entity = await _context.SocijalniNatjecajBodovniPodaci
            .FirstOrDefaultAsync(x => x.ZahtjevId == zahtjevId);

        var isCreate = entity == null;

        if (isCreate)
        {
            entity = new SocijalniNatjecajBodovniPodaci
            {
                ZahtjevId = zahtjevId
            };
            _context.SocijalniNatjecajBodovniPodaci.Add(entity);
        }

        entity.BrojUzdrzavanePunoljetneDjece = dto.BrojUzdrzavanePunoljetneDjece;
        entity.PrimateljZajamceneMinimalneNaknade = dto.PrimateljZajamceneMinimalneNaknade;
        entity.StatusRoditeljaNjegovatelja = dto.StatusRoditeljaNjegovatelja;
        entity.KorisnikDoplatkaZaPomoc = dto.KorisnikDoplatkaZaPomoc;
        entity.BrojOdraslihKorisnikaInvalidnine = dto.BrojOdraslihKorisnikaInvalidnine;
        entity.BrojMaloljetnihKorisnikaInvalidnine = dto.BrojMaloljetnihKorisnikaInvalidnine;
        entity.ZrtvaObiteljskogNasilja = dto.ZrtvaObiteljskogNasilja;
        entity.BrojOsobaUAlternativnojSkrbi = dto.BrojOsobaUAlternativnojSkrbi;
        entity.BrojMjeseciObranaSuvereniteta = dto.BrojMjeseciObranaSuvereniteta;
        entity.BrojClanovaZrtavaSeksualnogNasilja = dto.BrojClanovaZrtavaSeksualnogNasilja;
        entity.BrojCivilnihStradalnika = dto.BrojCivilnihStradalnika;

        AuditHelper.ApplyAudit(entity, GetCurrentUserId(), isCreate);
        await _context.SaveChangesAsync();
    }

    private string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? throw new Exception("Korisnik nije prijavljen.");
    }
}
