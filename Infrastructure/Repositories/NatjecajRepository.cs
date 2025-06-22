using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Infrastructure.Repositories;

public class NatjecajRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : INatjecajRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

    public async Task<Natjecaj?> GetByKlasaAsync(int klasa)
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.Natjecaji.FirstOrDefaultAsync(n => n.Klasa == klasa);
    }

    public async Task<List<Natjecaj>> GetAllAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        return await context.Natjecaji.ToListAsync();
    }

    public Task AddAsync(Natjecaj natjecaj)
    {
        await using var context = _contextFactory.CreateDbContext();
        context.Natjecaji.Add(natjecaj);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        return context.SaveChangesAsync();
    }
}