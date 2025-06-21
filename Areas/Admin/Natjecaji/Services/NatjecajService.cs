using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Mapster;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;

public class NatjecajService(INatjecajRepository repo) : INatjecajService
{
    public async Task<NatjecajDto?> GetByKlasaAsync(int klasa)
    {
        var entity = await repo.GetByKlasaAsync(klasa);
        return entity?.Adapt<NatjecajDto>(TypeAdapterConfig.GlobalSettings);
    }

    public async Task<IEnumerable<NatjecajDto>> GetAllAsync()
    {
        var entities = await repo.GetAllAsync();
        return entities.Adapt<IEnumerable<NatjecajDto>>();
    }

    public async Task<bool> CreateAsync(NatjecajDto dto)
    {
        var entity = dto.Adapt<Natjecaj>(TypeAdapterConfig.GlobalSettings);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        await repo.AddAsync(entity);
        await repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int klasa, NatjecajDto dto)
    {
        var entity = await repo.GetByKlasaAsync(klasa);
        if (entity is null) return false;

        dto.Adapt(entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await repo.SaveChangesAsync();
        return true;
    }
}