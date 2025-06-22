using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages;

public class IndexBase : ComponentBase
{
    [Inject] public required INatjecajOdabirService NatjecajOdabirService { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }

    protected List<SocijalniNatjecajOdabirDto> Natjecaji = [];
    protected int SelectedId;
    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.SocijalniIndex();

    protected override async Task OnInitializedAsync()
    {
        var svi = await NatjecajOdabirService.GetAllModelsAsync();

        Natjecaji = svi
            .Where(n => n.PriustiviIliSocijalni == 2)
            .Select(n => new SocijalniNatjecajOdabirDto
            {
                Id = (int)n.Id,
                Godina = n.DatumObjave.Year
            })
            .OrderByDescending(n => n.Godina)
            .ToList();

        if (Natjecaji.Count != 0)
        {
            SelectedId = Natjecaji.First().Id;
            StateHasChanged();
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