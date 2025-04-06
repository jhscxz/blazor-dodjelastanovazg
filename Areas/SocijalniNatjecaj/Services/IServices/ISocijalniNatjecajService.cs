using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniNatjecajService
{
    Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync();

    Task CreateAsync(SocijalniNatjecajZahtjevDto zahtjevDto, string imePrezime, string? oib);
}