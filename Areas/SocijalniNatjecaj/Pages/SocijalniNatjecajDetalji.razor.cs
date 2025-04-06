using System.Web;
using Microsoft.AspNetCore.Components;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public partial class SocijalniNatjecajDetalji
{
    [Parameter] public long Id { get; set; }

    [Inject] private ISocijalniNatjecajDetaljiService DetaljiService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private SocijalniNatjecajDto? _detalji;
    private int _selectedTabIndex;
    private string? _activeTab;

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", CssClass = "text-red-500 font-bold" },
    ];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ResolveTabIndexFromQuery();
            _detalji = await DetaljiService.GetDetaljiAsync(Id);
            Console.WriteLine(_detalji.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Greška prilikom dohvaćanja detalja: {ex.Message}");
        }
    }

    private void ResolveTabIndexFromQuery()
    {
        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var query = HttpUtility.ParseQueryString(uri.Query);
        _activeTab = query["tab"];

        _selectedTabIndex = _activeTab switch
        {
            "Kucanstvo" => 1,
            "Clanovi" => 2,
            "Bodovi" => 3,
            _ => 0
        };
    }
}