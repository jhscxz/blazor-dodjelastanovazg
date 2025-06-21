using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Services.Interfaces
{
    public interface ISocijalniNatjecajDetaljiService
    {
        Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id);
        Task UpdateOsnovniPodaciAsync(long id, SocijalniNatjecajOsnovnoEditDto dto);
        Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId);
        SocijalniNatjecajClan ConvertToEntity(SocijalniNatjecajClanDto clanDto, SocijalniNatjecajZahtjev zahtjev);
    }
}