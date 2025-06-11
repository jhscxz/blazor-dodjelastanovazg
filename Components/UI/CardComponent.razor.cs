using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI;

public partial class CardComponent : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Naslov kartice";
    [Parameter] public string Description { get; set; } = "Opis kartice";
    [Parameter] public string ButtonText { get; set; } = "Pregled";
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}