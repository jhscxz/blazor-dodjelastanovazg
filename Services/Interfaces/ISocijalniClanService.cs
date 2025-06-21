using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface ISocijalniClanService
{
    Task<SocijalniNatjecajClanDto> EditClanAsync(SocijalniNatjecajClanDto dto);
    Task<SocijalniNatjecajClanDto> AddClanAsync(SocijalniNatjecajClan noviClan);
    Task RemoveClanAsync(long zahtjevId, long clanId);
    Task<Dictionary<long, List<SocijalniNatjecajClanDto>>> GetForZahtjeviAsync(IEnumerable<long> zahtjevIds);

}