using System.Security.Claims;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

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

        private IQueryable<SocijalniNatjecajZahtjev> BaseQuery(bool asNoTracking = false)
        {
            var query = _context.SocijalniNatjecajZahtjevi
                .Include(x => x.Clanovi).ThenInclude(c => c.CreatedByUser)
                .Include(x => x.Clanovi).ThenInclude(c => c.UpdatedByUser)
                .Include(x => x.BodovniPodaci)
                .Include(x => x.KucanstvoPodaci).ThenInclude(k => k!.CreatedByUser)
                .Include(x => x.KucanstvoPodaci).ThenInclude(k => k!.UpdatedByUser)
                .Include(x => x.CreatedByUser)
                .Include(x => x.UpdatedByUser);

            return asNoTracking ? query.AsNoTracking() : query;
        }

        public async Task<SocijalniNatjecajZahtjevDto> GetDetaljiAsync(long id)
        {
            var entity = await BaseQuery(asNoTracking: true)
                             .FirstOrDefaultAsync(x => x.Id == id)
                         ?? throw new NotFoundException($"Zahtjev s ID-om {id} nije pronađen.");

            return entity.ToDto();
        }

        public Task<List<SocijalniNatjecajZahtjevDto>> GetAllAsync() =>
            BaseQuery(asNoTracking: true)
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
            var entity = await BaseQuery(asNoTracking: true)
                             .FirstOrDefaultAsync(x => x.Id == id)
                         ?? throw new NotFoundException($"Zahtjev s ID-om {id} nije pronađen.");

            return entity.ToDto();
        }

        public async Task<SocijalniNatjecajZahtjev> CreateAsync(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
        {
            // 1. Kreiraj entitete bez veza
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
                ImePrezime = imePrezime,
                Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
                Srodstvo = Srodstvo.PodnositeljZahtjeva,
                Zahtjev = zahtjev
            };

            var kucanstvo = new SocijalniNatjecajKucanstvoPodaci();
            var bodovni = new SocijalniNatjecajBodovniPodaci();

            // 2. Primijeni audit PRIJE nego povežeš entitete
            AuditHelper.ApplyAudit(
                new AuditableEntity[] { zahtjev, podnositelj, kucanstvo, bodovni },
                CurrentUserId, isCreate: true);

            // 3. Poveži entitete
            zahtjev.Clanovi = new List<SocijalniNatjecajClan> { podnositelj };
            zahtjev.KucanstvoPodaci = kucanstvo;
            zahtjev.BodovniPodaci = bodovni;

            podnositelj.Zahtjev = zahtjev;
            kucanstvo.Zahtjev = zahtjev;
            bodovni.Zahtjev = zahtjev;

            // 4. Dodaj u kontekst
            await _context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);

            return zahtjev;
        }

        public async Task UpdateOsnovniPodaciAsync(long zahtjevId, SocijalniNatjecajOsnovnoEditDto dto)
        {
            var zahtjev = await _context.SocijalniNatjecajZahtjevi
                              .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                          ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");

            zahtjev.KlasaPredmeta = dto.KlasaPredmeta ?? 0;
            zahtjev.Adresa = dto.Adresa;
            zahtjev.Email = dto.Email;
            zahtjev.DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva!.Value;
            zahtjev.NapomenaObrade = dto.NapomenaObrade;
            zahtjev.RezultatObrade = dto.RezultatObrade ?? RezultatObrade.Nepotpun;

            AuditHelper.ApplyAudit(zahtjev, CurrentUserId, isCreate: false);
        }

        public async Task<SocijalniNatjecajZahtjev> GetZahtjevByIdAsync(long zahtjevId)
        {
            return await BaseQuery(asNoTracking: true)
                       .Include(z => z.Natjecaj)
                       .FirstOrDefaultAsync(z => z.Id == zahtjevId)
                   ?? throw new NotFoundException($"Zahtjev s ID-om {zahtjevId} nije pronađen.");
        }

        public SocijalniNatjecajClan ConvertToEntity(SocijalniNatjecajClanDto clanDto, SocijalniNatjecajZahtjev zahtjev)
            => new()
            {
                ImePrezime = clanDto.ImePrezime,
                Oib = clanDto.Oib,
                Srodstvo = clanDto.Srodstvo,
                DatumRodjenja = clanDto.DatumRodjenja,
                ZahtjevId = zahtjev.Id,
                Zahtjev = zahtjev
            };
        
        public async Task<PagedResult<SocijalniNatjecajZahtjevDto>> GetPagedAsync(
            long natjecajId,
            int page,
            int pageSize,
            string? sortBy,
            SortDirection sortDirection)
        {
            var query = BaseQuery(asNoTracking: true)
                .Where(x => x.NatjecajId == natjecajId);

            // Primijeni sortiranje (ako postoji labela)
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = query.OrderByDynamic(sortBy, sortDirection == SortDirection.Descending);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(x => new SocijalniNatjecajZahtjevDto
                {
                    Id = x.Id,
                    KlasaPredmeta = x.KlasaPredmeta,
                    DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
                    Adresa = x.Adresa!,
                    NatjecajId = x.NatjecajId,
                    Bodovni = x.KucanstvoPodaci == null ? null : new SocijalniBodovniDto()
                    {
                        UkupniPrihodKucanstva = x.KucanstvoPodaci.UkupniPrihodKucanstva,
                        StambeniStatusKucanstva = x.KucanstvoPodaci.StambeniStatusKucanstva,
                        SastavKucanstva = x.KucanstvoPodaci.SastavKucanstva
                    }
                })
                .ToListAsync();

            return new PagedResult<SocijalniNatjecajZahtjevDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

    }
}