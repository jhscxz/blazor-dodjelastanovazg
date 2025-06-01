using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages.Components.Detalji;

public class PrikazBase : ComponentBase
{
    [Parameter] public long Id { get; set; }

    [Inject] protected IUnitOfWork UnitOfWork { get; set; } = default!;

    protected SocijalniNatjecajZahtjevDto Detalji { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Detalji = await UnitOfWork.SocijalniNatjecajDetaljiService.GetDetaljiAsync(Id);
    }

    protected Color ChipColor => Detalji.RezultatObrade switch
    {
        RezultatObrade.Neosnovan => Color.Error,
        RezultatObrade.Nepotpun => Color.Warning,
        RezultatObrade.Zadovoljava => Color.Success,
        _ => Color.Default
    };

    protected string ChipIcon => Detalji.RezultatObrade switch
    {
        RezultatObrade.Neosnovan => Icons.Material.Filled.Dangerous,
        RezultatObrade.Nepotpun => Icons.Material.Filled.Warning,
        RezultatObrade.Zadovoljava => Icons.Material.Filled.CheckCircle,
        _ => Icons.Material.Filled.Info
    };

    protected string ChipText => Detalji.RezultatObrade switch
    {
        RezultatObrade.Neosnovan => "Neosnovan",
        RezultatObrade.Nepotpun => "Nepotpuno",
        RezultatObrade.Zadovoljava => "Osnovan",
        _ => "Nepoznato"
    };

    protected Variant ChipVariant => Detalji.RezultatObrade == RezultatObrade.Zadovoljava
        ? Variant.Outlined
        : Variant.Text;
}