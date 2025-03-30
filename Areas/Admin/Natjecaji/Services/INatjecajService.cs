using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;

public interface INatjecajService
{
    Task<NatjecajDto?> GetByKlasaAsync(int klasa);
    Task<bool> CreateAsync(NatjecajDto dto);
    Task<bool> UpdateAsync(int klasa, NatjecajDto dto);
    Task<IEnumerable<NatjecajDto>> GetAllAsync();
}