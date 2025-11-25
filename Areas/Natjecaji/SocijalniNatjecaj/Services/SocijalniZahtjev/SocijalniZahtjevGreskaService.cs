using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevGreskaService(
    IDbContextFactory<ApplicationDbContext> contextFactory,
    ISocijalniBodovnaGreskaService greskaService,
    IAuditService auditService,
    ILogger<SocijalniZahtjevGreskaService> logger)
    : ISocijalniZahtjevGreskaService
{
    public async Task ObradiGreskeAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        logger.LogInformation("Obrada grešaka za zahtjev {ZahtjevId}", zahtjev.Id);
        
        var noveGreske = await greskaService.PronadiGreskeAsync(zahtjev);
        logger.LogInformation("Pronađeno {Count} grešaka", noveGreske.Count);
        
        await using var context = await contextFactory.CreateDbContextAsync();

        await SinkronizirajGreskeAsync(context, zahtjev.Id, noveGreske);

        zahtjev.RezultatObrade =
            noveGreske.Count != 0 && zahtjev.ManualniRezultatObrade == RezultatObrade.Osnovan
                ? RezultatObrade.Greška
                : zahtjev.ManualniRezultatObrade;

        logger.LogInformation("Rezultat obrade zahtjeva {ZahtjevId}: {Rezultat}", zahtjev.Id, zahtjev.RezultatObrade);
        
        context.Attach(zahtjev);
        var entry = context.Entry(zahtjev);
        entry.Property(z => z.RezultatObrade).IsModified = true;
        entry.Property(z => z.UpdatedAt).IsModified = true;
        entry.Property(z => z.UpdatedBy).IsModified = true;

        await context.SaveChangesAsync();
        auditService.ApplyAudit(zahtjev, false);
    }

    private static async Task SinkronizirajGreskeAsync(
        ApplicationDbContext context,
        long zahtjevId,
        IEnumerable<SocijalniNatjecajBodovnaGreska> nove)
    {
        var set = context.SocijalniNatjecajBodovnaGreske;

        var postojece = await set
            .Where(g => g.ZahtjevId == zahtjevId)
            .ToListAsync();

        set.RemoveRange(postojece);

        await set.AddRangeAsync(nove);
        await context.SaveChangesAsync();
    }
}