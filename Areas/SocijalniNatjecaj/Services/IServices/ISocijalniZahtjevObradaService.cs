using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniZahtjevObradaService
{
    Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime,
        string? oib);
    Task SpremiKucanstvoIObracunajAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto);
    Task SpremiBodovnePodatkeIObracunajAsync(long zahtjevId, SocijalniNatjecajBodovniPodaciDto dto);
    Task DodajClanaIObracunajAsync(long zahtjevId, SocijalniNatjecajClanDto clanDto);
    Task ObrisiClanaIObracunajAsync(long zahtjevId, long clanId);
    Task EditClanIObracunajAsync(SocijalniNatjecajClanDto azurirani);
    Task AzurirajOsnovnoIObracunajAkoTrebaAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto);
}