using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniZahtjevObradaService(
    ApplicationDbContext context,
    ISocijalniKucanstvoService kucanstvoService,
    ISocijalniBodoviService bodoviService,
    ISocijalniBodovniPodaciService bodovniPodaciService,
    ISocijalniClanService clanService,
    ISocijalniBodovnaGreskaService greskaService,
    IUserContextService currentUserService,
    ISocijalniZahtjevFactory factory)
    : ISocijalniZahtjevObradaService
{
    private void ApplyAudit(object entity, bool isCreate) =>
        AuditHelper.ApplyAudit(entity, currentUserService.GetCurrentUserId(), isCreate);

    private void ApplyAuditAllNew(params object[] entities)
    {
        foreach (var entity in entities)
            ApplyAudit(entity, true);
    }

    private async Task ApplyAuditAsync(long zahtjevId)
    {
        var z = await context.SocijalniNatjecajZahtjevi.FindAsync(zahtjevId);
        if (z != null) ApplyAudit(z, false);
    }

    private async Task TransakcijskiObradiAsync(
        long zahtjevId,
        object? entity,
        bool isCreate = false,
        Func<Task>? dodatnaAkcija = null)
    {
        await using var tx = await context.Database.BeginTransactionAsync();

        if (entity != null)
            ApplyAudit(entity, isCreate);

        await ApplyAuditAsync(zahtjevId);
        await context.SaveChangesAsync();

        if (dodatnaAkcija != null)
            await dodatnaAkcija();

        await tx.CommitAsync();
    }

    private async Task ObradiIBodujAsync(long zahtjevId)
    {
        await bodoviService.IzracunajIBodujAsync(zahtjevId);
        await ObradiGreskeAsync(zahtjevId);
    }

    public async Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        await using var tx = await context.Database.BeginTransactionAsync();

        var zahtjev = factory.KreirajNovi(dto, imePrezime, oib);
        var prihod = factory.KreirajPrazanPrihod(zahtjev.KucanstvoPodaci!.Id);

        ApplyAudit(zahtjev, true);
        ApplyAuditAllNew(zahtjev.Clanovi.ToArray<object>());
        ApplyAuditAllNew(zahtjev.KucanstvoPodaci!, zahtjev.BodovniPodaci!, zahtjev.Bodovi!, prihod);

        await context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);
        await context.SocijalniPrihodi.AddAsync(prihod);
        await context.SaveChangesAsync();

        await ObradiIBodujAsync(zahtjev.Id);

        await context.SaveChangesAsync();
        await tx.CommitAsync();

        return zahtjev;
    }

    public async Task SpremiKucanstvoIObracunajAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
    {
        await kucanstvoService.UpdateKucanstvoPodaciAsync(zahtjevId, dto);

        var kuc = await context.SocijalniNatjecajKucanstvoPodaci
            .Include(k => k.Prihod)
            .FirstAsync(k => k.ZahtjevId == zahtjevId);

        context.Entry((object)kuc.Prihod).State = EntityState.Modified;

        await TransakcijskiObradiAsync(zahtjevId, kuc, false, () => ObradiIBodujAsync(zahtjevId));
    }

    public async Task ObradiGreskeAsync(long zahtjevId)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi
            .Include(z => z.KucanstvoPodaci)
            .Include(z => z.Clanovi)
            .Include(z => z.BodovniPodaci)
            .FirstAsync(z => z.Id == zahtjevId);

        var greske = await greskaService.PronadiGreskeAsync(zahtjev);

        var stare = await context.SocijalniNatjecajBodovnaGreske
            .Where(g => g.ZahtjevId == zahtjevId).ToListAsync();

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

        ApplyAudit(zahtjev, false);
        await context.SaveChangesAsync();
    }

    public async Task SpremiBodovnePodatkeIObracunajAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
    {
        await bodovniPodaciService.UpdateAsync(zahtjevId, dto);

        var bodovni = await context.SocijalniNatjecajBodovniPodaci
            .FirstAsync(x => x.ZahtjevId == zahtjevId);

        await TransakcijskiObradiAsync(zahtjevId, bodovni, false, () => ObradiIBodujAsync(zahtjevId));
    }

    public async Task DodajClanaIObracunajAsync(long zahtjevId, SocijalniNatjecajClanDto clanDto)
    {
        var novi = clanDto.ToEntity(zahtjevId);
        await clanService.AddClanAsync(novi);

        await TransakcijskiObradiAsync(zahtjevId, novi, true, () => ObradiIBodujAsync(zahtjevId));
    }

    public async Task ObrisiClanaIObracunajAsync(long zahtjevId, long clanId)
    {
        await clanService.RemoveClanAsync(zahtjevId, clanId);

        await TransakcijskiObradiAsync(zahtjevId, null, false, () => ObradiIBodujAsync(zahtjevId));
    }

    public async Task EditClanIObracunajAsync(SocijalniNatjecajClanDto azurirani)
    {
        var clan = await context.SocijalniNatjecajClanovi.FirstAsync(c => c.Id == azurirani.Id);
        azurirani.MapOnto(clan);

        await TransakcijskiObradiAsync(azurirani.ZahtjevId, clan, false, () => ObradiIBodujAsync(azurirani.ZahtjevId));
    }

    public async Task AzurirajOsnovnoIObracunajAkoTrebaAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);

        dto.MapOnto(zahtjev);
        zahtjev.ManualniRezultatObrade = dto.RezultatObrade!.Value;

        await TransakcijskiObradiAsync(
            zahtjevId,
            zahtjev,
            false,
            async () =>
            {
                if (dto.RezultatObrade == RezultatObrade.Osnovan)
                    await bodoviService.IzracunajIBodujAsync(zahtjevId);

                await ObradiGreskeAsync(zahtjevId);
            });
    }
}
