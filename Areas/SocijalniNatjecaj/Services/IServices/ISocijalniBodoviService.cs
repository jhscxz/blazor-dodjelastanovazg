using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

public interface ISocijalniBodoviService
{
    Task IzracunajIBodujAsync(long zahtjevId);
    Task<SocijalniNatjecajBodovi?> GetByIdAsync(long zahtjevId);
}