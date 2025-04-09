using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniNatjecajService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    : ISocijalniNatjecajService
{
    private string GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";
    }

    public async Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync()
    {
        return await context.SocijalniNatjecajZahtjevi
            .Select(x => new SocijalniNatjecajZahtjevDto
            {
                Id = x.Id,
                KlasaPredmeta = x.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
                Adresa = x.Adresa!,
                NatjecajId = x.NatjecajId
            })
            .ToListAsync();
    }

    public async Task CreateAsync(SocijalniNatjecajZahtjevDto zahtjevDto, string imePrezime, string? oib)
    {
        string currentUserId = GetCurrentUserId();

        // 1. Kreiraj novi zahtjev
        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = zahtjevDto.NatjecajId,
            KlasaPredmeta = zahtjevDto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = zahtjevDto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = zahtjevDto.Adresa,
            RezultatObrade = zahtjevDto.RezultatObrade!.Value,
            NapomenaObrade = zahtjevDto.NapomenaObrade,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = currentUserId,
            Clanovi = new List<SocijalniNatjecajClan>()
        };

        // 2. Dodaj podnositelja kao člana
        var podnositelj = new SocijalniNatjecajClan
        {
            Zahtjev = zahtjev,
            ImePrezime = imePrezime,
            Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        zahtjev.Clanovi.Add(podnositelj);

        // 3. Dodaj prazne podatke o kućanstvu
        zahtjev.KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci
        {
            Zahtjev = zahtjev
        };

        // 4. Dodaj prazne bodovne podatke
        zahtjev.BodovniPodaci = new SocijalniNatjecajBodovniPodaci
        {
            Zahtjev = zahtjev,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            EditedBy = currentUserId
        };

        // 5. Spremi sve
        await context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);
        await context.SaveChangesAsync();
    }

}
