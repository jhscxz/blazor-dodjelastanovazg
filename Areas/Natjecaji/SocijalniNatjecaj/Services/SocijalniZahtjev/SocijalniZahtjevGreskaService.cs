using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
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
    public async Task ObradiGreskeAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        var noveGreske = await greskaService.PronadiGreskeAsync(zahtjev);

        await SinkronizirajGreskeAsync(zahtjev.Id, noveGreske);

        zahtjev.RezultatObrade =
            zahtjev.ManualniRezultatObrade == RezultatObrade.Osnovan && noveGreske.Any()
                ? RezultatObrade.Greška
                : zahtjev.ManualniRezultatObrade;

        auditService.ApplyAudit(zahtjev, false);
    }

    private async Task SinkronizirajGreskeAsync(
        long zahtjevId,
        IEnumerable<SocijalniNatjecajBodovnaGreska> nove)
    {
        var set = context.SocijalniNatjecajBodovnaGreske;

        var postojece = await set
            .Where(g => g.ZahtjevId == zahtjevId)
            .ToListAsync();

        set.RemoveRange(postojece);

        await set.AddRangeAsync(nove);
    }
}