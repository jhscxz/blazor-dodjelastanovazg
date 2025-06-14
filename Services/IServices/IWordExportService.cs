using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Services.IServices;

public interface IWordExportService
{
    Task<byte[]> GenerirajIzvjestajAsync(SocijalniNatjecajZahtjev zahtjev, SocijalniNatjecajBodovi bodovi);
}