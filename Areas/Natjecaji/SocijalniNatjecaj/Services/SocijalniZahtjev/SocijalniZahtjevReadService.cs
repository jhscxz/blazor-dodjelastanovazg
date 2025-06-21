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
        var dto = await context.SocijalniNatjecajZahtjevi
                      .Where(z => z.Id == id)
                      .Select(z => new SocijalniNatjecajZahtjevDto
                      {
                          Id = z.Id,
                          NatjecajId = z.NatjecajId,
                          KlasaPredmeta = z.KlasaPredmeta,
                          DatumPodnosenjaZahtjeva = z.DatumPodnosenjaZahtjeva,
                          Adresa = z.Adresa,
                          Email = z.Email,

                          ImePrezime = z.Clanovi
                              .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                              .Select(c => c.ImePrezime)
                              .FirstOrDefault() ?? string.Empty,

                          Oib = z.Clanovi
                              .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                              .Select(c => c.Oib)
                              .FirstOrDefault(),

                          RezultatObrade = z.RezultatObrade,
                          NapomenaObrade = z.NapomenaObrade,

                          // audit
                          CreatedAt = z.CreatedAt,
                          CreatedBy = z.CreatedByUser!.NormalizedUserName,
                          UpdatedAt = z.UpdatedAt,
                          UpdatedBy = z.UpdatedByUser!.NormalizedUserName,

                          // kućanstvo
                          KucanstvoPodaci = z.KucanstvoPodaci == null
                              ? null
                              : new SocijalniKucanstvoPodaciDto
                              {
                                  UkupniPrihodKucanstva = z.KucanstvoPodaci.Prihod!.UkupniPrihodKucanstva,
                                  PrihodPoClanu = z.KucanstvoPodaci.Prihod.PrihodPoClanu,
                                  PostotakProsjeka = z.KucanstvoPodaci.Prihod.PostotakProsjeka,
                                  IspunjavaUvjetPrihoda = z.KucanstvoPodaci.Prihod.IspunjavaUvjetPrihoda,
                                  PrebivanjeOd = z.KucanstvoPodaci.PrebivanjeOd,
                                  StambeniStatusKucanstva = z.KucanstvoPodaci.StambeniStatusKucanstva,
                                  SastavKucanstva = z.KucanstvoPodaci.SastavKucanstva,
                                  ZahtjevId = z.Id
                              },

                          // članovi
                          Clanovi = z.Clanovi.Select(c => new SocijalniNatjecajClanDto
                          {
                              Id = c.Id,
                              ZahtjevId = c.ZahtjevId,
                              ImePrezime = c.ImePrezime,
                              Oib = c.Oib,
                              Srodstvo = c.Srodstvo,
                              DatumRodjenja = c.DatumRodjenja
                          }).ToList()
                      })
                      .AsNoTracking()
                      .FirstOrDefaultAsync()
                  ?? throw new($"Zahtjev {id} nije pronađen.");

        return dto;
    }


    public Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync() =>
        Query().Select(e => e.ToDto()).ToListAsync();

    public async Task<SocijalniNatjecajZahtjev?> GetZahtjevByIdAsync(long id)
    {
        var zahtjev = await Query()
                          .Include(z => z.Natjecaj)
                          .Include(z => z.BodovniPodaci)
                          .Include(z => z.Bodovi)
                          .Include(z => z.Clanovi)
                          .Include(z => z.CreatedByUser)
                          .Include(z => z.KucanstvoPodaci).ThenInclude(k => k!.Prihod)
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
        // ----- osnovni upit ----------------------------------------------------
        IQueryable<SocijalniNatjecajZahtjev> q = context.SocijalniNatjecajZahtjevi
            .Where(z => z.NatjecajId == natjecajId);

        // ----- search ----------------------------------------------------------
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

        // ----- filter po rezultatu obrade -------------------------------------
        if (filter is not null)
            q = q.Where(z => z.RezultatObrade == filter);

        // ----- sortiranje ------------------------------------------------------
        if (!string.IsNullOrWhiteSpace(sortBy))
            q = q.OrderByDynamic(sortBy, direction == SortDirection.Descending);
        else
            q = q.OrderBy(z => z.Id);

        // ----- ukupno ----------------------------------------------------------
        var total = await q.CountAsync();

        // ----- stranica --------------------------------------------------------
        var items = await q.Skip(page * pageSize)
            .Take(pageSize)
            .Select(z => new SocijalniNatjecajZahtjevDto
            {
                Id = z.Id,
                KlasaPredmeta = z.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = z.DatumPodnosenjaZahtjeva,
                Adresa = z.Adresa!,
                NatjecajId = z.NatjecajId,
                ImePrezime = z.Clanovi
                    .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                    .Select(c => c.ImePrezime)
                    .FirstOrDefault() ?? string.Empty,
                Oib = z.Clanovi
                    .Where(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)
                    .Select(c => c.Oib)
                    .FirstOrDefault(),
                RezultatObrade = z.RezultatObrade
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