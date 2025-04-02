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
                UkupniPrihodKucanstva = x.UkupniPrihodKucanstva,
                StambeniStatusKucanstva = x.StambeniStatusKucanstva,
                SastavKucanstva = x.SastavKucanstva,
                Aktivan = x.Aktivan,
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
            UkupniPrihodKucanstva = dto.UkupniPrihodKucanstva.GetValueOrDefault(),
            StambeniStatusKucanstva = dto.StambeniStatusKucanstva.GetValueOrDefault(),
            SastavKucanstva = dto.SastavKucanstva.GetValueOrDefault(),
            Aktivan = dto.Aktivan,
            CreatedAt = DateTime.UtcNow,
        };

        await context.SocijalniNatjecajZahtjevi.AddAsync(entitet);
        await context.SaveChangesAsync();

        var clan = new SocijalniNatjecajClan
        {
            NatjecajId = entitet.Id,
            ImeIPrezime = imePrezime,
            Oib = oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            CreatedAt = DateTime.UtcNow
        };

        await context.SocijalniNatjecajClanovi.AddAsync(clan);
        await context.SaveChangesAsync();
    }
}
