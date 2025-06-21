using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Infrastructure;

public class UnitOfWork(
    ApplicationDbContext context,
    ISocijalniClanService socijalniClanService,
    ISocijalniKucanstvoService socijalniKucanstvoService,
    ISocijalniBodovniPodaciService socijalniBodovniPodaciService,
    INatjecajOdabirService natjecajOdabirService,
    ISocijalniBodoviService socijalniBodoviService,
    ISocijalniBodovnaGreskaService socijalniBodovnaGreskaService,
    ISocijalniZahtjevReadService socijalniZahtjevReadService,
    ISocijalniZahtjevWriteService socijalniZahtjevWriteService,
    ISocijalniZahtjevProcessor socijalniZahtjevProcessor,
    INatjecajService natjecajiService)
    : IUnitOfWork
{
    public ISocijalniClanService SocijalniClanService { get; } = socijalniClanService;
    public ISocijalniKucanstvoService SocijalniKucanstvoService { get; } = socijalniKucanstvoService;
    public ISocijalniBodovniPodaciService SocijalniBodovniPodaciService { get; } = socijalniBodovniPodaciService;
    public INatjecajOdabirService NatjecajOdabirService { get; } = natjecajOdabirService;
    public INatjecajService NatjecajiService { get; } = natjecajiService;
    public ISocijalniBodoviService SocijalniBodoviService { get; } = socijalniBodoviService;
    public ISocijalniBodovnaGreskaService SocijalniBodovnaGreskaService { get; } = socijalniBodovnaGreskaService;
    public ISocijalniZahtjevReadService SocijalniZahtjevRead { get; } = socijalniZahtjevReadService;
    public ISocijalniZahtjevWriteService SocijalniZahtjevWrite { get; } = socijalniZahtjevWriteService;
    public ISocijalniZahtjevProcessor SocijalniZahtjevProcessor { get; } = socijalniZahtjevProcessor;

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}