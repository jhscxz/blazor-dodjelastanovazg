using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniClanService
{
    Task<SocijalniNatjecajClanDto> EditClanAsync(SocijalniNatjecajClanDto dto);
    Task<SocijalniNatjecajClanDto> AddClanAsync(SocijalniNatjecajClan noviClan);
    Task RemoveClanAsync(long zahtjevId, long clanId);
}