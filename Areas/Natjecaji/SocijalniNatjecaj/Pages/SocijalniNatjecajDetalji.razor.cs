using System.Web;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages;

public partial class SocijalniNatjecajDetalji
{
    [Parameter] public long Id { get; set; }
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private ILogger<SocijalniNatjecajDetalji> Logger { get; set; } = null!;
    [Inject] protected IUnitOfWork UnitOfWork { get; set; } = null!;
    private long _natjecajId;
    private int _selectedTabIndex;
    private string? _activeTab;

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.SocijalniDetalji();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ResolveTabIndexFromQuery();
            var zahtjev = await UnitOfWork.SocijalniZahtjevRead.GetZahtjevByIdAsync(Id);
            if (zahtjev != null) _natjecajId = zahtjev.NatjecajId;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Greška prilikom dohvaćanja detalja: {Message}", ex.Message);
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
    
    private void Povratak()
    {
        Navigation.NavigateTo($"/socijalni/pregled/{_natjecajId}");
    }
}