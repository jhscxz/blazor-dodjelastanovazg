using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class KucanstvoEditFormPage
{
    [Parameter] public long ZahtjevId { get; set; }

    [Inject] private IUnitOfWork UnitOfWork { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private SocijalniKucanstvoPodaciDto? _kucanstvoModel;
    private MudForm _form = null!;
    private DateTime? _prebivanjeOd;

    private List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", Url = "" },
        new() { Text = "Uredi kućanstvo", CssClass = "text-red-500 font-bold" }
    ];

    protected override async Task OnInitializedAsync()
    {
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

        BreadcrumbItems[2].Url = $"/socijalni/detalji/{ZahtjevId}";
    }

    private async Task Submit()
    {
        await _form.Validate();

        if (!_form.IsValid || _kucanstvoModel is null)
            return;

        _kucanstvoModel.PrebivanjeOd = _prebivanjeOd.HasValue
            ? DateOnly.FromDateTime(_prebivanjeOd.Value)
            : null;

        await UnitOfWork.SocijalniZahtjevProcessor.SpremiKucanstvoIObradiAsync(ZahtjevId, _kucanstvoModel);
        Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Kucanstvo");
    }

    private void Cancel()
    {
        Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Kucanstvo");
    }
}
