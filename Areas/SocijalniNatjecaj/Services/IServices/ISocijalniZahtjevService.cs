using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniZahtjevService
{
    Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync();
    Task<SocijalniNatjecajZahtjevDto> GetByIdAsync(long id);
    Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib);
    Task UpdateOsnovniPodaciAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto);
    Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId);
}