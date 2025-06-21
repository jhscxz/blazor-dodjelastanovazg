using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;

public class NatjecajService : INatjecajService
{
    private readonly ApplicationDbContext _context;

    public NatjecajService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<NatjecajDto?> GetByKlasaAsync(int klasa)
    {
        var entity = await _context.Natjecaji.FirstOrDefaultAsync(x => x.Klasa == klasa);
        return entity?.Adapt<NatjecajDto>();
    }

    public async Task<IEnumerable<NatjecajDto>> GetAllAsync()
    {
        return await _context.Natjecaji
            .ProjectToType<NatjecajDto>()
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(NatjecajDto dto)
    {
        var entity = dto.Adapt<Natjecaj>();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Natjecaji.Add(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int klasa, NatjecajDto dto)
    {
        var entity = await _context.Natjecaji.FirstOrDefaultAsync(x => x.Klasa == klasa);
        if (entity is null) return false;

        dto.Adapt(entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

}