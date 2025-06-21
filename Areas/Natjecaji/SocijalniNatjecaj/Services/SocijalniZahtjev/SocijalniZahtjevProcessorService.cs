using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

/// <summary>
/// Centralni “processor” koji rukuje svim zapisima nad socijalnim natječajem
/// te nakon svake izmjene ponovno računa bodove i sinkronizira bodovne greške.
/// </summary>
/// <remarks>
/// <list type="number">
///   <item>Jedan <c>LoadZahtjevAsync</c> helper učitava sve potrebne navigacije.</item>
///   <item><c>ObradiBodoveIGreskeAsync</c> prima samo <c>id</c> – unutar sebe učitava entitet.</item>
///   <item>Eliminirano je ponavljanje istih <c>.Include(...)</c> i <c>SaveChangesAsync()</c> poziva.</item>
/// </list>
/// </remarks>
public sealed class SocijalniZahtjevProcessorService(
    ApplicationDbContext           context,
    ISocijalniZahtjevFactory       factory,
    ISocijalniZahtjevWriteService  writeService,
    ISocijalniBodoviService        bodoviService,
    ISocijalniZahtjevGreskaService greskaService,
    ISocijalniBodovniPodaciService bodovniService,
    ISocijalniKucanstvoService     kucanstvoService,
    ISocijalniClanService          clanService)
    : ISocijalniZahtjevProcessorService
{
    #region Infrastructure helpers

    private async Task ExecuteAsync(Func<Task> work)
    {
        await using var tx = await context.Database.BeginTransactionAsync();
        try
        {
            await work();
            await context.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            throw new InvalidOperationException("Netko je u međuvremenu promijenio podatke. Osvježite stranicu pa pokušajte ponovo.");
        }
    }

    private Task<SocijalniNatjecajZahtjev> LoadZahtjevAsync(long id) =>
        context.SocijalniNatjecajZahtjevi
               .Include(z => z.Clanovi)
               .Include(z => z.KucanstvoPodaci)
               .Include(z => z.BodovniPodaci)
               .Include(z => z.Natjecaj)
               .FirstAsync(z => z.Id == id);

    private async Task ObradiBodoveIGreskeAsync(long zahtjevId)
    {
        // Bodovi računaju direktno po ID‑ju
        await bodoviService.IzracunajIBodujAsync(zahtjevId);

        // Greške trebaju kompletan entitet
        var zahtjev = await LoadZahtjevAsync(zahtjevId);
        await greskaService.ObradiGreskeAsync(zahtjev);
    }

    #endregion

    #region Kreiranje zahtjeva

    public async Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(
        SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        var zahtjev = factory.KreirajNovi(dto, imePrezime, oib);

        await ExecuteAsync(async () =>
        {
            await writeService.CreateAsync(zahtjev);
            await ObradiBodoveIGreskeAsync(zahtjev.Id);
        });

        return zahtjev;
    }

    #endregion

    #region Osnovno
    public async Task AzurirajOsnovnoIObradiAsync(long id, SocijalniNatjecajOsnovnoEditDto dto)
    {
        await ExecuteAsync(async () =>
        {
            await writeService.UpdateOsnovnoAsync(id, dto);

            if (dto.RezultatObrade == RezultatObrade.Osnovan)
                await bodoviService.IzracunajIBodujAsync(id);

            await ObradiBodoveIGreskeAsync(id);
        });
    }
    #endregion

    #region Kućanstvo
    public async Task SpremiKucanstvoIObradiAsync(long id, SocijalniKucanstvoPodaciDto dto)
    {
        await ExecuteAsync(async () =>
        {
            await kucanstvoService.UpdateKucanstvoPodaciAsync(id, dto);
            await ObradiBodoveIGreskeAsync(id);
        });
    }
    #endregion

    #region Bodovni podaci
    public async Task SpremiBodovnePodatkeIObradiAsync(long id, SocijalniNatjecajBodovniPodaciDto dto)
    {
        await ExecuteAsync(async () =>
        {
            await bodovniService.UpdateAsync(id, dto);
            await ObradiBodoveIGreskeAsync(id);
        });
    }
    #endregion

    #region Članovi
    public async Task DodajClanaIObradiAsync(long id, SocijalniNatjecajClanDto clanDto)
    {
        await ExecuteAsync(async () =>
        {
            await clanService.AddClanAsync(clanDto.ToEntity(id));
            await ObradiBodoveIGreskeAsync(id);
        });
    }

    public async Task UrediClanaIObradiAsync(SocijalniNatjecajClanDto dto)
    {
        await ExecuteAsync(async () =>
        {
            var clan = await context.SocijalniNatjecajClanovi.FirstAsync(c => c.Id == dto.Id);
            dto.MapOnto(clan);
            await ObradiBodoveIGreskeAsync(dto.ZahtjevId);
        });
    }

    public async Task ObrisiClanaIObradiAsync(long zahtjevId, long clanId)
    {
        await ExecuteAsync(async () =>
        {
            await clanService.RemoveClanAsync(zahtjevId, clanId);
            await ObradiBodoveIGreskeAsync(zahtjevId);
        });
    }
    #endregion
}