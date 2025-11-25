using Microsoft.AspNetCore.Components;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Components.UI.MudInputs;

public partial class RezultatObradeToggle : ComponentBase
{
    [Parameter] public RezultatObrade? Value { get; set; }
    [Parameter] public EventCallback<RezultatObrade?> ValueChanged { get; set; }
}