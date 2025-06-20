using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniBodovnaGreskaService
{
    Task<List<SocijalniNatjecajBodovnaGreska>> PronadiGreskeAsync(SocijalniNatjecajZahtjev zahtjev);
}