using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface ISocijalniZahtjevFormHandler
{
    Task<(bool Success, List<string> Errors)> SubmitAsync(SocijalniNatjecajZahtjevDto model, int? rezultatObrade);
}