using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices
{
    public interface ISocijalniNatjecajDetaljiService
    {
        Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id);
        Task AddClanAsync(SocijalniNatjecajClan noviClan);
        SocijalniNatjecajClan ConvertToEntity(SocijalniNatjecajClanDto clanDto, SocijalniNatjecajZahtjev zahtjev);
        Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId);
        Task UpdateKucanstvoPodaciAsync(long zahtjevId, SocijalniKucanstvoPodaciDto dto);
        Task UpdateOsnovniPodaciAsync(long id, SocijalniNatjecajOsnovnoEditDto dto);
        public Task RemoveClanAsync(long zahtjevId, long clanId);
        public Task EditClanAsync(SocijalniNatjecajClanDto azuriraniClan);
        string GetCurrentUserId();
    }
}