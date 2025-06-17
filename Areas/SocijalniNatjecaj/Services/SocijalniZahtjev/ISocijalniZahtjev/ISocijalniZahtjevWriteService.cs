using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;

public interface ISocijalniZahtjevWriteService
{
    Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjev zahtjev);
    Task UpdateOsnovnoAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto);
}