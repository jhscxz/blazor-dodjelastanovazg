using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Helpers.IServices;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevWriteService(
    ApplicationDbContext context,
    IAuditService auditService,
    IUserContextService userContext)
    : ISocijalniZahtjevWriteService
{
    private readonly IAuditService _auditService = auditService;
    public async Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        _auditService.ApplyAudit(zahtjev, true);
        _auditService.ApplyAudit(zahtjev.Clanovi, true);
        _auditService.ApplyAudit(new object[]
        {
            zahtjev.KucanstvoPodaci!,
            zahtjev.BodovniPodaci!,
            zahtjev.Bodovi!
        }, true);

        // Dodaj zahtjev (s pripadajućim entitetima)
        await context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);
        await context.SaveChangesAsync(); // Spremi prvo zahtjev da dobije Id-e

        // Kreiraj prihod i poveži ga preko navigacijskog svojstva
        var prihod = new SocijalniPrihodi
        {
            KucanstvoPodaci = zahtjev.KucanstvoPodaci!,  // EF će automatski postaviti Id i FK
            UkupniPrihodKucanstva = 0,
            PrihodPoClanu = 0,
            IspunjavaUvjetPrihoda = true
        };

        _auditService.ApplyAudit(prihod, true);
        await context.SocijalniPrihodi.AddAsync(prihod);
        await context.SaveChangesAsync();

        return zahtjev;
    }

    public async Task UpdateOsnovnoAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi
                          .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                      ?? throw new Exception($"Zahtjev {zahtjevId} nije pronađen.");

        dto.MapOnto(zahtjev);
        _auditService.ApplyAudit(zahtjev, false);
        await context.SaveChangesAsync();
    }
}