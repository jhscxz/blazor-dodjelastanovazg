using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;

public class NatjecajOdabirService(ApplicationDbContext context, ILogger<NatjecajOdabirService> logger) : INatjecajOdabirService
{
    public async Task<List<Natjecaj>> GetAllModelsAsync()
    {
        logger.LogInformation("Fetching all natjecaji");
        return await context.Natjecaji.ToListAsync();
    }
}