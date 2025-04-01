using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class NatjecajOdabirService : INatjecajOdabirService
{
    private readonly ApplicationDbContext _context;

    public NatjecajOdabirService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Natjecaj>> GetAllModelsAsync()
    {
        return await _context.Natjecaji.ToListAsync();
    }
}