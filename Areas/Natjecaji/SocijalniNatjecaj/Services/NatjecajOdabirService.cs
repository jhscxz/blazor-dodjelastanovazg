using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;

public class NatjecajOdabirService(ApplicationDbContext context) : INatjecajOdabirService
{
    public async Task<List<Natjecaj>> GetAllModelsAsync()
    {
        return await context.Natjecaji.ToListAsync();
    }
}