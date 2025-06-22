using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages;

public partial class SocijalniZahtjevPregledContent
{
    [Parameter] public required SocijalniNatjecajZahtjevDto Model { get; set; }

    [Inject] public NavigationManager Nav { get; set; } = null!;

    private static string FormatBool(bool? value) => value == true ? "Da" : "Ne";
    private static string FormatInt(int? value) => value?.ToString() ?? "0";
    private static string FormatDecimal(decimal? value) => value?.ToString("N2") + " €";
    private static string FormatFloat(float? value) => value?.ToString("0.##") ?? "0";

    private void GenerirajZapisnik()
    {
        var url = Nav.BaseUri + $"api/export/zapisnik/{Model.Id}";
        Nav.NavigateTo(url, forceLoad: true);
    }

    private bool ShouldDisableZapisnik =>
        Model.RezultatObrade == RezultatObrade.Greška;
}