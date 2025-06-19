using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevReadService(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor)
    : ISocijalniZahtjevReadService
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

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
                     ?? throw new Exception($"Zahtjev s ID-om {id} nije pronađen.");

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

    public async Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long id)
    {
        return await BaseQuery(asNoTracking: true)
                   .Include(z => z.Natjecaj)
                   .Include(z => z.Bodovi)
                   .Include(z => z.Clanovi)
                   .Include(z => z.KucanstvoPodaci)
                       .ThenInclude(k => k!.Prihod)
                   .FirstOrDefaultAsync(z => z.Id == id)
               ?? throw new Exception($"Zahtjev s ID-om {id} nije pronađen.");
    }

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
                    c.Oib != null &&
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
