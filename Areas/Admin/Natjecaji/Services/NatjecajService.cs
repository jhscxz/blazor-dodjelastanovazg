using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;

public class NatjecajService(ApplicationDbContext context) : INatjecajService
{
    public async Task<NatjecajDto?> GetByKlasaAsync(int klasa)
    {
        var entity = await context.Natjecaji.FirstOrDefaultAsync(x => x.Klasa == klasa);
        return entity?.Adapt<NatjecajDto>();
    }

    public async Task<IEnumerable<NatjecajDto>> GetAllAsync()
    {
        return await context.Natjecaji
            .ProjectToType<NatjecajDto>()
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(NatjecajDto dto)
    {
        var entity = dto.Adapt<Natjecaj>();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        context.Natjecaji.Add(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int klasa, NatjecajDto dto)
    {
        var entity = await context.Natjecaji.FirstOrDefaultAsync(x => x.Klasa == klasa);
        if (entity is null) return false;

        dto.Adapt(entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }

}