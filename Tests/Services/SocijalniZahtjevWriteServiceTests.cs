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
    private class ConcurrencyFailDbContext : ApplicationDbContext
    {
        public byte[]? OriginalRowVersion { get; private set; }
        public ConcurrencyFailDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
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
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using (var seed = new ApplicationDbContext(options))
        {
            var natjecaj = new Natjecaj { Id = 1 };
            var zahtjev = new SocijalniNatjecajZahtjev
            {
                Id = 1,
                KlasaPredmeta = 1,
                NatjecajId = natjecaj.Id,
                Natjecaj = natjecaj
            };
            seed.Natjecaji.Add(natjecaj);
            seed.SocijalniNatjecajZahtjevi.Add(zahtjev);
            await seed.SaveChangesAsync();
        }

        var ctx = new ConcurrencyFailDbContext(options);
        var factoryMock = new Mock<IDbContextFactory<ApplicationDbContext>>();
        factoryMock.Setup(f => f.CreateDbContext()).Returns(ctx);

        var audit = new Mock<IAuditService>();
        var logger = new Mock<ILogger<SocijalniZahtjevWriteService>>();
        var service = new SocijalniZahtjevWriteService(factoryMock.Object, audit.Object, logger.Object);

        var dto = new SocijalniNatjecajOsnovnoEditDto
        {
            Id = 1,
            KlasaPredmeta = 2,
            DatumPodnosenjaZahtjeva = DateTime.Now,
            NatjecajId = 1,
            RowVersion = new byte[] { 5 }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateOsnovnoAsync(1, dto));
        Assert.Equal(dto.RowVersion, ctx.OriginalRowVersion);
    }
}