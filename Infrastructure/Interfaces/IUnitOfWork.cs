using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        ISocijalniClanService SocijalniClanService { get; }
        ISocijalniKucanstvoService SocijalniKucanstvoService { get; }
        ISocijalniBodovniPodaciService SocijalniBodovniPodaciService { get; }
        INatjecajOdabirService NatjecajOdabirService { get; }
        INatjecajService NatjecajiService { get; }
        ISocijalniBodoviService SocijalniBodoviService { get; }
        ISocijalniBodovnaGreskaService SocijalniBodovnaGreskaService { get; }
        ISocijalniZahtjevReadService SocijalniZahtjevRead { get; }
        ISocijalniZahtjevWriteService SocijalniZahtjevWrite { get; }
        ISocijalniZahtjevProcessorService SocijalniZahtjevProcessorService { get; }
    }
}