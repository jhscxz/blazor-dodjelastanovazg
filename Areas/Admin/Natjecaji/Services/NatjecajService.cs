using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Mapster;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;

public class NatjecajService(INatjecajRepository repo, ILogger<NatjecajService> logger) : INatjecajService
{
    public async Task<NatjecajDto?> GetByIdAsync(long id)
    {
        logger.LogDebug("Dohvaćanje natječaja {Id}", id);

        var entity = await repo.GetByIdAsync(id);
        logger.LogDebug(entity != null
            ? "Natječaj {Id} pronađen"
            : "Natječaj {Id} nije pronađen", id);

        return entity?.Adapt<NatjecajDto>(TypeAdapterConfig.GlobalSettings);
    }
    
    public async Task<NatjecajDto?> GetByKlasaAsync(int klasa)
    {
        logger.LogDebug("Dohvaćanje natječaja {Klasa}", klasa);

        var entity = await repo.GetByKlasaAsync(klasa);
        logger.LogDebug(entity != null
            ? "Natječaj {Klasa} pronađen"
            : "Natječaj {Klasa} nije pronađen", klasa);

        return entity?.Adapt<NatjecajDto>(TypeAdapterConfig.GlobalSettings);
    }

    public async Task<IEnumerable<NatjecajDto>> GetAllAsync()
    {
        logger.LogDebug("Dohvaćanje svih natječaja");
        var entities = await repo.GetAllAsync();
        logger.LogDebug("Pronađeno {Count} natječaja", entities.Count);
        return entities.Adapt<IEnumerable<NatjecajDto>>();
    }

    public async Task<bool> CreateAsync(NatjecajDto dto)
    {
        var entity = dto.Adapt<Natjecaj>(TypeAdapterConfig.GlobalSettings);

        logger.LogInformation("Kreiranje natječaja {Klasa}", entity.Klasa);

        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        await repo.AddAsync(entity);
        await repo.SaveChangesAsync();

        logger.LogInformation("Natječaj {Klasa} kreiran", entity.Klasa);

        return true;
    }

    public async Task<bool> UpdateAsync(int klasa, NatjecajDto dto)
    {
        logger.LogInformation("Ažuriranje natječaja {Klasa}", klasa);

        var entity = await repo.GetByKlasaAsync(klasa);
        if (entity is null)
        {
            logger.LogWarning("Natječaj {Klasa} nije pronađen", klasa);
            return false;
        }

        dto.Adapt(entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await repo.UpdateAsync(entity);
        logger.LogInformation("Natječaj {Klasa} ažuriran", klasa);
        return true;
    }
}