using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public class BodovanjeOpcijeEditTabBase : ComponentBase
{
    [Parameter] public long Id { get; set; }
    [Inject] protected IUnitOfWork UnitOfWork { get; set; } = null!;
    [Inject] protected ISnackbar Snackbar { get; set; } = null!;
    [Inject] protected NavigationManager Navigation { get; set; } = null!;
    protected SocijalniNatjecajBodovniPodaciDto? Model { get; set; }
    
    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbItems = BreadcrumbProvider.ZahtjevEdit(Id, "Uredi bodovne podatke");
        Model = await UnitOfWork.SocijalniBodovniPodaciService.GetAsync(Id);
    }

    protected async Task SaveChanges()
    {
        if (Model is null) return;

        try
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.SpremiBodovnePodatkeIObradiAsync(Id, Model);

            Snackbar.Add("Podaci uspješno spremljeni.", Severity.Success);
            Navigation.NavigateTo($"/socijalni/detalji/{Id}?tab=Bodovi");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
        }
    }
    
    protected void Cancel()
    {
        Navigation.NavigateTo($"/socijalni/detalji/{Id}?tab=Bodovi");
    }
}