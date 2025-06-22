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
        
        await SinkronizirajGreskeAsync(zahtjev.Id, noveGreske);

        zahtjev.RezultatObrade =
            zahtjev.ManualniRezultatObrade == RezultatObrade.Osnovan && noveGreske.Any()
                ? RezultatObrade.Greška
                : zahtjev.ManualniRezultatObrade;

        logger.LogInformation("Rezultat obrade zahtjeva {ZahtjevId}: {Rezultat}", zahtjev.Id, zahtjev.RezultatObrade);
        
        auditService.ApplyAudit(zahtjev, false);
    }

    private async Task SinkronizirajGreskeAsync(
        long zahtjevId,
        IEnumerable<SocijalniNatjecajBodovnaGreska> nove)
    {
        await using var context = contextFactory.CreateDbContext();
        var set = context.SocijalniNatjecajBodovnaGreske;

        var postojece = await set
            .Where(g => g.ZahtjevId == zahtjevId)
            .ToListAsync();

        set.RemoveRange(postojece);

        await set.AddRangeAsync(nove);
        await context.SaveChangesAsync();
    }
}