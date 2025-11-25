using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevReadService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<SocijalniZahtjevReadService> logger) : ISocijalniZahtjevReadService
{
    private IQueryable<SocijalniNatjecajZahtjev> Query(ApplicationDbContext context, bool tracking = false) =>
        tracking ? context.SocijalniNatjecajZahtjevi : context.SocijalniNatjecajZahtjevi.AsNoTracking();

    public async Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id)
    {
        logger.LogDebug("Dohvaćanje detalja zahtjeva {ZahtjevId}", id);
        await using var context = await contextFactory.CreateDbContextAsync();
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
        dto.RowVersion = entity.RowVersion;
        
        dto.CreatedBy = entity.CreatedByUser?.NormalizedUserName;
        dto.UpdatedBy = entity.UpdatedByUser?.NormalizedUserName;

        logger.LogDebug("Dohvaćeni detalji zahtjeva {ZahtjevId}", id);

        return dto;
    }


    public async Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync()
    {
        logger.LogDebug("Dohvaćanje svih socijalnih zahtjeva");
        await using var context = await contextFactory.CreateDbContextAsync();
        return await Query(context).Select(e => e.ToDto()).ToListAsync();
    }

    public async Task<SocijalniNatjecajZahtjev?> GetZahtjevByIdAsync(long id)
    {
        logger.LogDebug("Dohvaćanje zahtjeva {ZahtjevId}", id);
        await using var context = await contextFactory.CreateDbContextAsync();
        var entity = await Query(context)
            .Include(z => z.Natjecaj)
            .Include(z => z.BodovniPodaci)
            .Include(z => z.Bodovi)
            .Include(z => z.Clanovi)
            .Include(z => z.CreatedByUser)
            .Include(z => z.KucanstvoPodaci).ThenInclude(k => k!.Prihod)
            .FirstOrDefaultAsync(z => z.Id == id);

        if (entity == null)
        {
            logger.LogWarning("Zahtjev {ZahtjevId} nije pronađen", id);
            throw new($"Zahtjev {id} nije pronađen.");
        }

        logger.LogDebug("Zahtjev {ZahtjevId} pronađen", id);
        return entity;
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
        logger.LogDebug("Paged zahtjevi natječaja {NatjecajId} - page {Page} size {PageSize}", natjecajId, page, pageSize);
        await using var context = await contextFactory.CreateDbContextAsync();
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

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            q = sortBy switch
            {
                SortKeys.KlasaPredmeta => direction == SortDirection.Descending
                    ? q.OrderByDescending(z => z.KlasaPredmeta)
                    : q.OrderBy(z => z.KlasaPredmeta),

                SortKeys.ImePrezime => direction == SortDirection.Descending
                    ? q.OrderByDescending(z => z.Clanovi
                        .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                        .Select(c => c.ImePrezime)
                        .FirstOrDefault())
                    : q.OrderBy(z => z.Clanovi
                        .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                        .Select(c => c.ImePrezime)
                        .FirstOrDefault()),

                SortKeys.Oib => direction == SortDirection.Descending
                    ? q.OrderByDescending(z => z.Clanovi
                        .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                        .Select(c => c.Oib)
                        .FirstOrDefault())
                    : q.OrderBy(z => z.Clanovi
                        .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                        .Select(c => c.Oib)
                        .FirstOrDefault()),

                SortKeys.Bodovi => direction == SortDirection.Descending
                    ? q.OrderByDescending(z => z.Bodovi!.UkupnoBodova)
                    : q.OrderBy(z => z.Bodovi!.UkupnoBodova),

                SortKeys.RezultatObrade => direction == SortDirection.Descending
                    ? q.OrderByDescending(z => z.RezultatObrade)
                    : q.OrderBy(z => z.RezultatObrade),

                _ => q.OrderByDynamic(sortBy, direction == SortDirection.Descending)
            };
        }

        else
        {
            q = q.OrderBy(z => z.Id);
        }

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

        if (sortBy == SortKeys.RezultatObrade)
        {
            items = (direction == SortDirection.Descending
                    ? items.OrderBy(i => GetRezultatObradePriority(i.RezultatObrade))
                    : items.OrderByDescending(i => GetRezultatObradePriority(i.RezultatObrade))
                ).ToList();
        }
        
        logger.LogDebug("Pronađeno {Count} zahtjeva (ukupno {Total})", items.Count, total);
        
        return new PagedResult<SocijalniNatjecajZahtjevDto>
        {
            Items = items,
            TotalCount = total
        };
    }
    private static int GetRezultatObradePriority(RezultatObrade? r) => r switch
    {
        RezultatObrade.Osnovan => 1,
        RezultatObrade.Greška => 2,
        RezultatObrade.Neosnovan => 3,
        RezultatObrade.Nepotpun => 4,
        _ => 5
    };
}