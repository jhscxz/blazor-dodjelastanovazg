using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class NatjecajOdabirService(ApplicationDbContext context) : INatjecajOdabirService
{
    public async Task<List<Natjecaj>> GetAllModelsAsync()
    {
        return await context.Natjecaji.ToListAsync();
    }
}