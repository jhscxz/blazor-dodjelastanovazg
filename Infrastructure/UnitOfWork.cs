using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Infrastructure;

public class UnitOfWork(
    ISocijalniClanService socijalniClanService,
    ISocijalniKucanstvoService socijalniKucanstvoService,
    ISocijalniBodovniPodaciService socijalniBodovniPodaciService,
    INatjecajOdabirService natjecajOdabirService,
    ISocijalniBodoviService socijalniBodoviService,
    ISocijalniBodovnaGreskaService socijalniBodovnaGreskaService,
    ISocijalniZahtjevReadService socijalniZahtjevReadService,
    ISocijalniZahtjevWriteService socijalniZahtjevWriteService,
    ISocijalniZahtjevProcessorService socijalniZahtjevProcessorService,
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
    public ISocijalniZahtjevProcessorService SocijalniZahtjevProcessorService { get; } = socijalniZahtjevProcessorService;
}