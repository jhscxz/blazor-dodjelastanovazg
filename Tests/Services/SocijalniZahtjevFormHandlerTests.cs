using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class SocijalniZahtjevFormHandlerTests
{
    private class TestNavigationManager : NavigationManager
    {
        public string? NavigatedTo;
        public TestNavigationManager() => Initialize("http://localhost/", "http://localhost/");
        protected override void NavigateToCore(string uri, bool forceLoad) => NavigatedTo = uri;
    }

    [Fact]
    public async Task SubmitAsync_DateOutsideRange_ReturnsError()
    {
        // Arrange
        var natjecajDto = new NatjecajDto
        {
            DatumObjave = new DateOnly(2024, 1, 1),
            RokZaPrijavu = new DateOnly(2024, 1, 31)
        };

        var natjecajService = new Mock<INatjecajService>();
        natjecajService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(natjecajDto);

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(u => u.NatjecajiService).Returns(natjecajService.Object);
        uow.SetupGet(u => u.SocijalniZahtjevProcessorService).Returns(Mock.Of<ISocijalniZahtjevProcessorService>());

        var bodoviService = new Mock<ISocijalniBodoviService>();
        var nav = new TestNavigationManager();
        var logger = new Mock<ILogger<SocijalniZahtjevFormHandler>>();
        var handler = new SocijalniZahtjevFormHandler(uow.Object, bodoviService.Object, nav, logger.Object);

        var model = new SocijalniNatjecajZahtjevDto
        {
            NatjecajId = 1,
            DatumPodnosenjaZahtjeva = new DateTime(2023, 12, 31),
            ImePrezime = "Test"
        };

        // Act
        var result = await handler.SubmitAsync(model, (int)RezultatObrade.Osnovan);

        // Assert
        Assert.False(result.Success);
        Assert.Single(result.Errors);
        Assert.Contains("Datum podnošenja", result.Errors[0]);
    }
}