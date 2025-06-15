using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniZahtjevFormHandler
{
    Task<(bool Success, List<string> Errors)> SubmitAsync(SocijalniNatjecajZahtjevDto model, int? rezultatObrade);
}