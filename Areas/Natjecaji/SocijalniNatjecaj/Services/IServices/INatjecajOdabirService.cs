using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface INatjecajOdabirService
{
    Task<List<Natjecaj>> GetAllModelsAsync();
}