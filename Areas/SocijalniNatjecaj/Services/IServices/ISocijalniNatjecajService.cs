using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniNatjecajService
{
    Task<List<SocijalniNatjecajDto>> GetAllAsync();
}