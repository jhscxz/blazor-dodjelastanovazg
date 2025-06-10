using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class BodovanjeOpcijeEditTabBase : ComponentBase
{
    [Parameter] public long Id { get; set; }

    [Inject] protected IUnitOfWork UnitOfWork { get; set; } = default!;
    [Inject] protected ISnackbar Snackbar { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    protected SocijalniNatjecajBodovniPodaciDto? Model { get; set; }

    private DateTime CreatedAt { get; set; }
    private string? CreatedBy { get; set; }
    private string? CreatedByUserName { get; set; }
    private DateTime? UpdatedAt { get; set; }
    private string? UpdatedBy { get; set; }
    private string? UpdatedByUserName { get; set; }
    
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

        await UnitOfWork.SocijalniBodovniPodaciService.UpdateAsync(Id, Model);
        await UnitOfWork.SaveChangesAsync();

        Snackbar.Add("Podaci uspješno spremljeni.", Severity.Success);
        Navigation.NavigateTo($"/socijalni/detalji/{Id}?tab=Bodovi");
    }
}