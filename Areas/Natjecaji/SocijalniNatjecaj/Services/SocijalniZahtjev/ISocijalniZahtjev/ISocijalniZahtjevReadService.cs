using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;

public interface ISocijalniZahtjevReadService
{
    Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id);
    Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync();
    Task<PagedResult<SocijalniNatjecajZahtjevDto>> GetPagedAsync(long natjecajId, int page, int pageSize, string? sortBy, SortDirection sortDirection, string? search = null, RezultatObrade? osnovanost = null);
    Task<SocijalniNatjecajZahtjev?> GetZahtjevByIdAsync(long id);
}