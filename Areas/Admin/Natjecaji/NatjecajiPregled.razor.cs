using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji;

public class NatjecajiPregledBase : ComponentBase
{
    [Inject] public required NavigationManager Navigation { get; set; }
    [Inject] public required IUnitOfWork UnitOfWork { get; set; }
    protected MudTable<NatjecajDto> Table = null!;
    protected string? OdabranaVrsta;
    protected static int RowsPerPage => 10;
    private List<NatjecajDto> Natjecaji { get; set; } = [];

    protected List<NatjecajDto> FilteredNatjecaji => string.IsNullOrEmpty(OdabranaVrsta)
        ? Natjecaji
        : Natjecaji.Where(x => x.Vrsta == OdabranaVrsta).ToList();

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.AdminNatjecajiPregled();

    protected override async Task OnInitializedAsync()
    {
        var all = await UnitOfWork.NatjecajiService.GetAllAsync();
        Natjecaji = all.ToList();
    }

    protected void KreirajNatjecaj()
    {
        Navigation.NavigateTo("/admin/natjecaji/add");
    }

    protected void Uredi(NatjecajDto natjecaj)
    {
        Navigation.NavigateTo($"/admin/natjecaji/edit/{natjecaj.Klasa}");
    }

    protected async Task Zakljucaj(NatjecajDto natjecaj)
    {
        natjecaj.Status = "Zaključen";
        await UnitOfWork.NatjecajiService.UpdateAsync(natjecaj.Klasa!.Value, natjecaj);
    }

    protected async Task Otkljucaj(NatjecajDto natjecaj)
    {
        natjecaj.Status = "Aktivan";
        await UnitOfWork.NatjecajiService.UpdateAsync(natjecaj.Klasa!.Value, natjecaj);
    }
    protected void OnVrstaChanged(string? vrsta)
    {
        OdabranaVrsta = string.IsNullOrWhiteSpace(vrsta) ? null : vrsta;
    }
}