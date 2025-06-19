using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Helpers.IServices;

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
        ISocijalniZahtjevProcessor SocijalniZahtjevProcessor { get; }



        Task SaveChangesAsync();
    }
}