using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Infrastructure.Interfaces;

public interface INatjecajRepository
{
    Task<Natjecaj?> GetByKlasaAsync(int klasa);
    Task<List<Natjecaj>> GetAllAsync();
    Task AddAsync(Natjecaj natjecaj);
    Task SaveChangesAsync();
}