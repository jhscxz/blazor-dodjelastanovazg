using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public class SocijalniNatjecajPregledBase : ComponentBase
{
    [Inject] public required IUnitOfWork UnitOfWork { get; set; }
    [Inject] public NavigationManager Navigation { get; set; } = null!;

    [Parameter] public long NatjecajId { get; set; }

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
        Natjecaji = (await UnitOfWork.SocijalniZahtjevService.GetAllAsync())
            .Where(x => x.NatjecajId == NatjecajId)
            .ToList();
    }

    protected void AddZahtjev()
    {
        Navigation.NavigateTo($"/socijalni/dodaj/{NatjecajId}");
    }

    protected void NavigateToDetails(SocijalniNatjecajZahtjevDto zahtjevDto)
    {
        Navigation.NavigateTo($"/socijalni/detalji/{zahtjevDto.Id}");
    }
}