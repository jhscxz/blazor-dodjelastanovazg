using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface ISocijalniBodovnaGreskaService
{
    Task<List<SocijalniNatjecajBodovnaGreska>> PronadiGreskeAsync(SocijalniNatjecajZahtjev zahtjev);
    Task<List<SocijalniNatjecajBodovnaGreska>> GetByZahtjevIdAsync(long zahtjevId);
}