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
    IUserContextService currentUserService)
    : ISocijalniZahtjevObradaService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ISocijalniKucanstvoService _kucanstvoService = kucanstvoService;
    private readonly ISocijalniBodoviService _bodoviService = bodoviService;
    private readonly ISocijalniBodovniPodaciService _bodovniPodaciService = bodovniPodaciService;
    private readonly ISocijalniClanService _clanService = clanService;
    private readonly IUserContextService _currentUserService = currentUserService;

    private async Task ApplyAuditAsync(long zahtjevId)
    {
        var zahtjev = await _context.SocijalniNatjecajZahtjevi.FirstOrDefaultAsync(x => x.Id == zahtjevId);
        if (zahtjev is not null)
        {
            AuditHelper.ApplyAudit(zahtjev, _currentUserService.GetCurrentUserId(), isCreate: false);
        }
    }

    public async Task SpremiKucanstvoIObracunajAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _kucanstvoService.UpdateKucanstvoPodaciAsync(zahtjevId, dto);
        await ApplyAuditAsync(zahtjevId);
        await _context.SaveChangesAsync();

        await _bodoviService.IzracunajIBodujAsync(zahtjevId);
        await transaction.CommitAsync();
    }

    public async Task SpremiBodovnePodatkeIObracunajAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _bodovniPodaciService.UpdateAsync(zahtjevId, dto);
        await ApplyAuditAsync(zahtjevId);
        await _context.SaveChangesAsync();

        await _bodoviService.IzracunajIBodujAsync(zahtjevId);
        await transaction.CommitAsync();
    }

    public async Task DodajClanaIObracunajAsync(long zahtjevId, SocijalniNatjecajClanDto clanDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var noviClan = new SocijalniNatjecajClan
        {
            ZahtjevId = zahtjevId,
            ImePrezime = clanDto.ImePrezime,
            Oib = clanDto.Oib,
            DatumRodjenja = clanDto.DatumRodjenja,
            Srodstvo = clanDto.Srodstvo,
            Zahtjev = null!
        };

        await _clanService.AddClanAsync(noviClan);
        await ApplyAuditAsync(zahtjevId);
        await _context.SaveChangesAsync();

        await _bodoviService.IzracunajIBodujAsync(zahtjevId);
        await transaction.CommitAsync();
    }

    public async Task ObrisiClanaIObracunajAsync(long zahtjevId, long clanId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _clanService.RemoveClanAsync(zahtjevId, clanId);
        await ApplyAuditAsync(zahtjevId);
        await _context.SaveChangesAsync();

        await _bodoviService.IzracunajIBodujAsync(zahtjevId);
        await transaction.CommitAsync();
    }

    public async Task EditClanIObracunajAsync(SocijalniNatjecajClanDto azurirani)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        await _clanService.EditClanAsync(azurirani);
        await ApplyAuditAsync(azurirani.ZahtjevId);
        await _context.SaveChangesAsync();

        await _bodoviService.IzracunajIBodujAsync(azurirani.ZahtjevId);
        await transaction.CommitAsync();
    }

    public async Task AzurirajOsnovnoIObracunajAkoTrebaAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var zahtjev = await _context.SocijalniNatjecajZahtjevi.FirstAsync(z => z.Id == zahtjevId);

        zahtjev.KlasaPredmeta = dto.KlasaPredmeta ?? 0;
        zahtjev.DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? default;
        zahtjev.Adresa = dto.Adresa;
        zahtjev.RezultatObrade = dto.RezultatObrade ?? 0;
        zahtjev.NapomenaObrade = dto.NapomenaObrade;
        zahtjev.Email = dto.Email;

        //AuditHelper.ApplyAudit(zahtjev, _currentUserService.GetCurrentUserId(), isCreate: false);
        await ApplyAuditAsync(zahtjev.Id);
        await _context.SaveChangesAsync();

        if (dto.RezultatObrade == RezultatObrade.Zadovoljava)
        {
            await _bodoviService.IzracunajIBodujAsync(zahtjevId);
        }

        await transaction.CommitAsync();
    }
}
