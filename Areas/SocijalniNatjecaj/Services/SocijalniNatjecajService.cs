using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniNatjecajService : ISocijalniNatjecajService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SocijalniNatjecajService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";
    }

    public async Task<List<SocijalniNatjecajDto>> GetAllAsync()
    {
        return await _context.SocijalniNatjecajZahtjevi
            .Select(x => new SocijalniNatjecajDto
            {
                KlasaPredmeta = x.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
                Adresa = x.Adresa!,
                NatjecajId = x.NatjecajId
            })
            .ToListAsync();
    }

    public async Task CreateAsync(SocijalniNatjecajDto dto, string imePrezime, string? oib)
    {
        string currentUserId = GetCurrentUserId();

        // Kreiramo novi zahtjev s osnovnim podacima
        var entitet = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta.GetValueOrDefault(),
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva,
            Adresa = dto.Adresa,
            RezultatObrade = dto.RezultatObrade!.Value,
            NapomenaObrade = dto.NapomenaObrade,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = currentUserId, // Postavljamo CreatedBy na ID trenutno prijavljenog korisnika
            Clanovi = new List<SocijalniNatjecajClan>()
        };

        // Kreiramo prvog člana – podnositelja zahtjeva – koristeći podatke iz DTO-a
        var clan = new SocijalniNatjecajClan
        {
            Zahtjev = entitet,
            ImePrezime = imePrezime,
            Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Dodajemo podnositelja kao prvog člana u kolekciju Clanovi
        entitet.Clanovi.Add(clan);

        // Sprema se entitet s dodanim članom u bazu
        await _context.SocijalniNatjecajZahtjevi.AddAsync(entitet);
        await _context.SaveChangesAsync();
    }
}
