using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Components.UI.MudInputs;

public partial class EditButton : ComponentBase
{
    [Parameter] public string Label { get; set; } = "Uredi";
    [Parameter] public string Icon { get; set; } = Icons.Material.Filled.Edit;
    [Parameter] public string? Href { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public string Class { get; set; } = "mt-2";
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    protected async Task HandleClick()
    {
        if (!string.IsNullOrWhiteSpace(Href))
        {
            Navigation.NavigateTo(Href);
        }

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}