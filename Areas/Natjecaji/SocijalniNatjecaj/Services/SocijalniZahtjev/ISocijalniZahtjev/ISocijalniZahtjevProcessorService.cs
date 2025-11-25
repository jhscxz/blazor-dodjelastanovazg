using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;

public interface ISocijalniZahtjevProcessorService
{
    Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib);
    Task AzurirajOsnovnoIObradiAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto);
    Task SpremiKucanstvoIObradiAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto);
    Task SpremiBodovnePodatkeIObradiAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto);
    Task DodajClanaIObradiAsync(long zahtjevId, SocijalniNatjecajClanDto clanDto);
    Task UrediClanaIObradiAsync(SocijalniNatjecajClanDto clanDto);
    Task ObrisiClanaIObradiAsync(long zahtjevId, long clanId);
    Task ObradiSveZahtjeveAsync(long natjecajId);
}