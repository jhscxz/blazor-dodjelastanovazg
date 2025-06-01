using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices
{
    public interface ISocijalniNatjecajService
    {
        Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync();
        Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib);
        Task<SocijalniNatjecajZahtjevDto> GetByIdAsync(long id);
    }
}