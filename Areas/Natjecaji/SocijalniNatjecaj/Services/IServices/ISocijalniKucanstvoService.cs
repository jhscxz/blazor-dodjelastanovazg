using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniKucanstvoService
{
    Task<SocijalniKucanstvoPodaciDto> UpdateKucanstvoPodaciAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto);
    Task<SocijalniKucanstvoPodaciDto?> GetAsync(long zahtjevId);
}