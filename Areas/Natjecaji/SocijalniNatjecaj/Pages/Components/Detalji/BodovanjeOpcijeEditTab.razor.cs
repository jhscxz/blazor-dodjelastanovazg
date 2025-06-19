using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
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
    
    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", Url = "" },
        new() { Text = "Uredi bodovne podatke", CssClass = "text-red-500 font-bold" }
    ];

    protected override async Task OnInitializedAsync()
    {
        Model = await UnitOfWork.SocijalniBodovniPodaciService.GetAsync(Id);
        BreadcrumbItems[2].Url = $"/socijalni/detalji/{Id}";
    }

    protected async Task SaveChanges()
    {
        if (Model is null) return;

        await UnitOfWork.SocijalniZahtjevProcessor.SpremiBodovnePodatkeIObradiAsync(Id, Model);

        Snackbar.Add("Podaci uspješno spremljeni.", Severity.Success);
        Navigation.NavigateTo($"/socijalni/detalji/{Id}?tab=Bodovi");
    }
    
    protected void Cancel()
    {
        Navigation.NavigateTo($"/socijalni/detalji/{Id}?tab=Bodovi");
    }
}