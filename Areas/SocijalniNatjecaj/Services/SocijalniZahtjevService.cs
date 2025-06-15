using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class NotFoundException(string message) : Exception(message);

public class SocijalniZahtjevService(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor,
    ISocijalniBodovnaGreskaService greskaService)
    : ISocijalniZahtjevService
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    private readonly ISocijalniBodovnaGreskaService _greskaService =
        greskaService ?? throw new ArgumentNullException(nameof(greskaService));

    private string CurrentUserId => _httpContextAccessor.HttpContext?.User
                                        ?.FindFirstValue(ClaimTypes.NameIdentifier)
                                    ?? throw new InvalidOperationException("Korisnik nije prijavljen.");

    private IQueryable<SocijalniNatjecajZahtjev> BaseQuery(bool asNoTracking = false)
    {
        var query = _context.SocijalniNatjecajZahtjevi
            .Include(z => z.Clanovi)
            .ThenInclude(c => c.CreatedByUser)
            .Include(z => z.Clanovi)
            .ThenInclude(c => c.UpdatedByUser)
            .Include(z => z.BodovniPodaci)
            .Include(z => z.KucanstvoPodaci)
            .ThenInclude(k => k!.Prihod)
            .ThenInclude(p => p!.CreatedByUser)
            .Include(z => z.KucanstvoPodaci)
            .ThenInclude(k => k!.Prihod)
            .ThenInclude(p => p!.UpdatedByUser)
            .Include(z => z.KucanstvoPodaci)
            .ThenInclude(k => k!.UpdatedByUser)
            .Include(z => z.CreatedByUser)
            .Include(z => z.UpdatedByUser);

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id)
    {
        var entity = await BaseQuery(asNoTracking: true)
                         .FirstOrDefaultAsync(x => x.Id == id)
                     ?? throw new NotFoundException($"Zahtjev s ID-om {id} nije pronađen.");

        return entity.ToDto();
    }

    public Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync() =>
        BaseQuery(asNoTracking: true)
            .Select(x => new SocijalniNatjecajZahtjevDto
            {
                Id = x.Id,
                KlasaPredmeta = x.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
                Adresa = x.Adresa!,
                NatjecajId = x.NatjecajId
            })
            .ToListAsync();

    public async Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime,
        string? oib)
    {
        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = dto.Adresa,
            Email = dto.Email,
            RezultatObrade = dto.RezultatObrade!.Value,
            ManualniRezultatObrade = dto.RezultatObrade!.Value,
            NapomenaObrade = dto.NapomenaObrade
        };

        var podnositelj = new SocijalniNatjecajClan
        {
            ImePrezime = imePrezime!,
            Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            Zahtjev = zahtjev
        };

        var kucanstvo = new SocijalniNatjecajKucanstvoPodaci
        {
            Zahtjev = zahtjev
        };

        var bodovni = new SocijalniNatjecajBodovniPodaci { Zahtjev = zahtjev };
        var bodovi = new SocijalniNatjecajBodovi { Zahtjev = zahtjev };

        // Generiraj greške
        var greske = await _greskaService.PronadiGreskeAsync(zahtjev);
        foreach (var g in greske)
        {
            AuditHelper.ApplyAudit(g, CurrentUserId, isCreate: true);
        }

        // Audit svih entiteta osim prihoda
        AuditHelper.ApplyAudit(
            new object[] { zahtjev, podnositelj, kucanstvo, bodovni, bodovi },
            CurrentUserId,
            isCreate: true);

        zahtjev.Clanovi = new List<SocijalniNatjecajClan> { podnositelj };
        zahtjev.KucanstvoPodaci = kucanstvo;
        zahtjev.BodovniPodaci = bodovni;
        zahtjev.Bodovi = bodovi;
        zahtjev.Greske = greske;

        // 1. Spremi zahtjev i sve vezane entitete (uključuje kucanstvo)
        await _context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);
        await _context.SaveChangesAsync();

        // 2. Dohvati ID kućanstva iz baze (jer Id još nije bio generiran prije)
        var kucanstvoId = zahtjev.KucanstvoPodaci.Id;

        // 3. Kreiraj prihod s točnim ID-om
        var prihod = new SocijalniPrihodi
        {
            Id = kucanstvoId, // shared primary key
            UkupniPrihodKucanstva = 0,
            PrihodPoClanu = 0,
            IspunjavaUvjetPrihoda = true
        };

        AuditHelper.ApplyAudit(prihod, CurrentUserId, isCreate: true);

        // 4. Spremi prihod
        await _context.SocijalniPrihodi.AddAsync(prihod);
        await _context.SaveChangesAsync();

        return zahtjev;
    }


    public async Task UpdateOsnovniPodaciAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await _context.SocijalniNatjecajZahtjevi
                          .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                      ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

        zahtjev.KlasaPredmeta = dto.KlasaPredmeta ?? zahtjev.KlasaPredmeta;
        zahtjev.Adresa = dto.Adresa;
        zahtjev.Email = dto.Email;
        zahtjev.DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? zahtjev.DatumPodnosenjaZahtjeva;
        zahtjev.NapomenaObrade = dto.NapomenaObrade;
        zahtjev.RezultatObrade = dto.RezultatObrade ?? zahtjev.RezultatObrade;

        await _context.SaveChangesAsync();
    }

    public async Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId)
    {
        return await BaseQuery(asNoTracking: true)
                   .Include(z => z.Natjecaj)
                   .Include(z => z.Bodovi) // ← OVO DODAJ
                   .Include(z => z.Clanovi) // preporučeno ako koristiš u WordExportService
                   .Include(z => z.KucanstvoPodaci)
                   .Include(z => z.KucanstvoPodaci!.Prihod)
                   .FirstOrDefaultAsync(z => z.Id == zahtjevId)
               ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");
    }


    public SocijalniNatjecajClan ConvertToEntity(SocijalniNatjecajClanDto clanDto, SocijalniNatjecajZahtjev zahtjev)
        => new()
        {
            ImePrezime = clanDto.ImePrezime,
            Oib = clanDto.Oib,
            Srodstvo = clanDto.Srodstvo,
            DatumRodjenja = clanDto.DatumRodjenja,
            ZahtjevId = zahtjev.Id,
            Zahtjev = zahtjev
        };

    public async Task<PagedResult<SocijalniNatjecajZahtjevDto>> GetPagedAsync(
    long natjecajId,
    int page,
    int pageSize,
    string? sortBy,
    SortDirection sortDirection,
    string? search = null,
    RezultatObrade? osnovanost = null)
{
    var query = BaseQuery(asNoTracking: true)
        .Where(x => x.NatjecajId == natjecajId);

    if (!string.IsNullOrWhiteSpace(search))
    {
        var lowered = search.ToLower();

        query = query.Where(x =>
            x.KlasaPredmeta.ToString().Contains(lowered) ||
            x.Clanovi.Any(c =>
                c.Srodstvo == Srodstvo.PodnositeljZahtjeva &&
                (
                    c.ImePrezime.ToLower().Contains(lowered) ||
                    c.Oib.ToLower().Contains(lowered)
                )
            ) ||
            x.RezultatObrade.ToString().ToLower().Contains(lowered)
        );
    }

    if (osnovanost.HasValue)
    {
        query = query.Where(x => x.RezultatObrade == osnovanost.Value);
    }

    if (!string.IsNullOrEmpty(sortBy))
    {
        query = query.OrderByDynamic(sortBy, sortDirection == SortDirection.Descending);
    }

    var totalCount = await query.CountAsync();

    var items = await query
        .Skip(page * pageSize)
        .Take(pageSize)
        .Select(x => new SocijalniNatjecajZahtjevDto
        {
            Id = x.Id,
            KlasaPredmeta = x.KlasaPredmeta,
            DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
            Adresa = x.Adresa!,
            NatjecajId = x.NatjecajId,

            ImePrezime = x.Clanovi
                .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                .Select(c => c.ImePrezime)
                .FirstOrDefault() ?? string.Empty,

            Oib = x.Clanovi
                .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                .Select(c => c.Oib)
                .FirstOrDefault(),

            RezultatObrade = x.RezultatObrade,

            Bodovni = x.KucanstvoPodaci == null
                ? null
                : new SocijalniBodovniDto
                {
                    UkupniPrihodKucanstva = x.KucanstvoPodaci.Prihod!.UkupniPrihodKucanstva,
                    StambeniStatusKucanstva = x.KucanstvoPodaci.StambeniStatusKucanstva,
                    SastavKucanstva = x.KucanstvoPodaci.SastavKucanstva
                },

            Bodovi = x.Bodovi
        })
        .ToListAsync();

    return new PagedResult<SocijalniNatjecajZahtjevDto>
    {
        Items = items,
        TotalCount = totalCount
    };
}

}