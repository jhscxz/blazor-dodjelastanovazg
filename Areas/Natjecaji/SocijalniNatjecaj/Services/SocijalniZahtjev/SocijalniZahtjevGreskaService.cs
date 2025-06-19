using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers.IServices;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevGreskaService(
    ApplicationDbContext context,
    ISocijalniBodovnaGreskaService greskaService,
    IAuditService auditService)
    : ISocijalniZahtjevGreskaService
{
    
    private readonly IAuditService _auditService = auditService;
    public async Task ObradiGreskeAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        var manualniOsnovan = zahtjev.ManualniRezultatObrade == RezultatObrade.Osnovan;

        var noveGreske = await greskaService.PronadiGreskeAsync(zahtjev);

        var stareGreske = await context.SocijalniNatjecajBodovnaGreske
            .Where(g => g.ZahtjevId == zahtjev.Id)
            .ToListAsync();

        context.SocijalniNatjecajBodovnaGreske.RemoveRange(stareGreske);

        if (noveGreske.Any())
            await context.SocijalniNatjecajBodovnaGreske.AddRangeAsync(noveGreske);

        zahtjev.RezultatObrade = manualniOsnovan && noveGreske.Any()
            ? RezultatObrade.Greška
            : zahtjev.ManualniRezultatObrade;

        _auditService.ApplyAudit(zahtjev, false);
    }
}