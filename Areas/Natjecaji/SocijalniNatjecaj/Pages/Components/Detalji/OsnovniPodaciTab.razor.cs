using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class OsnovniPodaciTab : ComponentBase
{
    [Parameter] public long Id { get; set; }

    [Inject] protected IUnitOfWork UnitOfWork { get; set; } = null!;

    protected SocijalniNatjecajZahtjevDto Detalji { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Detalji = await UnitOfWork.SocijalniZahtjevRead.GetDetaljiAsync(Id);
    }

    private Color ChipColor => Detalji.RezultatObrade switch
    {
        RezultatObrade.Neosnovan => Color.Error,
        RezultatObrade.Greška => Color.Error,
        RezultatObrade.Nepotpun => Color.Warning,
        RezultatObrade.Osnovan => Color.Success,
        _ => Color.Default
    };

    private string ChipText => Detalji.RezultatObrade switch
    {
        RezultatObrade.Neosnovan => "Neosnovan",
        RezultatObrade.Nepotpun => "Nepotpuno",
        RezultatObrade.Osnovan => "Osnovan",
        RezultatObrade.Greška =>"Greška",
        _ => "Nepoznato"
    };
}