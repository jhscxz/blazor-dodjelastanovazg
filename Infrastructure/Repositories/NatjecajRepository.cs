using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Infrastructure.Repositories;

public class NatjecajRepository(ApplicationDbContext context) : INatjecajRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Natjecaj?> GetByKlasaAsync(int klasa)
    {
        return await _context.Natjecaji.FirstOrDefaultAsync(n => n.Klasa == klasa);
    }

    public async Task<List<Natjecaj>> GetAllAsync()
    {
        return await _context.Natjecaji.ToListAsync();
    }

    public Task AddAsync(Natjecaj natjecaj)
    {
        _context.Natjecaji.Add(natjecaj);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}