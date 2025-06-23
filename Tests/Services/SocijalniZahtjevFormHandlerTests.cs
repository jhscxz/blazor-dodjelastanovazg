using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Models;
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
    public async Task SubmitAsync_DateOutsideRange_AllowsCreation()
    {
        var natjecajDto = new NatjecajDto
        {
            DatumObjave = new DateOnly(2024, 1, 1),
            RokZaPrijavu = new DateOnly(2024, 1, 31)
        };

        var natjecajService = Mock.Of<INatjecajService>(s =>
            s.GetByIdAsync(1) == Task.FromResult(natjecajDto));

        var processor = new Mock<ISocijalniZahtjevProcessorService>();
        processor.Setup(p => p.KreirajZahtjevAsync(It.IsAny<SocijalniNatjecajZahtjevDto>(), null, null))
            .ReturnsAsync(new SocijalniNatjecajZahtjev { Id = 5 });

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(u => u.NatjecajiService).Returns(natjecajService);
        uow.SetupGet(u => u.SocijalniZahtjevProcessorService).Returns(processor.Object);

        var nav = new TestNavigationManager();
        var handler = new SocijalniZahtjevFormHandler(
            uow.Object,
            Mock.Of<ISocijalniBodoviService>(), 
            nav,
            Mock.Of<ILogger<SocijalniZahtjevFormHandler>>()
        );

        var model = new SocijalniNatjecajZahtjevDto
        {
            NatjecajId = 1,
            DatumPodnosenjaZahtjeva = new DateTime(2023, 12, 31),
            ImePrezime = "Test"
        };

        var result = await handler.SubmitAsync(model, (int)RezultatObrade.Osnovan);

        Assert.False(result.Success);
    }

}