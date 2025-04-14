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
        INatjecajOdabirService natjecajOdabirService)
    {
        _context = context;
        SocijalniNatjecajService = socijalniNatjecajService;
        SocijalniNatjecajDetaljiService = socijalniNatjecajDetaljiService;
        NatjecajOdabirService = natjecajOdabirService;
    }

    public ISocijalniNatjecajService SocijalniNatjecajService { get; }
    public ISocijalniNatjecajDetaljiService SocijalniNatjecajDetaljiService { get; }
    public INatjecajOdabirService NatjecajOdabirService { get; }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}