using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public interface ISocijalniNatjecajService
{
    Task<List<SocijalniNatjecajDto>> GetAllAsync();
}