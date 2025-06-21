using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface ISocijalniKucanstvoService
{
    Task<SocijalniKucanstvoPodaciDto> UpdateKucanstvoPodaciAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto);
    Task<SocijalniKucanstvoPodaciDto?> GetAsync(long zahtjevId);
}