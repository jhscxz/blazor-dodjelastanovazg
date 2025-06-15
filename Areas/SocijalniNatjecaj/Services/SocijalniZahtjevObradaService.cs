using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Helpers.IHelpers;
using DodjelaStanovaZG.Models;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniZahtjevObradaService(
    ApplicationDbContext context,
    ISocijalniKucanstvoService kucanstvoService,
    ISocijalniBodoviService bodoviService,
    ISocijalniBodovniPodaciService bodovniPodaciService,
    ISocijalniClanService clanService,
    ISocijalniBodovnaGreskaService greskaService,
    IUserContextService currentUserService)
    : ISocijalniZahtjevObradaService
{
    // Pomoćne metode za audit i transakcije
    private void ApplyAudit(object entity, bool isCreate) =>
        AuditHelper.ApplyAudit(entity, currentUserService.GetCurrentUserId(), isCreate);

    private async Task ApplyAuditAsync(long zahtjevId)
    {
        var z = await context.SocijalniNatjecajZahtjevi.FindAsync(zahtjevId);
        if (z != null) ApplyAudit(z, false);
    }

    private async Task ObradiAsync(
        long zahtjevId,
        object? entity,
        bool isCreate = false,
        Func<Task>? dodatnaAkcija = null)
    {
        await using var tx = await context.Database.BeginTransactionAsync();

        if (entity != null) ApplyAudit(entity, isCreate);
        await ApplyAuditAsync(zahtjevId);
        await context.SaveChangesAsync();

        if (dodatnaAkcija != null) 
            await dodatnaAkcija();

        await tx.CommitAsync();
    }

    // 1) Kreiranje novog zahtjeva
    public async Task<SocijalniNatjecajZahtjev> KreirajZahtjevAsync(
        SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        await using var tx = await context.Database.BeginTransactionAsync();

        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = dto.Adresa,
            Email = dto.Email,
            RezultatObrade = dto.RezultatObrade!.Value,
            ManualniRezultatObrade = dto.RezultatObrade!.Value, // inicijaliziramo manualni
            NapomenaObrade = dto.NapomenaObrade
        };

        var podnositelj = new SocijalniNatjecajClan
        {
            ImePrezime = imePrezime!,
            Oib = string.IsNullOrWhiteSpace(oib)
                ? null
                : oib,
            Srodstvo = Srodstvo.PodnositeljZahtjeva,
            Zahtjev = null
        };

        var kucanstvo = new SocijalniNatjecajKucanstvoPodaci();
        var bodovni = new SocijalniNatjecajBodovniPodaci();
        var bodovi = new SocijalniNatjecajBodovi();

        // Sastavimo object graph i dodamo u kontekst
        zahtjev.Clanovi = new List<SocijalniNatjecajClan> { podnositelj };
        zahtjev.KucanstvoPodaci = kucanstvo;
        zahtjev.BodovniPodaci = bodovni;
        zahtjev.Bodovi = bodovi;

        await context.SocijalniNatjecajZahtjevi.AddAsync(zahtjev);
        ApplyAudit(zahtjev, true);
        ApplyAudit(podnositelj, true);
        ApplyAudit(kucanstvo, true);
        ApplyAudit(bodovni, true);
        ApplyAudit(bodovi, true);

        // Spremimo da dobijemo Id za kucanstvo
        await context.SaveChangesAsync();

        // Kreiramo pripadajući zapis o prihodima (shared PK)
        var prihod = new SocijalniPrihodi
        {
            Id = kucanstvo.Id,
            UkupniPrihodKucanstva = 0,
            PrihodPoClanu = 0,
            IspunjavaUvjetPrihoda = true
        };
        await context.SocijalniPrihodi.AddAsync(prihod);
        ApplyAudit(prihod, true);
        await context.SaveChangesAsync();

        // Izračunaj bodove i greške
        await bodoviService.IzracunajIBodujAsync(zahtjev.Id);
        await ObradiGreskeAsync(zahtjev.Id);

        await context.SaveChangesAsync();
        await tx.CommitAsync();

        return zahtjev;
    }

    // 2) Uređivanje kućanstva
    public async Task SpremiKucanstvoIObracunajAsync(
        long zahtjevId,
        SocijalniKucanstvoPodaciDto dto)
    {
        // Update kucanstva i prihoda
        await kucanstvoService.UpdateKucanstvoPodaciAsync(zahtjevId, dto);

        // Osiguraj da je prihod “attachan” u EF Core
        var kuc = await context.SocijalniNatjecajKucanstvoPodaci
            .Include(k => k.Prihod)
            .FirstAsync(k => k.ZahtjevId == zahtjevId);

        if (kuc.Prihod != null)
            context.Entry(kuc.Prihod).State = EntityState.Modified;

        await ObradiAsync(
            zahtjevId,
            kuc,
            isCreate: false,
            dodatnaAkcija: async () =>
            {
                await bodoviService.IzracunajIBodujAsync(zahtjevId);
                await ObradiGreskeAsync(zahtjevId);
            });
    }

    // 3) Obrada grešaka i vraćanje manualnog rezultata
    public async Task ObradiGreskeAsync(long zahtjevId)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi
            .Include(z => z.KucanstvoPodaci)
            .Include(z => z.Clanovi)
            .Include(z => z.BodovniPodaci)
            .FirstAsync(z => z.Id == zahtjevId);

        var greske = await greskaService.PronadiGreskeAsync(zahtjev);

        // Zamijeni stare greške novima
        var stare = await context.SocijalniNatjecajBodovnaGreske
            .Where(g => g.ZahtjevId == zahtjevId)
            .ToListAsync();
        context.SocijalniNatjecajBodovnaGreske.RemoveRange(stare);
        if (greske.Count > 0)
            await context.SocijalniNatjecajBodovnaGreske.AddRangeAsync(greske);

        // Postavi RezultatObrade
        if (greske.Any())
        {
            zahtjev.RezultatObrade = RezultatObrade.Greška;
        }
        else
        {
            // vraćamo točno ono što je referent zadnji unio
            zahtjev.RezultatObrade = zahtjev.ManualniRezultatObrade;
        }

        ApplyAudit(zahtjev, isCreate: false);
        await context.SaveChangesAsync();
    }

    // 4) Uređivanje bodovnih podataka
    public async Task SpremiBodovnePodatkeIObracunajAsync(
        long zahtjevId,
        SocijalniNatjecajBodovniPodaciDto dto)
    {
        await bodovniPodaciService.UpdateAsync(zahtjevId, dto);

        var bodovni = await context.SocijalniNatjecajBodovniPodaci
            .FirstAsync(x => x.ZahtjevId == zahtjevId);

        await ObradiAsync(
            zahtjevId,
            bodovni,
            isCreate: false,
            dodatnaAkcija: async () =>
            {
                await bodoviService.IzracunajIBodujAsync(zahtjevId);
                await ObradiGreskeAsync(zahtjevId);
            });
    }

    // 5) Dodavanje člana
    public async Task DodajClanaIObracunajAsync(
        long zahtjevId,
        SocijalniNatjecajClanDto clanDto)
    {
        var novi = clanDto.ToEntity(zahtjevId);
        await clanService.AddClanAsync(novi);

        // Nakon dodavanja, samo proslijedi u ObradiAsync
        await ObradiAsync(
            zahtjevId,
            novi,
            isCreate: true,
            dodatnaAkcija: async () =>
            {
                await bodoviService.IzracunajIBodujAsync(zahtjevId);
                await ObradiGreskeAsync(zahtjevId);
            });
    }

    // 6) Brisanje člana
    public async Task ObrisiClanaIObracunajAsync(long zahtjevId, long clanId)
    {
        await clanService.RemoveClanAsync(zahtjevId, clanId);

        // entity za audit možemo dohvatiti prije ili poslije brisanja
        await ObradiAsync(
            zahtjevId,
            entity: null,
            isCreate: false,
            dodatnaAkcija: async () =>
            {
                await bodoviService.IzracunajIBodujAsync(zahtjevId);
                await ObradiGreskeAsync(zahtjevId);
            });
    }

    // 7) Edit postojećeg člana
    public async Task EditClanIObracunajAsync(SocijalniNatjecajClanDto azurirani)
    {
        var clan = await context.SocijalniNatjecajClanovi
            .FirstAsync(c => c.Id == azurirani.Id);
        azurirani.MapOnto(clan);

        await ObradiAsync(
            azurirani.ZahtjevId,
            clan,
            isCreate: false,
            dodatnaAkcija: async () =>
            {
                await bodoviService.IzracunajIBodujAsync(azurirani.ZahtjevId);
                await ObradiGreskeAsync(azurirani.ZahtjevId);
            });
    }

    // 8) Ažuriranje osnovnih podataka
    public async Task AzurirajOsnovnoIObracunajAkoTrebaAsync(
        long zahtjevId,
        SocijalniNatjecajOsnovnoEditDto dto)
    {
        var zahtjev = await context.SocijalniNatjecajZahtjevi
            .FirstAsync(z => z.Id == zahtjevId);

        dto.MapOnto(zahtjev);
        // Čuvamo referentov izbor
        zahtjev.ManualniRezultatObrade = dto.RezultatObrade!.Value;

        await ObradiAsync(
            zahtjevId,
            zahtjev,
            isCreate: false,
            dodatnaAkcija: async () =>
            {
                // Ako je referent izabrao Zadovoljava, preracunaj bodove
                if (dto.RezultatObrade == RezultatObrade.Osnovan)
                    await bodoviService.IzracunajIBodujAsync(zahtjevId);
                await ObradiGreskeAsync(zahtjevId);
            });
    }
}
