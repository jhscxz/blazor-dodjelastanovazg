using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Infrastructure.Interfaces;

public interface ISocijalniBodoviRepository
{
    Task<SocijalniNatjecajZahtjev?> GetZahtjevWithDetailsAsync(long zahtjevId);
    Task AddBodoviAsync(SocijalniNatjecajBodovi bodovi);
    Task SaveChangesAsync();
}