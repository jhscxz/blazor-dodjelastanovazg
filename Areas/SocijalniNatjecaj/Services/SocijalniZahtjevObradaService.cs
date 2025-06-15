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

    public async Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = dto.Adresa,
            Email = dto.Email,
            RezultatObrade = dto.RezultatObrade!.Value,
            NapomenaObrade = dto.NapomenaObrade
        };

        // Kreiraj povezane entitete
        var podnositelj = new SocijalniNatjecajClan
        {
            ImePrezime = imePrezime!,
            Oib = string.IsNullOrWhiteSpace(oib)
                ? null
                : oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            Zahtjev = null,
        };
        var kucanstvo = new SocijalniNatjecajKucanstvoPodaci();
        var bodovni = new SocijalniNatjecajBodovniPodaci();
        var bodovi = new SocijalniNatjecajBodovi();

        // Sastavi graph
        zahtjev.Clanovi = new List<SocijalniNatjecajClan> { podnositelj };
        zahtjev.KucanstvoPodaci = kucanstvo;
        zahtjev.BodovniPodaci = bodovni;
        zahtjev.Bodovi = bodovi;

        // Dodaj graph u kontekst
        await context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);

        // Primijeni audit na sve entitete nakon što su attachani
        ApplyAudit(zahtjev, true);
        ApplyAudit(podnositelj, true);
        ApplyAudit(kucanstvo, true);
        ApplyAudit(bodovni, true);
        ApplyAudit(bodovi, true);

        await context.SaveChangesAsync();

        // Daljnja logika
        await bodoviService.IzracunajIBodujAsync(zahtjev.Id);
        await ObradiGreskeAsync(zahtjev.Id);

        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        return zahtjev;
    }

    public async Task SpremiKucanstvoIObracunajAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
    {
        await kucanstvoService.UpdateKucanstvoPodaciAsync(zahtjevId, dto);
        var kucanstvo = await context.SocijalniNatjecajKucanstvoPodaci
            .FirstOrDefaultAsync(x => x.ZahtjevId == zahtjevId);

        await ObradiAsync(zahtjevId, kucanstvo, false, async () =>
        {
            await bodoviService.IzracunajIBodujAsync(zahtjevId);
            await ObradiGreskeAsync(zahtjevId);
        });
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
            .Where(x => x.ZahtjevId == zahtjevId)
            .ToListAsync();

        context.SocijalniNatjecajBodovnaGreske.RemoveRange(stare);

        if (greske.Count > 0)
            await context.SocijalniNatjecajBodovnaGreske.AddRangeAsync(greske);
        
        await context.SaveChangesAsync();
    }

    public async Task SpremiBodovnePodatkeIObracunajAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
    {
        await bodovniPodaciService.UpdateAsync(zahtjevId, dto);
        var bodovni = await context.SocijalniNatjecajBodovniPodaci
            .FirstOrDefaultAsync(x => x.ZahtjevId == zahtjevId);

        await ObradiAsync(zahtjevId, bodovni, false, async () =>
        {
            await bodoviService.IzracunajIBodujAsync(zahtjevId);
            await ObradiGreskeAsync(zahtjevId);
        });
    }

    public async Task DodajClanaIObracunajAsync(long zahtjevId, SocijalniNatjecajClanDto clanDto)
    {
        var noviClan = clanDto.ToEntity(zahtjevId);
        await clanService.AddClanAsync(noviClan);

        await ObradiAsync(zahtjevId, noviClan, true, async () =>
        {
            await bodoviService.IzracunajIBodujAsync(zahtjevId);
            await ObradiGreskeAsync(zahtjevId);
        });
    }

    public async Task ObrisiClanaIObracunajAsync(long zahtjevId, long clanId)
    {
        var clan = await context.SocijalniNatjecajClanovi
            .FirstOrDefaultAsync(c => c.Id == clanId && c.ZahtjevId == zahtjevId);

        await clanService.RemoveClanAsync(zahtjevId, clanId);

        await ObradiAsync(zahtjevId, clan, false, async () =>
        {
            await bodoviService.IzracunajIBodujAsync(zahtjevId);
            await ObradiGreskeAsync(zahtjevId);
        });
    }

    public async Task EditClanIObracunajAsync(SocijalniNatjecajClanDto azurirani)
    {
        var clan = await context.SocijalniNatjecajClanovi.FirstAsync(c => c.Id == azurirani.Id);
        azurirani.MapOnto(clan);

        await ObradiAsync(azurirani.ZahtjevId, clan, false, async () =>
        {
            await bodoviService.IzracunajIBodujAsync(azurirani.ZahtjevId);
            await ObradiGreskeAsync(azurirani.ZahtjevId);
        });
    }

    public async Task AzurirajOsnovnoIObracunajAkoTrebaAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);
        dto.MapOnto(zahtjev);

        await ObradiAsync(zahtjevId, zahtjev, false, async () =>
        {
            if (dto.RezultatObrade == RezultatObrade.Zadovoljava)
                await bodoviService.IzracunajIBodujAsync(zahtjevId);

            await ObradiGreskeAsync(zahtjevId);
        });
    }
}