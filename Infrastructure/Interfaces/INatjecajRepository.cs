using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Infrastructure.Interfaces;

public interface INatjecajRepository
{
    Task<Natjecaj?> GetByIdAsync(long id);
    Task<Natjecaj?> GetByKlasaAsync(int klasa);
    Task<List<Natjecaj>> GetAllAsync();
    Task AddAsync(Natjecaj natjecaj);
    Task UpdateAsync(Natjecaj natjecaj);
    Task SaveChangesAsync();
}