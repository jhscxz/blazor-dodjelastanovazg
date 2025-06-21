using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface IWordExportService
{
    Task<byte[]> GenerirajIzvjestajAsync(SocijalniNatjecajZahtjev zahtjev, SocijalniNatjecajBodovi bodovi);
}