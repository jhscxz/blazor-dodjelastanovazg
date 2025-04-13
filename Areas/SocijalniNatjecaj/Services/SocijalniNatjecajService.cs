using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
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

    public async Task<long> CreateAsync(SocijalniNatjecajZahtjevDto zahtjevDto, string imePrezime, string? oib)
    {
        string currentUserId = GetCurrentUserId();

        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = zahtjevDto.NatjecajId,
            KlasaPredmeta = zahtjevDto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = zahtjevDto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = zahtjevDto.Adresa,
            Email = zahtjevDto.Email,
            RezultatObrade = zahtjevDto.RezultatObrade!.Value,
            NapomenaObrade = zahtjevDto.NapomenaObrade,
            Clanovi = new List<SocijalniNatjecajClan>()
        };

        var podnositelj = new SocijalniNatjecajClan
        {
            Zahtjev = zahtjev,
            ImePrezime = imePrezime,
            Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva
        };
        zahtjev.Clanovi.Add(podnositelj);

        zahtjev.KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci
        {
            Zahtjev = zahtjev
        };

        zahtjev.BodovniPodaci = new SocijalniNatjecajBodovniPodaci
        {
            Zahtjev = zahtjev
        };

        // Audit
        AuditHelper.ApplyAudit(zahtjev, currentUserId, true);
        AuditHelper.ApplyAudit(podnositelj, currentUserId, true);
        AuditHelper.ApplyAudit(zahtjev.KucanstvoPodaci, currentUserId, true);
        AuditHelper.ApplyAudit(zahtjev.BodovniPodaci, currentUserId, true);

        await context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);
        await context.SaveChangesAsync();

        return zahtjev.Id;
    }
}
