using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniNatjecajService(ApplicationDbContext context) : ISocijalniNatjecajService
{
    public async Task<List<SocijalniNatjecajDto>> GetAllAsync()
    {
        return await context.SocijalniNatjecajZahtjevi
            .Select(x => new SocijalniNatjecajDto
            {
                KlasaPredmeta = x.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
                Adresa = x.Adresa!,
                NatjecajId = x.NatjecajId
            })
            .ToListAsync();
    }

    public async Task CreateAsync(SocijalniNatjecajDto dto, string imePrezime, string oib)
    {
        var entitet = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta.GetValueOrDefault(),
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva,
            Adresa = dto.Adresa,
            CreatedAt = DateTime.UtcNow,
        };

        await context.SocijalniNatjecajZahtjevi.AddAsync(entitet);
        await context.SaveChangesAsync();

        var clan = new SocijalniNatjecajClan
        {
            Oib = oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            CreatedAt = DateTime.UtcNow
        };

        await context.SaveChangesAsync();
    }
}
