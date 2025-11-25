using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Infrastructure.Interfaces;

public interface ISocijalniBodoviRepository
{
    Task<SocijalniNatjecajZahtjev?> GetZahtjevWithDetailsAsync(ApplicationDbContext context, long zahtjevId);
    Task AddBodoviAsync(ApplicationDbContext context, SocijalniNatjecajBodovi bodovi);
    Task SaveChangesAsync(ApplicationDbContext context);
}