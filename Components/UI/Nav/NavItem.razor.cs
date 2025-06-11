using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI.Nav;

public partial class NavItem : ComponentBase
{
    [Parameter] public string Href { get; set; } = "#";
    [Parameter] public string Text { get; set; } = "";
    [Parameter] public string TextColor { get; set; } = "#ffffff";
    [Parameter] public bool UseLogoutIcon { get; set; }
    private string Style => $"color: {TextColor};";
}