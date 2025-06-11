using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI.MudInputs;

public partial class EnumSelect<T> : ComponentBase where T : struct, Enum
{
    [Parameter] public T? Value { get; set; }
    [Parameter] public EventCallback<T?> ValueChanged { get; set; }

    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public bool Required { get; set; }
    [Parameter] public string Placeholder { get; set; } = "Odaberi...";

    [Parameter] public Func<T, bool>? Filter { get; set; }

    protected T? _value
    {
        get => Value;
        set
        {
            if (EqualityComparer<T?>.Default.Equals(Value, value)) return;
            Value = value;
            ValueChanged.InvokeAsync(value);
        }
    }
}