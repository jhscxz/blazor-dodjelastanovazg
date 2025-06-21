using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevReadService(ApplicationDbContext context)
    : ISocijalniZahtjevReadService
{
    private IQueryable<SocijalniNatjecajZahtjev> Query(bool tracking = false) =>
        tracking ? context.SocijalniNatjecajZahtjevi : context.SocijalniNatjecajZahtjevi.AsNoTracking();

    public async Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id)
    {
        var entity = await context.SocijalniNatjecajZahtjevi
                         .Include(z => z.Clanovi)
                         .Include(z => z.BodovniPodaci)
                         .Include(z => z.KucanstvoPodaci)!.ThenInclude(k => k!.Prihod)
                         .Include(z => z.CreatedByUser)
                         .Include(z => z.UpdatedByUser)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(z => z.Id == id)
                     ?? throw new($"Zahtjev {id} nije pronađen.");

        var dto = entity.ToDto();

        dto.Clanovi = entity.Clanovi.Select(c => c.ToDto()).ToList();
        dto.KucanstvoPodaci = entity.KucanstvoPodaci?.ToDto();
        dto.BodovniPodaci = entity.BodovniPodaci?.ToDto();

        dto.CreatedBy = entity.CreatedByUser?.NormalizedUserName;
        dto.UpdatedBy = entity.UpdatedByUser?.NormalizedUserName;

        return dto;
    }


    public Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync() =>
        Query().Select(e => e.ToDto()).ToListAsync();

    public async Task<SocijalniNatjecajZahtjev?> GetZahtjevByIdAsync(long id)
    {
        return await Query()
            .Include(z => z.Natjecaj)
            .Include(z => z.BodovniPodaci)
            .Include(z => z.Bodovi)
            .Include(z => z.Clanovi)
            .Include(z => z.CreatedByUser)
            .Include(z => z.KucanstvoPodaci).ThenInclude(k => k!.Prihod)
            .FirstOrDefaultAsync(z => z.Id == id)
            ?? throw new($"Zahtjev {id} nije pronađen.");
    }

    public async Task<PagedResult<SocijalniNatjecajZahtjevDto>> GetPagedAsync(
    long natjecajId,
    int page,
    int pageSize,
    string? sortBy,
    SortDirection direction,
    string? search = null,
    RezultatObrade? filter = null)
{
    var q = context.SocijalniNatjecajZahtjevi
        .Include(z => z.Clanovi)
        .Where(z => z.NatjecajId == natjecajId);

    if (!string.IsNullOrWhiteSpace(search))
    {
        var s = search.ToLower();
        q = q.Where(z =>
            EF.Functions.Like(z.KlasaPredmeta.ToString(), $"%{s}%") ||
            EF.Functions.Like(z.RezultatObrade.ToString(), $"%{s}%") ||
            z.Clanovi.Any(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva &&
                (EF.Functions.Like(c.ImePrezime!.ToLower(), $"%{s}%") ||
                 EF.Functions.Like((c.Oib ?? string.Empty).ToLower(), $"%{s}%"))));
    }

    if (filter is not null)
        q = q.Where(z => z.RezultatObrade == filter);

    q = !string.IsNullOrWhiteSpace(sortBy)
        ? q.OrderByDynamic(sortBy, direction == SortDirection.Descending)
        : q.OrderBy(z => z.Id);

    var total = await q.CountAsync();

    var items = await q
        .Skip(page * pageSize)
        .Take(pageSize)
        .Select(z => new SocijalniNatjecajZahtjevDto
        {
            Id = z.Id,
            KlasaPredmeta = z.KlasaPredmeta,
            DatumPodnosenjaZahtjeva = z.DatumPodnosenjaZahtjeva,
            Adresa = z.Adresa!,
            NatjecajId = z.NatjecajId,
            RezultatObrade = z.RezultatObrade,

            ImePrezime = z.Clanovi
                .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                .Select(c => c.ImePrezime)
                .FirstOrDefault() ?? string.Empty,

            Oib = z.Clanovi
                .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                .Select(c => c.Oib)
                .FirstOrDefault()
        })
        .AsNoTracking()
        .ToListAsync();

    return new PagedResult<SocijalniNatjecajZahtjevDto>
    {
        Items = items,
        TotalCount = total
    };
}

}
