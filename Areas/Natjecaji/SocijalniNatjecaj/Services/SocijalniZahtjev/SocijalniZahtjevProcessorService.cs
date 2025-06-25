using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public sealed class SocijalniZahtjevProcessorService(
    IDbContextFactory<ApplicationDbContext> contextFactory,
    ISocijalniZahtjevFactory       factory,
    ISocijalniZahtjevWriteService  writeService,
    ISocijalniBodoviService        bodoviService,
    ISocijalniZahtjevGreskaService greskaService,
    ISocijalniBodovniPodaciService bodovniService,
    ISocijalniKucanstvoService     kucanstvoService,
    ISocijalniClanService          clanService,
    ILogger<SocijalniZahtjevProcessorService> logger)
    : ISocijalniZahtjevProcessorService
{
    #region Infrastructure helpers

    private async Task<SocijalniNatjecajZahtjev> LoadZahtjevAsync(long id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.SocijalniNatjecajZahtjevi
            .Include(z => z.Clanovi)
            .Include(z => z.KucanstvoPodaci)
            .ThenInclude(k => k!.Prihod)
            .Include(z => z.BodovniPodaci)
            .Include(z => z.Natjecaj)
            .FirstAsync(z => z.Id == id);
    }

    private async Task EnsureNatjecajOpenForZahtjevAsync(long zahtjevId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var info = await context.SocijalniNatjecajZahtjevi
            .Where(z => z.Id == zahtjevId)
            .Select(z => new { z.NatjecajId, IsClosed = z.Natjecaj == null || z.Natjecaj.IsClosed })
            .FirstAsync();
        if (info.IsClosed)
        {
            logger.LogWarning("Natječaj {NatjecajId} je zaključen i izmjene nisu moguće", info.NatjecajId);
            throw new InvalidOperationException($"Natječaj {info.NatjecajId} je zaključen i izmjene nisu moguće");
        }
    }

    private async Task EnsureNatjecajOpenAsync(long natjecajId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var isClosed = await context.Natjecaji
            .Where(n => n.Id == natjecajId)
            .Select(n => n.IsClosed)
            .FirstAsync();
        if (isClosed)
        {
            logger.LogWarning("Natječaj {NatjecajId} je zaključen i izmjene nisu moguće", natjecajId);
            throw new InvalidOperationException($"Natječaj {natjecajId} je zaključen i izmjene nisu moguće");
        }
    }
    
    private async Task ObradiBodoveIGreskeAsync(long zahtjevId)
    {
        await bodoviService.IzracunajIBodujAsync(zahtjevId);

        var zahtjev = await LoadZahtjevAsync(zahtjevId);
        await greskaService.ObradiGreskeAsync(zahtjev);
    }

    #endregion

    #region Kreiranje zahtjeva

    public async Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(
        SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        logger.LogInformation("Kreiranje zahtjeva za natječaj {NatjecajId}", dto.NatjecajId);
        
        await EnsureNatjecajOpenAsync(dto.NatjecajId);
        
        var zahtjev = factory.KreirajNovi(dto, imePrezime, oib);

        await writeService.CreateAsync(zahtjev);
        await ObradiBodoveIGreskeAsync(zahtjev.Id);

        logger.LogInformation("Zahtjev {ZahtjevId} kreiran", zahtjev.Id);
        
        return zahtjev;
    }

    #endregion

    #region Osnovno
    public async Task AzurirajOsnovnoIObradiAsync(long id, SocijalniNatjecajOsnovnoEditDto dto)
    {
        logger.LogInformation("Ažuriranje osnovnih podataka zahtjeva {Id}", id);
        
        await using var ctx = await contextFactory.CreateDbContextAsync();
        
        await EnsureNatjecajOpenForZahtjevAsync(id);
        
        await writeService.UpdateOsnovnoAsync(id, dto);

        if (dto.RezultatObrade == RezultatObrade.Osnovan)
            await bodoviService.IzracunajIBodujAsync(id);

        await ObradiBodoveIGreskeAsync(id);
        
        logger.LogInformation("Ažurirani osnovni podaci zahtjeva {Id}", id);
    }
    #endregion

    #region Kućanstvo
    public async Task SpremiKucanstvoIObradiAsync(long id, SocijalniKucanstvoPodaciDto dto)
    {
        logger.LogInformation("Spremanje kućanstva za zahtjev {Id}", id);
        
        await EnsureNatjecajOpenForZahtjevAsync(id);
        
        await kucanstvoService.UpdateKucanstvoPodaciAsync(id, dto);
        await ObradiBodoveIGreskeAsync(id);
        
        logger.LogInformation("Spremljeno kućanstvo za zahtjev {Id}", id);
    }
    #endregion

    #region Bodovni podaci
    public async Task SpremiBodovnePodatkeIObradiAsync(long id, SocijalniNatjecajBodovniPodaciDto dto)
    {
        logger.LogInformation("Spremanje bodovnih podataka za zahtjev {Id}", id);
        
        await EnsureNatjecajOpenForZahtjevAsync(id);
        
        await bodovniService.UpdateAsync(id, dto);
        await ObradiBodoveIGreskeAsync(id);
        
        logger.LogInformation("Spremljeni bodovni podaci za zahtjev {Id}", id);
    }
    #endregion

    #region Članovi
    public async Task DodajClanaIObradiAsync(long id, SocijalniNatjecajClanDto clanDto)
    {
        logger.LogInformation("Dodavanje člana za zahtjev {Id}", id);
        
        await EnsureNatjecajOpenForZahtjevAsync(id);
        
        await clanService.AddClanAsync(clanDto.ToEntity(id));
        await ObradiBodoveIGreskeAsync(id);
        
        logger.LogInformation("Dodani član za zahtjev {Id}", id);
    }

    public async Task UrediClanaIObradiAsync(SocijalniNatjecajClanDto dto)
    {
        logger.LogInformation("Uređivanje člana {ClanId} za zahtjev {ZahtjevId}", dto.Id, dto.ZahtjevId);
        
        await EnsureNatjecajOpenForZahtjevAsync(dto.ZahtjevId);
        
        await using (var context = contextFactory.CreateDbContext())
        {
            var clan = await context.SocijalniNatjecajClanovi.FirstAsync(c => c.Id == dto.Id);
            dto.MapOnto(clan);
            await context.SaveChangesAsync();
        }
        await ObradiBodoveIGreskeAsync(dto.ZahtjevId);
        
        logger.LogInformation("Uređen član {ClanId} za zahtjev {ZahtjevId}", dto.Id, dto.ZahtjevId);
    }

    public async Task ObrisiClanaIObradiAsync(long zahtjevId, long clanId)
    {
        logger.LogInformation("Brisanje člana {ClanId} sa zahtjeva {ZahtjevId}", clanId, zahtjevId);
        
        await EnsureNatjecajOpenForZahtjevAsync(zahtjevId);
        
        await clanService.RemoveClanAsync(zahtjevId, clanId);
        await ObradiBodoveIGreskeAsync(zahtjevId);
        
        logger.LogInformation("Obrisan član {ClanId} sa zahtjeva {ZahtjevId}", clanId, zahtjevId);
    }
    
    public async Task ObradiSveZahtjeveAsync(long natjecajId)
    {
        logger.LogInformation("Obrada svih zahtjeva za natječaj {NatjecajId}", natjecajId);

        await using var context = contextFactory.CreateDbContext();
        var zahtjevi = await context.SocijalniNatjecajZahtjevi
            .Where(z => z.NatjecajId == natjecajId)
            .Select(z => z.Id)
            .ToListAsync();

        foreach (var id in zahtjevi)
        {
            await bodoviService.IzracunajIBodujAsync(id);
            var zahtjev = await LoadZahtjevAsync(id);
            await greskaService.ObradiGreskeAsync(zahtjev);
        }
    }
    #endregion
}