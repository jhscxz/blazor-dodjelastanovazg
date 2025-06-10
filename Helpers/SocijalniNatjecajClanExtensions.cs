using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Helpers;

public static class SocijalniNatjecajClanExtensions
{
    public static SocijalniNatjecajClanDto ToDto(this SocijalniNatjecajClan entity)
    {
        return new SocijalniNatjecajClanDto
        {
            Id = entity.Id,
            ZahtjevId = entity.ZahtjevId,
            ImePrezime = entity.ImePrezime,
            Oib = entity.Oib,
            Srodstvo = entity.Srodstvo,
            DatumRodjenja = entity.DatumRodjenja
        }.WithAuditFrom(entity);
    }
}