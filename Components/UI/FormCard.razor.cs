using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI;

public partial class FormCard : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public IEnumerable<string>? ErrorMessages { get; set; }
    [Parameter] public int Xs { get; set; } = 12;
    [Parameter] public int Sm { get; set; } = 10;
    [Parameter] public int Md { get; set; } = 6;
    [Parameter] public int Lg { get; set; } = 4;
    [Parameter] public string? CardClass { get; set; }
    [Parameter] public string? CardStyle { get; set; }
}