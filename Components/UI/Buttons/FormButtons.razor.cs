using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI.Buttons;

public partial class FormButtons
{
    [Parameter] public EventCallback Submit { get; set; }
    [Parameter] public EventCallback Cancel { get; set; }
}