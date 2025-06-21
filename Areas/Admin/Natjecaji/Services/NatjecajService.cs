using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Services.Interfaces;
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

        if (entity is null) return null;

        return new NatjecajDto
        {
            Vrsta = entity.PriustiviIliSocijalni == 2 ? "Socijalni" : "Priuštivi",
            Klasa = entity.Klasa,
            ProsjekPlace = entity.ProsjekPlace,
            DatumObjave = entity.DatumObjave.ToDateTime(TimeOnly.MinValue),
            RokZaPrijavu = entity.RokZaPrijavu.ToDateTime(TimeOnly.MinValue),
            Status = entity.Zakljucen == 2 ? "Zaključen" : "Aktivan"
        };
    }

    public async Task<IEnumerable<NatjecajDto>> GetAllAsync()
    {
        return await _context.Natjecaji
            .Select(entity => new NatjecajDto
            {
                Vrsta = entity.PriustiviIliSocijalni == 2 ? "Socijalni" : "Priuštivi",
                Klasa = entity.Klasa,
                ProsjekPlace = entity.ProsjekPlace,
                DatumObjave = entity.DatumObjave.ToDateTime(TimeOnly.MinValue),
                RokZaPrijavu = entity.RokZaPrijavu.ToDateTime(TimeOnly.MinValue),
                Status = entity.Zakljucen == 2 ? "Zaključen" : "Aktivan"
            })
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(NatjecajDto dto)
    {
        var entity = new Models.Natjecaj
        {
            PriustiviIliSocijalni = dto.Vrsta == "Socijalni" ? (byte)2 : (byte)1,
            Klasa = dto.Klasa,
            ProsjekPlace = dto.ProsjekPlace,
            DatumObjave = DateOnly.FromDateTime(dto.DatumObjave!.Value),
            RokZaPrijavu = DateOnly.FromDateTime(dto.RokZaPrijavu!.Value),
            Zakljucen = dto.Status == "Zaključen" ? (byte)2 : (byte)1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Natjecaji.Add(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(int klasa, NatjecajDto dto)
    {
        var entity = await _context.Natjecaji.FirstOrDefaultAsync(x => x.Klasa == klasa);
        if (entity is null) return false;

        entity.PriustiviIliSocijalni = dto.Vrsta == "Socijalni" ? (byte)2 : (byte)1;
        entity.ProsjekPlace = dto.ProsjekPlace;
        entity.DatumObjave = DateOnly.FromDateTime(dto.DatumObjave!.Value);
        entity.RokZaPrijavu = DateOnly.FromDateTime(dto.RokZaPrijavu!.Value);
        entity.Zakljucen = dto.Status == "Zaključen" ? (byte)2 : (byte)1;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}