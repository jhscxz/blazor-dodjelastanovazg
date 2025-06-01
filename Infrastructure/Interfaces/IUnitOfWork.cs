using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

namespace DodjelaStanovaZG.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        ISocijalniNatjecajService SocijalniNatjecajService { get; }
        ISocijalniNatjecajDetaljiService SocijalniNatjecajDetaljiService { get; }
        INatjecajOdabirService NatjecajOdabirService { get; }
        INatjecajService NatjecajiService { get; }
        Task SaveChangesAsync();
    }
}