using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevWriteService(
    ApplicationDbContext context,
    IAuditService auditService)
    : ISocijalniZahtjevWriteService
{
    public async Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        auditService.ApplyAudit(zahtjev, true);
        auditService.ApplyAudit(zahtjev.Clanovi, true);
        auditService.ApplyAudit([
            zahtjev.KucanstvoPodaci!,
            zahtjev.BodovniPodaci!,
            zahtjev.Bodovi!
        ], true);

        var prihod = new SocijalniPrihodi
        {
            KucanstvoPodaci = zahtjev.KucanstvoPodaci!,
            UkupniPrihodKucanstva = 0,
            PrihodPoClanu = 0,
            IspunjavaUvjetPrihoda = true
        };
        auditService.ApplyAudit(prihod, true);

        await context.AddAsync(zahtjev);
        await context.AddAsync(prihod);

        await SaveAsync();
        return zahtjev;
    }

    public async Task UpdateOsnovnoAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi
                          .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                      ?? throw new Exception($"Zahtjev {zahtjevId} nije pronađen.");

        dto.MapOnto(zahtjev);
        auditService.ApplyAudit(zahtjev, false);

        await SaveAsync();
    }

    private async Task SaveAsync()
    {
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException(
                "Netko je u međuvremenu promijenio podatke. Osvježite stranicu pa pokušajte ponovo.");
        }
    }
}