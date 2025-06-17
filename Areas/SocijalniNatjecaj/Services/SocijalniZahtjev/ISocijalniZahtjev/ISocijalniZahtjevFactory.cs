namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

public interface ISocijalniZahtjevFactory
{
    SocijalniNatjecajZahtjev KreirajNovi(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib);
    SocijalniPrihodi KreirajPrazanPrihod(long kucanstvoId);
}