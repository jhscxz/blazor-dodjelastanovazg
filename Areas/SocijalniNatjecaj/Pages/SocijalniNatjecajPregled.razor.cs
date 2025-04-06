using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public class SocijalniNatjecajPregledBase : ComponentBase
{
    [Inject] public required ISocijalniNatjecajService SocijalniNatjecajService { get; set; }
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Parameter]
    public long NatjecajId { get; set; }

    protected MudTable<SocijalniNatjecajZahtjevDto> Table = null!;
    protected List<SocijalniNatjecajZahtjevDto> Natjecaji { get; set; } = [];

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Pregled zapisa", CssClass = "text-red-500 font-bold" },
    ];

    protected override async Task OnInitializedAsync()
    {
        Natjecaji = (await SocijalniNatjecajService.GetAllAsync())
            .Where(x => x.NatjecajId == NatjecajId)
            .ToList();
    }
    
    protected void AddZahtjev()
    {
        Navigation.NavigateTo($"/socijalni/dodaj/{NatjecajId}");
    }
    
    protected void NavigateToDetails(SocijalniNatjecajZahtjevDto zahtjevDto)
    {
        // Pretpostavljamo da je stranica detalja na /socijalni/detalji/{NatjecajId}
        Navigation.NavigateTo($"/socijalni/detalji/{zahtjevDto.Id}");
    }

}