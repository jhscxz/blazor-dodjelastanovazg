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
        var greske = await greskaService.PronadiGreskeAsync(zahtjev);

        var stare = await context.SocijalniNatjecajBodovnaGreske
            .Where(g => g.ZahtjevId == zahtjev.Id)
            .ToListAsync();

        context.SocijalniNatjecajBodovnaGreske.RemoveRange(stare);

        if (greske.Any())
        {
            await context.SocijalniNatjecajBodovnaGreske.AddRangeAsync(greske);
            zahtjev.RezultatObrade = RezultatObrade.Greška;
        }
        else
        {
            zahtjev.RezultatObrade = zahtjev.ManualniRezultatObrade;
        }
        
        
        _auditService.ApplyAudit(zahtjev, false);
    }
}