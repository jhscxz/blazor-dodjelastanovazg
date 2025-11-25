using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class SocijalniZahtjevWriteServiceTests
{
    private class ConcurrencyFailDbContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options)
    {
        public byte[]? OriginalRowVersion { get; private set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entry = ChangeTracker.Entries<SocijalniNatjecajZahtjev>().First();
            OriginalRowVersion = (byte[]?)entry.Property(z => z.RowVersion).OriginalValue;
            throw new DbUpdateConcurrencyException();
        }
    }

    [Fact]
    public async Task UpdateOsnovnoAsync_ConcurrencyConflict_ThrowsInvalidOperation()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Seed
        await using (var seed = new ApplicationDbContext(options))
        {
            seed.Natjecaji.Add(new Natjecaj { Id = 1 });
            seed.SocijalniNatjecajZahtjevi.Add(new SocijalniNatjecajZahtjev
            {
                Id = 1,
                KlasaPredmeta = 1,
                NatjecajId = 1
            });
            await seed.SaveChangesAsync();
        }

        var ctx = new ConcurrencyFailDbContext(options);
        var factory = Mock.Of<IDbContextFactory<ApplicationDbContext>>(f => f.CreateDbContext() == ctx);
        var audit = Mock.Of<IAuditService>();
        var logger = Mock.Of<ILogger<SocijalniZahtjevWriteService>>();

        var service = new SocijalniZahtjevWriteService(factory, audit, logger);

        var dto = new SocijalniNatjecajOsnovnoEditDto
        {
            Id = 1,
            KlasaPredmeta = 2,
            DatumPodnosenjaZahtjeva = DateTime.Now,
            NatjecajId = 1,
            RowVersion = [5]
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateOsnovnoAsync(1, dto));
        Assert.Equal(dto.RowVersion, ctx.OriginalRowVersion);
    }

}