using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class ClanoviKucanstvaTab : ComponentBase
{
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IUnitOfWork UnitOfWork { get; set; } = null!;
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Parameter] public long Id { get; set; }

    private SocijalniNatjecajZahtjevDto Zahtjev { get; set; } = null!;
    protected List<SocijalniNatjecajClanDto>? Clanovi { get; set; } = [];
    protected long? DeletePendingId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Zahtjev = await UnitOfWork.SocijalniZahtjevRead.GetDetaljiAsync(Id);
        Clanovi = Zahtjev.Clanovi;
    }

    protected void NavigateAddClan()
    {
        Navigation.NavigateTo($"/socijalni/clan/edit/{Id}");
    }

    protected void NavigateEditClan(long clanId)
    {
        Navigation.NavigateTo($"/socijalni/clan/edit/{Id}/{clanId}");
    }

    protected void PromptDeleteClan(long id)
    {
        DeletePendingId = id;
    }

    protected async Task ConfirmDeleteClanAsync()
    {
        if (DeletePendingId is null)
            return;

        try
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.ObrisiClanaIObradiAsync(Id, DeletePendingId.Value);
            Clanovi?.RemoveAll(c => c.Id == DeletePendingId.Value);
            Snackbar.Add("Član kućanstva je obrisan.", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
        }

        DeletePendingId = null;
    }

    protected void CancelDelete()
    {
        DeletePendingId = null;
    }
}
