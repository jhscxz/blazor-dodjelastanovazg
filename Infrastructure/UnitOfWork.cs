using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Infrastructure.Interfaces;

namespace DodjelaStanovaZG.Infrastructure;

public class UnitOfWork(
    ApplicationDbContext context,
    ISocijalniClanService socijalniClanService,
    ISocijalniKucanstvoService socijalniKucanstvoService,
    ISocijalniBodovniPodaciService socijalniBodovniPodaciService,
    INatjecajOdabirService natjecajOdabirService,
    ISocijalniZahtjevService socijalniZahtjevService,
    INatjecajService natjecajiService)
    : IUnitOfWork
{
    public ISocijalniClanService SocijalniClanService { get; } = socijalniClanService;
    public ISocijalniKucanstvoService SocijalniKucanstvoService { get; } = socijalniKucanstvoService;
    public ISocijalniBodovniPodaciService SocijalniBodovniPodaciService { get; } = socijalniBodovniPodaciService;
    public INatjecajOdabirService NatjecajOdabirService { get; } = natjecajOdabirService;
    public INatjecajService NatjecajiService { get; } = natjecajiService;
    public ISocijalniZahtjevService SocijalniZahtjevService { get; } = socijalniZahtjevService;

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}