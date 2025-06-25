using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevWriteService(IDbContextFactory<ApplicationDbContext> contextFactory, IAuditService auditService, ILogger<SocijalniZahtjevWriteService> logger) : ISocijalniZahtjevWriteService
{
    public async Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        logger.LogInformation("Creating zahtjev");
        
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var natjecaj = await context.Natjecaji.FindAsync(zahtjev.NatjecajId)
                       ?? throw new Exception($"Natječaj {zahtjev.NatjecajId} nije pronađen.");
        if (natjecaj.IsClosed)
        {
            logger.LogWarning("Natječaj {NatjecajId} je zaključen i izmjene nisu moguće", natjecaj.Id);
            throw new InvalidOperationException($"Natječaj {natjecaj.Id} je zaključen i izmjene nisu moguće");
        }
        
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

        await SaveAsync(context);
        logger.LogInformation("Created zahtjev {Id}", zahtjev.Id);
        return zahtjev;
    }

    public async Task UpdateOsnovnoAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        logger.LogInformation("Updating osnovne podatke zahtjeva {Id}", zahtjevId);
        await using var context = await contextFactory.CreateDbContextAsync();
        var zahtjev = await context.SocijalniNatjecajZahtjevi
                          .Include(z => z.Natjecaj)
                          .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                      ?? throw new Exception($"Zahtjev {zahtjevId} nije pronađen.");

        var isClosed = await context.Natjecaji
            .Where(n => n.Id == zahtjev.NatjecajId)
            .Select(n => n.IsClosed)
            .FirstAsync();
        if (isClosed)
        {
            logger.LogWarning("Natječaj {NatjecajId} je zaključen i izmjene nisu moguće", zahtjev.NatjecajId);
            throw new InvalidOperationException($"Natječaj {zahtjev.NatjecajId} je zaključen i izmjene nisu moguće");
        }

        dto.MapOnto(zahtjev);
        auditService.ApplyAudit(zahtjev, false);

        if (dto.RowVersion is not null)
            context.Entry(zahtjev).Property(z => z.RowVersion).OriginalValue = dto.RowVersion;
        
        await SaveAsync(context);
        logger.LogInformation("Updated osnovne podatke zahtjeva {Id}", zahtjevId);
    }

    private async Task SaveAsync(ApplicationDbContext context)
    {
        try
        {
            await context.SaveChangesAsync();
            logger.LogDebug("Changes saved to database");
        }
        catch (DbUpdateConcurrencyException)
        {
            logger.LogError("Concurrency conflict while saving");
            throw new InvalidOperationException(
                "Netko je u međuvremenu promijenio podatke. Osvježite stranicu pa pokušajte ponovo.");
        }
    }
}