using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;

public interface ISocijalniZahtjevGreskaService
{
    Task ObradiGreskeAsync(SocijalniNatjecajZahtjev zahtjev);
}