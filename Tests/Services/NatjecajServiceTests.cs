using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class NatjecajServiceTests
{
    public NatjecajServiceTests()
    {
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Helpers.MappingExtensions).TypeHandle);
    }

    [Fact]
    public async Task GetByKlasaAsync_ReturnsMappedDto()
    {
        // Arrange
        var entity = new Natjecaj
        {
            Id = 1,
            Klasa = 123,
            ProsjekPlace = 50m,
            PriustiviIliSocijalni = 2,
            Zakljucen = 1,
            DatumObjave = DateOnly.FromDateTime(DateTime.Today),
            RokZaPrijavu = DateOnly.FromDateTime(DateTime.Today.AddDays(30))
        };

        var repo = new Mock<INatjecajRepository>();
        repo.Setup(r => r.GetByKlasaAsync(123)).ReturnsAsync(entity);

        var logger = new Mock<ILogger<NatjecajService>>();
        var service = new NatjecajService(repo.Object, logger.Object);

        // Act
        var result = await service.GetByKlasaAsync(123);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Klasa, result.Klasa);
        Assert.Equal(entity.ProsjekPlace, result.ProsjekPlace);
        Assert.Equal("Socijalni", result.Vrsta);
        Assert.Equal("Aktivan", result.Status);
    }

    [Fact]
    public async Task CreateAsync_AddsAndSaves()
    {
        // Arrange
        var dto = new NatjecajDto
        {
            Klasa = 55,
            ProsjekPlace = 1m,
            Vrsta = "Priuštivi",
            Status = "Aktivan",
            DatumObjave = DateOnly.FromDateTime(DateTime.Today),
            RokZaPrijavu = DateOnly.FromDateTime(DateTime.Today.AddDays(15))
        };

        var repo = new Mock<INatjecajRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<Natjecaj>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var logger = new Mock<ILogger<NatjecajService>>();
        var service = new NatjecajService(repo.Object, logger.Object);

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        Assert.True(result);
        repo.Verify(r => r.AddAsync(It.Is<Natjecaj>(n =>
            n.Klasa == dto.Klasa &&
            n.ProsjekPlace == dto.ProsjekPlace &&
            n.PriustiviIliSocijalni == 1 &&
            n.Zakljucen == 1
        )), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}