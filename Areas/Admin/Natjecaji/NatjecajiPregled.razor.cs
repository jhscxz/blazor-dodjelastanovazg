using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji;

public class NatjecajiPregledBase : ComponentBase
{
    [Inject] public required NavigationManager Navigation { get; set; }
    [Inject] public required IUnitOfWork UnitOfWork { get; set; }
    protected MudTable<NatjecajDto> _table = null!;
    protected string? OdabranaVrsta;
    protected static int RowsPerPage => 10;

    protected List<NatjecajDto> Natjecaji { get; set; } = new();

    protected List<NatjecajDto> FilteredNatjecaji => string.IsNullOrEmpty(OdabranaVrsta)
        ? Natjecaji
        : Natjecaji.Where(x => x.Vrsta == OdabranaVrsta).ToList();

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
        new Breadcrumbs.BreadcrumbItem { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new Breadcrumbs.BreadcrumbItem { Text = "Natječaji", CssClass = "text-red-500 font-bold" },
    ];

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
        await UnitOfWork.NatjecajiService.UpdateAsync(natjecaj.Klasa, natjecaj);
        await UnitOfWork.SaveChangesAsync();
    }

    protected async Task  Otkljucaj(NatjecajDto natjecaj)
    {
        natjecaj.Status = "Aktivan";
        await UnitOfWork.NatjecajiService.UpdateAsync(natjecaj.Klasa, natjecaj);
        await UnitOfWork.SaveChangesAsync();
    }
}