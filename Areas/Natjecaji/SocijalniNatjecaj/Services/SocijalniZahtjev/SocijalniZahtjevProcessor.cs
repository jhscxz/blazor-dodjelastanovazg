using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IServices;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevProcessor(
    ApplicationDbContext context,
    ISocijalniZahtjevFactory factory,
    ISocijalniZahtjevWriteService writeService,
    ISocijalniBodoviService bodoviService,
    ISocijalniZahtjevGreskaService greskaProcessor,
    ISocijalniBodovniPodaciService bodovniPodaciService,
    ISocijalniKucanstvoService kucanstvoService,
    ISocijalniClanService clanService,
    IAuditService auditService)
    : ISocijalniZahtjevProcessor
{
    private readonly ApplicationDbContext _context = context;
    private readonly ISocijalniZahtjevFactory _factory = factory;
    private readonly ISocijalniZahtjevWriteService _writeService = writeService;
    private readonly ISocijalniBodoviService _bodoviService = bodoviService;
    private readonly ISocijalniZahtjevGreskaService _greskaProcessor = greskaProcessor;
    private readonly ISocijalniBodovniPodaciService _bodovniPodaciService = bodovniPodaciService;
    private readonly ISocijalniKucanstvoService _kucanstvoService = kucanstvoService;
    private readonly ISocijalniClanService _clanService = clanService;
    private readonly IAuditService _auditService = auditService;

    private async Task ObradiBodoveIGreskeAsync(SocijalniNatjecajZahtjev zahtjev)
    {
        await _bodoviService.IzracunajIBodujAsync(zahtjev.Id);
        await _greskaProcessor.ObradiGreskeAsync(zahtjev);
    }

    private async Task TransakcijskiObradiAsync(SocijalniNatjecajZahtjev zahtjev, object? entity = null,
        bool isCreate = false, Func<Task>? akcija = null)
    {
        await using var tx = await _context.Database.BeginTransactionAsync();

        if (entity != null)
            _auditService.ApplyAudit(entity, isCreate);

        _auditService.ApplyAudit(zahtjev, false);

        if (akcija != null)
            await akcija();

        await _context.SaveChangesAsync();
        await tx.CommitAsync();
    }

    public async Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime,
        string? oib)
    {
        var zahtjev = _factory.KreirajNovi(dto, imePrezime, oib);

        await TransakcijskiObradiAsync(zahtjev, zahtjev, true, async () =>
        {
            await _writeService.CreateAsync(zahtjev);
            await ObradiBodoveIGreskeAsync(zahtjev);
        });

        return zahtjev;
    }

    public async Task AzurirajOsnovnoIObradiAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        await _writeService.UpdateOsnovnoAsync(zahtjevId, dto);
        var zahtjev = await _context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);

        await TransakcijskiObradiAsync(zahtjev, zahtjev, false, async () =>
        {
            if (dto.RezultatObrade == RezultatObrade.Osnovan)
                await _bodoviService.IzracunajIBodujAsync(zahtjev.Id);

            await _greskaProcessor.ObradiGreskeAsync(zahtjev);
        });
    }

    public async Task SpremiKucanstvoIObradiAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
    {
        await _kucanstvoService.UpdateKucanstvoPodaciAsync(zahtjevId, dto);

        var kuc = await _context.SocijalniNatjecajKucanstvoPodaci
            .Include(k => k.Prihod)
            .FirstAsync(k => k.ZahtjevId == zahtjevId);

        var zahtjev = await _context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);
        _context.Entry(kuc.Prihod!).State = EntityState.Modified;

        await TransakcijskiObradiAsync(zahtjev, kuc, false, () => ObradiBodoveIGreskeAsync(zahtjev));
    }

    public async Task SpremiBodovnePodatkeIObradiAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
    {
        await _bodovniPodaciService.UpdateAsync(zahtjevId, dto);
        var bodovni = await _context.SocijalniNatjecajBodovniPodaci.FirstAsync(x => x.ZahtjevId == zahtjevId);
        var zahtjev = await _context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);

        await TransakcijskiObradiAsync(zahtjev, bodovni, false, () => ObradiBodoveIGreskeAsync(zahtjev));
    }

    public async Task DodajClanaIObradiAsync(long zahtjevId, SocijalniNatjecajClanDto clanDto)
    {
        var novi = clanDto.ToEntity(zahtjevId);
        await _clanService.AddClanAsync(novi);
        var zahtjev = await _context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);

        await TransakcijskiObradiAsync(zahtjev, novi, true, () => ObradiBodoveIGreskeAsync(zahtjev));
    }

    public async Task UrediClanaIObradiAsync(SocijalniNatjecajClanDto clanDto)
    {
        var clan = await _context.SocijalniNatjecajClanovi.FirstAsync(c => c.Id == clanDto.Id);
        clanDto.MapOnto(clan);

        var zahtjev = await _context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == clanDto.ZahtjevId);

        await TransakcijskiObradiAsync(zahtjev, clan, false, () => ObradiBodoveIGreskeAsync(zahtjev));
    }

    public async Task ObrisiClanaIObradiAsync(long zahtjevId, long clanId)
    {
        await _clanService.RemoveClanAsync(zahtjevId, clanId);
        var zahtjev = await _context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);

        await TransakcijskiObradiAsync(zahtjev, null, false, () => ObradiBodoveIGreskeAsync(zahtjev));
    }
}
