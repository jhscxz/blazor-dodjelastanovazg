using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;

public interface ISocijalniZahtjevGreskaService
{
    Task ObradiGreskeAsync(SocijalniNatjecajZahtjev zahtjev);
}