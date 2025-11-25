using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI.Buttons;

public partial class ChangePasswordButton : ComponentBase
{
    [Parameter] public string Label { get; set; } = "Promijeni lozinku";
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public string? NavigateTo { get; set; }

    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private async Task HandleClick()
    {
        if (!string.IsNullOrWhiteSpace(NavigateTo))
        {
            Navigation.NavigateTo(NavigateTo);
        }

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}