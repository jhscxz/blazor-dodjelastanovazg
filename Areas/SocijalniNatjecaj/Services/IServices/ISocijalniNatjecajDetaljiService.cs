using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices
{
    public interface ISocijalniNatjecajDetaljiService
    {
        Task<SocijalniNatjecajDto> GetDetaljiAsync(long id);
        Task AddClanAsync(SocijalniNatjecajClan noviClan);
        SocijalniNatjecajClan ConvertToEntity(SocijalniNatjecajClanDto clanDto, SocijalniNatjecajZahtjev zahtjev);
        Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId);
    }
}