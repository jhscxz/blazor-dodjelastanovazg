using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Services.Interfaces
{
    public interface ISocijalniNatjecajService
    {
        Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync();
        Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib);
        Task<SocijalniNatjecajZahtjevDto> GetByIdAsync(long id);
    }
}