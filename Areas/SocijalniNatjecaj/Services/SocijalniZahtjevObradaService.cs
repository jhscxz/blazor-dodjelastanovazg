using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniZahtjevObradaService(
    ApplicationDbContext context,
    ISocijalniKucanstvoService kucanstvoService,
    ISocijalniBodoviService bodoviService,
    ISocijalniBodovniPodaciService bodovniPodaciService,
    ISocijalniClanService clanService,
    IUserContextService currentUserService)
    : ISocijalniZahtjevObradaService
{
    private void ApplyAudit(object entity, bool isCreate)
        => AuditHelper.ApplyAudit(entity, currentUserService.GetCurrentUserId(), isCreate);

    private async Task ApplyAuditAsync(long zahtjevId)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi
            .FirstOrDefaultAsync(x => x.Id == zahtjevId);

        if (zahtjev is not null)
            ApplyAudit(zahtjev, false);
    }

    private async Task ObradiAsync(
        long zahtjevId,
        object? entity,
        bool isCreate = false,
        Func<Task>? dodatnaAkcija = null)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        if (entity is not null)
            ApplyAudit(entity, isCreate);

        await ApplyAuditAsync(zahtjevId);
        await context.SaveChangesAsync();

        if (dodatnaAkcija is not null)
            await dodatnaAkcija();

        await transaction.CommitAsync();
    }

    public async Task SpremiKucanstvoIObracunajAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
    {
        await kucanstvoService.UpdateKucanstvoPodaciAsync(zahtjevId, dto);
        var kucanstvo = await context.SocijalniNatjecajKucanstvoPodaci
            .FirstOrDefaultAsync(x => x.ZahtjevId == zahtjevId);

        await ObradiAsync(zahtjevId, kucanstvo, false, () => bodoviService.IzracunajIBodujAsync(zahtjevId));
    }

    public async Task SpremiBodovnePodatkeIObracunajAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
    {
        await bodovniPodaciService.UpdateAsync(zahtjevId, dto);
        var bodovni = await context.SocijalniNatjecajBodovniPodaci
            .FirstOrDefaultAsync(x => x.ZahtjevId == zahtjevId);

        await ObradiAsync(zahtjevId, bodovni, false, () => bodoviService.IzracunajIBodujAsync(zahtjevId));
    }

    public async Task DodajClanaIObracunajAsync(long zahtjevId, SocijalniNatjecajClanDto clanDto)
    {
        var noviClan = clanDto.ToEntity(zahtjevId);
        await clanService.AddClanAsync(noviClan);

        await ObradiAsync(zahtjevId, noviClan, true, () => bodoviService.IzracunajIBodujAsync(zahtjevId));
    }

    public async Task ObrisiClanaIObracunajAsync(long zahtjevId, long clanId)
    {
        var clan = await context.SocijalniNatjecajClanovi
            .FirstOrDefaultAsync(c => c.Id == clanId && c.ZahtjevId == zahtjevId);

        await clanService.RemoveClanAsync(zahtjevId, clanId);

        await ObradiAsync(zahtjevId, clan, false, () => bodoviService.IzracunajIBodujAsync(zahtjevId));
    }

    public async Task EditClanIObracunajAsync(SocijalniNatjecajClanDto azurirani)
    {
        var clan = await context.SocijalniNatjecajClanovi.FirstAsync(c => c.Id == azurirani.Id);
        azurirani.MapOnto(clan);

        await ObradiAsync(azurirani.ZahtjevId, clan, false,
            () => bodoviService.IzracunajIBodujAsync(azurirani.ZahtjevId));
    }

    public async Task AzurirajOsnovnoIObracunajAkoTrebaAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);
        dto.MapOnto(zahtjev);

        await ObradiAsync(zahtjevId, zahtjev, false, async () =>
        {
            if (dto.RezultatObrade == RezultatObrade.Zadovoljava)
                await bodoviService.IzracunajIBodujAsync(zahtjevId);
        });
    }
}