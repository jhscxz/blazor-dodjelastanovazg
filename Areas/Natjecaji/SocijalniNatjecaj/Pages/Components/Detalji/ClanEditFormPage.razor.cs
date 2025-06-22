using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class ClanEditFormPage : ComponentBase
{
    [Parameter] public long ZahtjevId { get; set; }
    [Parameter] public long? ClanId { get; set; }

    [Inject] private IUnitOfWork UnitOfWork { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private SocijalniNatjecajClanDto? _model;

    private List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", Url = "" },
        new() { Text = "Uredi člana", CssClass = "text-red-500 font-bold" }
    ];

    protected override async Task OnInitializedAsync()
    {
        SocijalniNatjecajZahtjevDto zahtjev;
        try
        {
            zahtjev = await UnitOfWork.SocijalniZahtjevRead.GetDetaljiAsync(ZahtjevId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
            Navigation.NavigateTo("/socijalni-natjecaj");
            return;
        }

        var clan = ClanId.HasValue
            ? zahtjev.Clanovi?.FirstOrDefault(c => c.Id == ClanId.Value)
            : null;

        _model = clan is null
            ? new SocijalniNatjecajClanDto { ZahtjevId = ZahtjevId }
            : new SocijalniNatjecajClanDto
            {
                Id = clan.Id,
                ImePrezime = clan.ImePrezime,
                Oib = clan.Oib,
                Srodstvo = clan.Srodstvo,
                DatumRodjenja = clan.DatumRodjenja,
                ZahtjevId = ZahtjevId
            };

        BreadcrumbItems[2].Url = $"/socijalni/detalji/{ZahtjevId}";
    }

    private async Task HandleSubmit(SocijalniNatjecajClanDto clan)
    {
        if (ClanId.HasValue)
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.UrediClanaIObradiAsync(clan);
        }
        else
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.DodajClanaIObradiAsync(ZahtjevId, clan);
        }

        Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Clanovi");
    }

    private void HandleCancel()
    {
        Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Clanovi");
    }
}