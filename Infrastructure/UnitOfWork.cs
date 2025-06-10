using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;

namespace DodjelaStanovaZG.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(
        ApplicationDbContext context,
        ISocijalniClanService socijalniClanService,
        ISocijalniKucanstvoService socijalniKucanstvoService,
        ISocijalniBodovniPodaciService socijalniBodovniPodaciService,
        INatjecajOdabirService natjecajOdabirService,
        ISocijalniZahtjevService socijalniZahtjevService,
        INatjecajService natjecajiService)
    {
        _context = context;
        SocijalniClanService = socijalniClanService;
        SocijalniKucanstvoService = socijalniKucanstvoService;
        SocijalniBodovniPodaciService = socijalniBodovniPodaciService;
        NatjecajOdabirService = natjecajOdabirService;
        SocijalniZahtjevService = socijalniZahtjevService;
        NatjecajiService = natjecajiService;
    }

    public ISocijalniClanService SocijalniClanService { get; }
    public ISocijalniKucanstvoService SocijalniKucanstvoService { get; }
    public ISocijalniBodovniPodaciService SocijalniBodovniPodaciService { get; }
    public INatjecajOdabirService NatjecajOdabirService { get; }
    public INatjecajService NatjecajiService { get; }
    public ISocijalniZahtjevService SocijalniZahtjevService { get; }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}