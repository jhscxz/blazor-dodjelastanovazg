using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniClanService : ISocijalniClanService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserContextService _userContext;

    public SocijalniClanService(ApplicationDbContext context, IUserContextService userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<SocijalniNatjecajClanDto> AddClanAsync(SocijalniNatjecajClan noviClan)
    {
        var zahtjev = await _context.SocijalniNatjecajZahtjevi
                          .FirstOrDefaultAsync(z => z.Id == noviClan.ZahtjevId)
                      ?? throw new($"Zahtjev s ID-om {noviClan.ZahtjevId} nije pronađen.");

        _context.SocijalniNatjecajClanovi.Add(noviClan);

        AuditHelper.ApplyAudit(zahtjev, _userContext.GetCurrentUserId(), isCreate: false);
        return noviClan.ToDto();
    }

    public async Task<SocijalniNatjecajClanDto> EditClanAsync(SocijalniNatjecajClanDto azurirani)
    {
        var zahtjev = await _context.SocijalniNatjecajZahtjevi
                          .Include(z => z.Clanovi)
                          .FirstOrDefaultAsync(z => z.Id == azurirani.ZahtjevId)
                      ?? throw new($"Zahtjev s ID-om {azurirani.ZahtjevId} nije pronađen.");

        var clan = zahtjev.Clanovi.FirstOrDefault(c => c.Id == azurirani.Id)
                   ?? throw new($"Član s ID-om {azurirani.Id} nije pronađen.");

        clan.ImePrezime = azurirani.ImePrezime;
        clan.Oib = azurirani.Oib;
        clan.Srodstvo = azurirani.Srodstvo;
        clan.DatumRodjenja = azurirani.DatumRodjenja;

        AuditHelper.ApplyAudit(zahtjev, _userContext.GetCurrentUserId(), isCreate: false);
        return clan.ToDto();
    }

    public async Task RemoveClanAsync(long zahtjevId, long clanId)
    {
        var zahtjev = await _context.SocijalniNatjecajZahtjevi
                          .Include(z => z.Clanovi)
                          .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                      ?? throw new($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

        var clan = zahtjev.Clanovi.FirstOrDefault(c => c.Id == clanId)
                   ?? throw new($"Član s ID-om {clanId} nije pronađen.");

        _context.SocijalniNatjecajClanovi.Remove(clan);
        AuditHelper.ApplyAudit(zahtjev, _userContext.GetCurrentUserId(), isCreate: false);
    }
}
