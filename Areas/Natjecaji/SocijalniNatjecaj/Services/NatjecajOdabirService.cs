using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;

public class NatjecajOdabirService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<NatjecajOdabirService> logger) : INatjecajOdabirService
{
    public async Task<List<Natjecaj>> GetAllModelsAsync()
    {
        logger.LogInformation("Fetching all natjecaji");
        await using var context = contextFactory.CreateDbContext();
        return await context.Natjecaji.ToListAsync();
    }
}