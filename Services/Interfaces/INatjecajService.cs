using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface INatjecajService
{
    Task<NatjecajDto?> GetByIdAsync(long id);
    Task<NatjecajDto?> GetByKlasaAsync(int klasa);
    Task<bool> CreateAsync(NatjecajDto dto);
    Task<bool> UpdateAsync(int klasa, NatjecajDto dto);
    Task<IEnumerable<NatjecajDto>> GetAllAsync();
}