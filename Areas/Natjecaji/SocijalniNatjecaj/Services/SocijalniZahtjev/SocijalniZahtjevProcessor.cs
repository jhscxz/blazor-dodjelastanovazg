using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IServices;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevProcessor(
    ApplicationDbContext context,
    ISocijalniZahtjevFactory factory,
    ISocijalniZahtjevWriteService writeService,
    ISocijalniBodoviService bodoviService,
    ISocijalniZahtjevGreskaService greskaService,
    ISocijalniBodovniPodaciService bodovniPodaciService,
    ISocijalniKucanstvoService kucanstvoService,
    ISocijalniClanService clanService,
    IAuditService auditService)
    : ISocijalniZahtjevProcessor
{
    private async Task ObradiBodoveIGreskeAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        await bodoviService.IzracunajIBodujAsync(zahtjev.Id);
        await greskaService.ObradiGreskeAsync(zahtjev);
    }

    public async Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(
        SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        var zahtjev = factory.KreirajNovi(dto, imePrezime, oib);
        await writeService.CreateAsync(zahtjev);
        await ObradiBodoveIGreskeAsync(zahtjev);
        await context.SaveChangesAsync();
        return zahtjev;
    }

    public async Task AzurirajOsnovnoIObradiAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        await writeService.UpdateOsnovnoAsync(zahtjevId, dto);
        var zahtjev = await context.SocijalniNatjecajZahtjevi.FindAsync(zahtjevId);
        if (zahtjev == null) throw new Exception($"Zahtjev {zahtjevId} nije pronađen.");

        if (dto.RezultatObrade == RezultatObrade.Osnovan)
            await bodoviService.IzracunajIBodujAsync(zahtjev.Id);

        await greskaService.ObradiGreskeAsync(zahtjev);
        await context.SaveChangesAsync();
    }

    public async Task SpremiKucanstvoIObradiAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
    {
        await kucanstvoService.UpdateKucanstvoPodaciAsync(zahtjevId, dto);

        var zahtjev = await context.SocijalniNatjecajZahtjevi.FindAsync(zahtjevId)
                      ?? throw new Exception($"Zahtjev {zahtjevId} nije pronađen.");

        await ObradiBodoveIGreskeAsync(zahtjev);
        await context.SaveChangesAsync();
    }

    public async Task SpremiBodovnePodatkeIObradiAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
    {
        await bodovniPodaciService.UpdateAsync(zahtjevId, dto);

        var zahtjev = await context.SocijalniNatjecajZahtjevi.FindAsync(zahtjevId)
                      ?? throw new Exception($"Zahtjev {zahtjevId} nije pronađen.");

        await ObradiBodoveIGreskeAsync(zahtjev);
        await context.SaveChangesAsync();
    }

    public async Task DodajClanaIObradiAsync(long zahtjevId, SocijalniNatjecajClanDto clanDto)
    {
        var novi = clanDto.ToEntity(zahtjevId);
        auditService.ApplyAudit(novi, true);
        await clanService.AddClanAsync(novi);

        var zahtjev = await context.SocijalniNatjecajZahtjevi.FindAsync(zahtjevId)
                      ?? throw new Exception($"Zahtjev {zahtjevId} nije pronađen.");

        await ObradiBodoveIGreskeAsync(zahtjev);
        await context.SaveChangesAsync();
    }

    public async Task UrediClanaIObradiAsync(SocijalniNatjecajClanDto clanDto)
    {
        var clan = await context.SocijalniNatjecajClanovi.FindAsync(clanDto.Id)
                   ?? throw new Exception($"Član {clanDto.Id} nije pronađen.");

        clanDto.MapOnto(clan);
        auditService.ApplyAudit(clan, false);

        var zahtjev = await context.SocijalniNatjecajZahtjevi.FindAsync(clanDto.ZahtjevId)
                      ?? throw new Exception($"Zahtjev {clanDto.ZahtjevId} nije pronađen.");

        await ObradiBodoveIGreskeAsync(zahtjev);
        await context.SaveChangesAsync();
    }

    public async Task ObrisiClanaIObradiAsync(long zahtjevId, long clanId)
    {
        await clanService.RemoveClanAsync(zahtjevId, clanId);

        var zahtjev = await context.SocijalniNatjecajZahtjevi.FindAsync(zahtjevId)
                      ?? throw new Exception($"Zahtjev {zahtjevId} nije pronađen.");

        await ObradiBodoveIGreskeAsync(zahtjev);
        await context.SaveChangesAsync();
    }
}