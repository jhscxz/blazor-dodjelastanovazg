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
        ISocijalniNatjecajService socijalniNatjecajService,
        ISocijalniNatjecajDetaljiService socijalniNatjecajDetaljiService,
        ISocijalniClanService socijalniClanService,
        ISocijalniKucanstvoService socijalniKucanstvoService,
        ISocijalniBodovniPodaciService socijalniBodovniPodaciService,
        INatjecajOdabirService natjecajOdabirService,
        INatjecajService natjecajiService)
    {
        _context = context;
        SocijalniNatjecajService = socijalniNatjecajService;
        SocijalniNatjecajDetaljiService = socijalniNatjecajDetaljiService;
        SocijalniClanService = socijalniClanService;
        SocijalniKucanstvoService = socijalniKucanstvoService;
        SocijalniBodovniPodaciService = socijalniBodovniPodaciService;
        NatjecajOdabirService = natjecajOdabirService;
        NatjecajiService = natjecajiService;
    }

    public ISocijalniNatjecajService SocijalniNatjecajService { get; }
    public ISocijalniNatjecajDetaljiService SocijalniNatjecajDetaljiService { get; }
    public ISocijalniClanService SocijalniClanService { get; }
    public ISocijalniKucanstvoService SocijalniKucanstvoService { get; }
    public ISocijalniBodovniPodaciService SocijalniBodovniPodaciService { get; }
    public INatjecajOdabirService NatjecajOdabirService { get; }
    public INatjecajService NatjecajiService { get; }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}