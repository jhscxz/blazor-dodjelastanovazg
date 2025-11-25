using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Infrastructure.Repositories;

public class NatjecajRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : INatjecajRepository
{
    public async Task<Natjecaj?> GetByIdAsync(long id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Natjecaji.FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<Natjecaj?> GetByKlasaAsync(int klasa)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Natjecaji.FirstOrDefaultAsync(n => n.Klasa == klasa);
    }

    public async Task<List<Natjecaj>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Natjecaji.ToListAsync();
    }

    public async Task AddAsync(Natjecaj natjecaj)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Natjecaji.Add(natjecaj);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Natjecaj natjecaj)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Natjecaji.Update(natjecaj);
        await context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.SaveChangesAsync();
    }
}