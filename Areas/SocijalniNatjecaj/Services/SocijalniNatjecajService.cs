using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniNatjecajService : ISocijalniNatjecajService
{
    private readonly ApplicationDbContext _context;

    public SocijalniNatjecajService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SocijalniNatjecajDto>> GetAllAsync()
    {
        return await _context.SocijalniNatjecaji
            .Select(x => new SocijalniNatjecajDto
            {
                Id = x.Id,
                KlasaPredmeta = x.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
                Adresa = x.Adresa,
                UkupniPrihodKucanstva = x.UkupniPrihodKucanstva,
                StambeniStatusKucanstva = x.StambeniStatusKucanstva,
                SastavKucanstva = x.SastavKucanstva,
                Aktivan = x.Aktivan,
                NatjecajId = x.NatjecajId
            })
            .ToListAsync();
    }
}