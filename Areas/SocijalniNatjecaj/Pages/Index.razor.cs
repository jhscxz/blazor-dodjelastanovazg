using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public class IndexBase : ComponentBase
{
    [Inject] public required INatjecajOdabirService NatjecajOdabirService { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }

    protected List<SocijalniNatjecajOdabirDto> Natjecaji = [];
    protected int SelectedId;

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
        new Breadcrumbs.BreadcrumbItem { Text = "Socijalni natječaji", CssClass = "text-red-500 font-bold" },
    ];

    protected override async Task OnInitializedAsync()
    {
        var svi = await NatjecajOdabirService.GetAllModelsAsync();

        Natjecaji = svi
            .Where(n => n.PriustiviIliSocijalni == 2)
            .Select(n => new SocijalniNatjecajOdabirDto
            {
                Id = (int)n.Id,
                Godina = n.DatumObjave.Year,
                PriustiviIliSocijalni = n.PriustiviIliSocijalni
            })
            .OrderByDescending(n => n.Godina)
            .ToList();

        if (Natjecaji.Any())
        {
            SelectedId = Natjecaji.First().Id;
        }
    }

    protected void OnPregledajClick()
    {
        if (SelectedId != 0)
        {
            Navigation.NavigateTo($"/socijalni/pregled/{SelectedId}");
        }
    }
}