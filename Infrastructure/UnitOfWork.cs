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
        INatjecajOdabirService natjecajOdabirService, INatjecajService natjecajiService)
    {
        _context = context;
        SocijalniNatjecajService = socijalniNatjecajService;
        SocijalniNatjecajDetaljiService = socijalniNatjecajDetaljiService;
        NatjecajOdabirService = natjecajOdabirService;
        NatjecajiService = natjecajiService;
    }

    public ISocijalniNatjecajService SocijalniNatjecajService { get; }
    public ISocijalniNatjecajDetaljiService SocijalniNatjecajDetaljiService { get; }

    public INatjecajOdabirService NatjecajOdabirService { get; }
    public INatjecajService NatjecajiService { get; }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}