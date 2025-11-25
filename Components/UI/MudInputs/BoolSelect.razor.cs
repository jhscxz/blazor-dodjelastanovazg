using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI.MudInputs;

public partial class BoolSelect : ComponentBase
{
    [Parameter] public bool Value { get; set; }
    [Parameter] public EventCallback<bool> ValueChanged { get; set; }
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public bool ReadOnly { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

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