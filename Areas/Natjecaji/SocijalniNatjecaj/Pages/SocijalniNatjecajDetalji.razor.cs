using System.Web;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages;

public partial class SocijalniNatjecajDetalji
{
    [Parameter] public long Id { get; set; }
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private ILogger<SocijalniNatjecajDetalji> Logger { get; set; } = null!;
    private int _selectedTabIndex;
    private string? _activeTab;

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.SocijalniDetalji();

    protected override Task OnInitializedAsync()
    {
        try
        {
            ResolveTabIndexFromQuery();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Greška prilikom dohvaćanja detalja: {Message}", ex.Message);
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