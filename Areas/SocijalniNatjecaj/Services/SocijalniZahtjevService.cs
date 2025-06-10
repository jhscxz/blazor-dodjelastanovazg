using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniZahtjevService : ISocijalniZahtjevService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SocijalniZahtjevService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private string CurrentUserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new Exception("Korisnik nije prijavljen.");

    public Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync() =>
        _context.SocijalniNatjecajZahtjevi
            .AsNoTracking()
            .Select(x => new SocijalniNatjecajZahtjevDto
            {
                Id = x.Id,
                KlasaPredmeta = x.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
                Adresa = x.Adresa!,
                NatjecajId = x.NatjecajId
            })
            .ToListAsync();

    public async Task<SocijalniNatjecajZahtjevDto> GetByIdAsync(long id)
{
    var entity = await _context.SocijalniNatjecajZahtjevi
        .Include(x => x.Clanovi).ThenInclude(c => c.CreatedByUser)
        .Include(x => x.Clanovi).ThenInclude(c => c.UpdatedByUser)
        .Include(x => x.BodovniPodaci)
        .Include(x => x.KucanstvoPodaci).ThenInclude(k => k!.CreatedByUser)
        .Include(x => x.KucanstvoPodaci).ThenInclude(k => k!.UpdatedByUser)
        .Include(x => x.CreatedByUser)
        .Include(x => x.UpdatedByUser)
        .FirstOrDefaultAsync(x => x.Id == id)
        ?? throw new Exception("Zahtjev nije pronađen.");

    var podnositelj = entity.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);

    return new SocijalniNatjecajZahtjevDto
    {
        Id = entity.Id,
        NatjecajId = entity.NatjecajId,
        KlasaPredmeta = entity.KlasaPredmeta,
        DatumPodnosenjaZahtjeva = entity.DatumPodnosenjaZahtjeva,
        Adresa = entity.Adresa,
        Email = entity.Email,
        ImePrezime = podnositelj?.ImePrezime ?? string.Empty,
        Oib = podnositelj?.Oib,
        RezultatObrade = entity.RezultatObrade,
        NapomenaObrade = entity.NapomenaObrade,

        // Podaci o kućanstvu
        KucanstvoPodaci = entity.KucanstvoPodaci is not null
            ? new SocijalniKucanstvoPodaciDto
            {
                UkupniPrihodKucanstva = entity.KucanstvoPodaci.UkupniPrihodKucanstva,
                PrebivanjeOd = entity.KucanstvoPodaci.PrebivanjeOd,
                StambeniStatusKucanstva = entity.KucanstvoPodaci.StambeniStatusKucanstva,
                SastavKucanstva = entity.KucanstvoPodaci.SastavKucanstva,
                ZahtjevId = entity.Id
            }.WithAuditFrom(entity.KucanstvoPodaci)
            : null,

        // Članovi
        Clanovi = entity.Clanovi.Select(clan => new SocijalniNatjecajClanDto
        {
            Id = clan.Id,
            ZahtjevId = entity.Id,
            ImePrezime = clan.ImePrezime,
            Oib = clan.Oib,
            Srodstvo = clan.Srodstvo,
            DatumRodjenja = clan.DatumRodjenja
        }.WithAuditFrom(clan)).ToList(),

        // Prazni bodovni podaci (placeholder)
        Bodovni = new SocijalniBodovniDto()
    }.WithAuditFrom(entity);
}

    public async Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = dto.Adresa,
            Email = dto.Email,
            RezultatObrade = dto.RezultatObrade!.Value,
            NapomenaObrade = dto.NapomenaObrade
        };

        var podnositelj = new SocijalniNatjecajClan
        {
            Zahtjev = zahtjev,
            ImePrezime = imePrezime,
            Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva
        };

        zahtjev.Clanovi = [podnositelj];
        zahtjev.KucanstvoPodaci = new() { Zahtjev = zahtjev };
        zahtjev.BodovniPodaci = new() { Zahtjev = zahtjev };

        foreach (var entitet in new AuditableEntity[] { zahtjev, podnositelj, zahtjev.KucanstvoPodaci, zahtjev.BodovniPodaci })
        {
            AuditHelper.ApplyAudit(entitet, CurrentUserId, isCreate: true);
        }

        await _context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);
        return zahtjev;
    }

    public async Task UpdateOsnovniPodaciAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await _context.SocijalniNatjecajZahtjevi
            .FirstOrDefaultAsync(z => z.Id == zahtjevId)
            ?? throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

        zahtjev.KlasaPredmeta = dto.KlasaPredmeta ?? 0;
        zahtjev.Adresa = dto.Adresa;
        zahtjev.Email = dto.Email;
        zahtjev.DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva!.Value;
        zahtjev.NapomenaObrade = dto.NapomenaObrade;
        zahtjev.RezultatObrade = dto.RezultatObrade ?? RezultatObrade.Nepotpun;

        AuditHelper.ApplyAudit(zahtjev, CurrentUserId, false);
    }

    public async Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId)
    {
        return await _context.SocijalniNatjecajZahtjevi
                   .Include(z => z.Clanovi)
                   .Include(z => z.Natjecaj)
                   .Include(z => z.BodovniPodaci)
                   .Include(z => z.KucanstvoPodaci)
                   .FirstOrDefaultAsync(z => z.Id == zahtjevId)
               ?? throw new Exception($"Zahtjev s ID-om {zahtjevId} nije pronađen.");
    }


}
