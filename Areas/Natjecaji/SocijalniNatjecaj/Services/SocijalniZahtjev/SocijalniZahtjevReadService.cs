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
        var entitet = await Query()
                          .Include(z => z.Clanovi)
                          .Include(z => z.KucanstvoPodaci).ThenInclude(k => k.Prihod)
                          .Include(z => z.CreatedByUser)      // ← DODAJ
                          .Include(z => z.UpdatedByUser)      // ← DODAJ
                          .FirstOrDefaultAsync(z => z.Id == id)
                      ?? throw new($"Zahtjev {id} nije pronađen.");

        return entitet.ToDto();
    }

    public Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync() =>
        Query().Select(e => e.ToDto()).ToListAsync();

    public async Task<SocijalniNatjecajZahtjev?> GetZahtjevByIdAsync(long id)
    {
        var zahtjev = await Query()
            .Include(z => z.Natjecaj)
            .Include(z => z.Bodovi)
            .Include(z => z.Clanovi)
            .Include(z => z.KucanstvoPodaci).ThenInclude(k => k.Prihod)
            .FirstOrDefaultAsync(z => z.Id == id)
            ?? throw new Exception($"Zahtjev {id} nije pronađen.");

        return zahtjev;
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
        var q = Query()
            .Where(z => z.NatjecajId == natjecajId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            q = q.Where(z => z.KlasaPredmeta.ToString().Contains(s) ||
                             z.RezultatObrade.ToString().Contains(s, StringComparison.CurrentCultureIgnoreCase) 
                             || z.Clanovi.Any(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva 
                                                   && ((c.ImePrezime ?? "").Contains(s, StringComparison.CurrentCultureIgnoreCase) 
                                                       || (c.Oib ?? "").Contains(s, StringComparison.CurrentCultureIgnoreCase))));
        }

        if (filter.HasValue)
            q = q.Where(z => z.RezultatObrade == filter);

        if (!string.IsNullOrWhiteSpace(sortBy))
            q = q.OrderByDynamic(sortBy, direction == SortDirection.Descending);

        var total = await q.CountAsync();

        var items = await q.Skip(page * pageSize)
                           .Take(pageSize)
                           .Select(e => e.ToDto())
                           .ToListAsync();

        return new PagedResult<SocijalniNatjecajZahtjevDto> { Items = items, TotalCount = total };
    }
}
