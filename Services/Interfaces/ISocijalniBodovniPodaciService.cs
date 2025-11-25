using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface ISocijalniBodovniPodaciService
{
    Task<SocijalniNatjecajBodovniPodaciDto> GetAsync(long zahtjevId);
    Task<SocijalniNatjecajBodovniPodaciDto> UpdateAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto);
}