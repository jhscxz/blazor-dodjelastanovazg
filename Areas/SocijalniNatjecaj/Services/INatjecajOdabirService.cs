using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public interface INatjecajOdabirService
{
    Task<List<Natjecaj>> GetAllModelsAsync();
}