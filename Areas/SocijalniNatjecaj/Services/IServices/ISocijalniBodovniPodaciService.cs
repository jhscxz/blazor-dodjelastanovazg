using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniBodovniPodaciService
{
    Task<SocijalniNatjecajBodovniPodaciDto> GetAsync(long zahtjevId);
    Task UpdateAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto);
}