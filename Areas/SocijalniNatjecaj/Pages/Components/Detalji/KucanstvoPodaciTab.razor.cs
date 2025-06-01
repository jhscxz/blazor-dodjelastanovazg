using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages.Components.Detalji;

public class KucanstvoPodaciTabBase : ComponentBase
{
    [Parameter]
    public long Id { get; set; }

    [Inject]
    protected IUnitOfWork UnitOfWork { get; set; } = default!;

    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    protected SocijalniKucanstvoPodaciDto? Kucanstvo { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var detalji = await UnitOfWork.SocijalniNatjecajDetaljiService.GetDetaljiAsync(Id);
        Kucanstvo = detalji.KucanstvoPodaci;
    }

    protected string GetDisplay(Enum? value) =>
        value is null ? string.Empty : value.GetDisplayName();
}