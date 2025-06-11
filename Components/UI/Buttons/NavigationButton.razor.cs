using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI.Buttons;

public partial class NavigationButton : ComponentBase
{
    [Parameter] public string NavigateTo { get; set; } = string.Empty;
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;
    [Parameter] public string ButtonClass { get; set; } = string.Empty;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private void Navigate()
    {
        if (!string.IsNullOrWhiteSpace(NavigateTo))
        {
            NavigationManager.NavigateTo(NavigateTo);
        }
    }
}