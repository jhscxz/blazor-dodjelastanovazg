using System.Web;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages;

public partial class SocijalniNatjecajDetalji
{
    [Parameter] public long Id { get; set; }

    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private int _selectedTabIndex;
    private string? _activeTab;

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", CssClass = "text-red-500 font-bold" },
    ];

    protected override Task OnInitializedAsync()
    {
        try
        {
            ResolveTabIndexFromQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Greška prilikom dohvaćanja detalja: {ex.Message}");
        }

        return Task.CompletedTask;
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