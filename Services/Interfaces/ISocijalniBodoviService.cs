using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface ISocijalniBodoviService
{
    Task IzracunajIBodujAsync(long zahtjevId);
    Task<SocijalniNatjecajBodovi?> GetByIdAsync(long zahtjevId);
    Task<List<SocijalniNatjecajBodovi>> GetForZahtjeviAsync(List<long> zahtjevIds);
}