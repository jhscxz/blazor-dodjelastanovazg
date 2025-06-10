using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

namespace DodjelaStanovaZG.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        ISocijalniClanService SocijalniClanService { get; }
        ISocijalniKucanstvoService SocijalniKucanstvoService { get; }
        ISocijalniBodovniPodaciService SocijalniBodovniPodaciService { get; }
        INatjecajOdabirService NatjecajOdabirService { get; }
        INatjecajService NatjecajiService { get; }
        ISocijalniZahtjevService SocijalniZahtjevService { get; }
        Task SaveChangesAsync();
    }
}