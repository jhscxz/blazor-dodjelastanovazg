using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI;

public partial class ExtendedTd : ComponentBase
{
    [Parameter] public int? ColSpan { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }
}