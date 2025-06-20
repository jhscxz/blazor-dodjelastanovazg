using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevProcessor(
    ApplicationDbContext            context,
    ISocijalniZahtjevFactory        factory,
    ISocijalniZahtjevWriteService   writeService,
    ISocijalniBodoviService         bodoviService,
    ISocijalniZahtjevGreskaService  greskaService,
    ISocijalniBodovniPodaciService  bodovniService,
    ISocijalniKucanstvoService      kucanstvoService,
    ISocijalniClanService           clanService)
    : ISocijalniZahtjevProcessor
{
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
    
    private async Task ObradiBodoveIGreskeAsync(SocijalniNatjecajZahtjev z)
    {
        await bodoviService.IzracunajIBodujAsync(z.Id);
        await greskaService.ObradiGreskeAsync(z);
    }

    public async Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(
        SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        var zahtjev = factory.KreirajNovi(dto, imePrezime, oib);

        await ExecuteAsync(async () =>
        {
            await writeService.CreateAsync(zahtjev);
            await ObradiBodoveIGreskeAsync(zahtjev);
        });

        return zahtjev;
    }

    #region osnovno
    public async Task AzurirajOsnovnoIObradiAsync(long id, SocijalniNatjecajOsnovnoEditDto dto)
    {
        await ExecuteAsync(async () =>
        {
            await writeService.UpdateOsnovnoAsync(id, dto);
            var z = await context.SocijalniNatjecajZahtjevi.FirstAsync(x => x.Id == id);

            if (dto.RezultatObrade == RezultatObrade.Osnovan)
                await bodoviService.IzracunajIBodujAsync(id);

            await greskaService.ObradiGreskeAsync(z);
        });
    }
    #endregion
    
    #region kućanstvo
    public async Task SpremiKucanstvoIObradiAsync(long id, SocijalniKucanstvoPodaciDto dto)
    {
        await ExecuteAsync(async () =>
        {
            await kucanstvoService.UpdateKucanstvoPodaciAsync(id, dto);
            var z = await context.SocijalniNatjecajZahtjevi.FirstAsync(x => x.Id == id);
            await ObradiBodoveIGreskeAsync(z);
        });
    }
    #endregion
    
    #region bodovni podaci
    public async Task SpremiBodovnePodatkeIObradiAsync(long id, SocijalniNatjecajBodovniPodaciDto dto)
    {
        await ExecuteAsync(async () =>
        {
            await bodovniService.UpdateAsync(id, dto);
            var z = await context.SocijalniNatjecajZahtjevi.FirstAsync(x => x.Id == id);
            await ObradiBodoveIGreskeAsync(z);
        });
    }
    #endregion
    
    #region članovi
    public async Task DodajClanaIObradiAsync(long id, SocijalniNatjecajClanDto clanDto)
    {
        await ExecuteAsync(async () =>
        {
            var novi = clanDto.ToEntity(id);
            await clanService.AddClanAsync(novi);
            var z = await context.SocijalniNatjecajZahtjevi.FirstAsync(x => x.Id == id);
            await ObradiBodoveIGreskeAsync(z);
        });
    }

    public async Task UrediClanaIObradiAsync(SocijalniNatjecajClanDto dto)
    {
        await ExecuteAsync(async () =>
        {
            var clan = await context.SocijalniNatjecajClanovi.FirstAsync(c => c.Id == dto.Id);
            dto.MapOnto(clan);
            var z = await context.SocijalniNatjecajZahtjevi.FirstAsync(x => x.Id == dto.ZahtjevId);
            await ObradiBodoveIGreskeAsync(z);
        });
    }

    public async Task ObrisiClanaIObradiAsync(long zahtjevId, long clanId)
    {
        await ExecuteAsync(async () =>
        {
            await clanService.RemoveClanAsync(zahtjevId, clanId);
            var z = await context.SocijalniNatjecajZahtjevi.FirstAsync(x => x.Id == zahtjevId);
            await ObradiBodoveIGreskeAsync(z);
        });
    }
    #endregion
}
