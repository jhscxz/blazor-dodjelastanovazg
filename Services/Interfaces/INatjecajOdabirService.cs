using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface INatjecajOdabirService
{
    Task<List<Natjecaj>> GetAllModelsAsync();
}