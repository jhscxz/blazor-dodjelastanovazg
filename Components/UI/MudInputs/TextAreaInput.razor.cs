using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Components.UI.MudInputs;

public partial class TextAreaInput<T> : ComponentBase
{
    [Parameter] public T? Value { get; set; }
    [Parameter] public EventCallback<T> ValueChanged { get; set; }
    [Parameter] public Expression<Func<T?>> For { get; set; } = null!;
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public bool Required { get; set; }
    [Parameter] public int Lines { get; set; } = 3;
    [Parameter] public bool ShrinkLabel { get; set; } = true;
    [Parameter] public Variant Variant { get; set; } = Variant.Outlined;
    [Parameter] public Margin Margin { get; set; } = Margin.Dense;
    [Parameter] public bool ReadOnly { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    protected T? CurrentValue
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