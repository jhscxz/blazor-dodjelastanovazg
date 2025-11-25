using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class KucanstvoEditFormPage
{
    [Parameter] public long ZahtjevId { get; set; }
    [Inject] private IUnitOfWork UnitOfWork { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    private SocijalniKucanstvoPodaciDto? _kucanstvoModel;
    private MudForm _form = null!;
    private DateTime? _prebivanjeOd;
    private List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbItems = BreadcrumbProvider.ZahtjevEdit(ZahtjevId, "Uredi kućanstvo");
        
        var zahtjev = await UnitOfWork.SocijalniZahtjevRead.GetZahtjevByIdAsync(ZahtjevId);

        _kucanstvoModel = zahtjev?.KucanstvoPodaci is null
            ? new SocijalniKucanstvoPodaciDto()
            : new SocijalniKucanstvoPodaciDto
            {
                
                Prihod = zahtjev.KucanstvoPodaci?.Prihod is null
                    ? null
                    : new SocijalniPrihodDto
                    {
                        UkupniPrihodKucanstva = zahtjev.KucanstvoPodaci.Prihod.UkupniPrihodKucanstva,
                        PrihodPoClanu = zahtjev.KucanstvoPodaci.Prihod.PrihodPoClanu,
                        PostotakProsjeka = zahtjev.KucanstvoPodaci.Prihod.PostotakProsjeka,
                        IspunjavaUvjetPrihoda = zahtjev.KucanstvoPodaci.Prihod.IspunjavaUvjetPrihoda
                    },
                PrebivanjeOd = zahtjev.KucanstvoPodaci?.PrebivanjeOd,
                StambeniStatusKucanstva = zahtjev.KucanstvoPodaci?.StambeniStatusKucanstva == 0
                    ? null
                    : zahtjev.KucanstvoPodaci?.StambeniStatusKucanstva,
                SastavKucanstva = zahtjev.KucanstvoPodaci?.SastavKucanstva == 0
                    ? null
                    : zahtjev.KucanstvoPodaci?.SastavKucanstva
            };

        _prebivanjeOd = _kucanstvoModel?.PrebivanjeOd?.ToDateTime(new TimeOnly(0));
    }

    private async Task Submit()
    {
        await _form.Validate();

        if (!_form.IsValid || _kucanstvoModel is null)
            return;

        _kucanstvoModel.PrebivanjeOd = _prebivanjeOd.HasValue
            ? DateOnly.FromDateTime(_prebivanjeOd.Value)
            : null;

        try
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.SpremiKucanstvoIObradiAsync(ZahtjevId, _kucanstvoModel);
            Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Kucanstvo");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel()
    {
        Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Kucanstvo");
    }
}
