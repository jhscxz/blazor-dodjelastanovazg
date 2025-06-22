using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI.MudInputs;

public partial class BoolSelect : ComponentBase
{
    [Parameter] public bool Value { get; set; }
    [Parameter] public EventCallback<bool> ValueChanged { get; set; }
    [Parameter] public string Label { get; set; } = string.Empty;

    protected bool InternalValue
    {
        get => Value;
        set
        {
            if (Value == value) return;
            Value = value;
            ValueChanged.InvokeAsync(value);
        }
    }
}