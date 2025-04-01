using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniNatjecajService(ApplicationDbContext context) : ISocijalniNatjecajService
{
    public async Task<List<SocijalniNatjecajDto>> GetAllAsync()
    {
        return await context.SocijalniNatjecaji
            .Select(x => new SocijalniNatjecajDto
            {
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