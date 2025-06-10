using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniKucanstvoService : ISocijalniKucanstvoService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserContextService _userContext;

    public SocijalniKucanstvoService(ApplicationDbContext context, IUserContextService userContext)
    {
        _context = context;
        _userContext = userContext;
    }
    
    public async Task UpdateKucanstvoPodaciAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
    {
        var zahtjev = await _context.SocijalniNatjecajZahtjevi
                          .Include(z => z.KucanstvoPodaci)
                          .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                      ?? throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

        if (zahtjev.KucanstvoPodaci == null)
        {
            zahtjev.KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci { ZahtjevId = zahtjevId };
            _context.SocijalniNatjecajKucanstvoPodaci.Add(zahtjev.KucanstvoPodaci);
        }

        zahtjev.KucanstvoPodaci.UkupniPrihodKucanstva = dto.UkupniPrihodKucanstva!.Value;
        zahtjev.KucanstvoPodaci.PrebivanjeOd = dto.PrebivanjeOd!.Value;
        zahtjev.KucanstvoPodaci.StambeniStatusKucanstva = dto.StambeniStatusKucanstva!.Value;
        zahtjev.KucanstvoPodaci.SastavKucanstva = dto.SastavKucanstva!.Value;

        AuditHelper.ApplyAudit(zahtjev, _userContext.GetCurrentUserId(), isCreate: false);
    }
}