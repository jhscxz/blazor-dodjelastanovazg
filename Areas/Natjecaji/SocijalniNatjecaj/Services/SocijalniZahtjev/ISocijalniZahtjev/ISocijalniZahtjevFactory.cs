using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;

public interface ISocijalniZahtjevFactory
{
    SocijalniNatjecajZahtjev KreirajNovi(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib);
}