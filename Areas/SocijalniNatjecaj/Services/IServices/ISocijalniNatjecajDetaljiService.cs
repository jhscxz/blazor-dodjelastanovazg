using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices
{
    public interface ISocijalniNatjecajDetaljiService
    {
        Task<SocijalniNatjecajDto> GetDetaljiAsync(long id);
    }
}