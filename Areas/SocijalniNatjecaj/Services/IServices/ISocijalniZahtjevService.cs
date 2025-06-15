using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniZahtjevService
{
    Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id);
    Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync();
    //Task<SocijalniNatjecajZahtjevDto> GetByIdAsync(long id);
    Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib);
    Task UpdateOsnovniPodaciAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto);
    Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId);
    public SocijalniNatjecajClan ConvertToEntity(SocijalniNatjecajClanDto clanDto, SocijalniNatjecajZahtjev zahtjev);
    
    Task<PagedResult<SocijalniNatjecajZahtjevDto>> GetPagedAsync(
        long natjecajId,
        int page,
        int pageSize,
        string? sortBy,
        SortDirection sortDirection,
        string? search,
        RezultatObrade? osnovanost);
}