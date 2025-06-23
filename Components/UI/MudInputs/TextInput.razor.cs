using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Components.UI.MudInputs;

public partial class TextInput<T> : ComponentBase
{
    [Parameter] public T? Value { get; set; }
    [Parameter] public EventCallback<T> ValueChanged { get; set; }
    [Parameter] public Expression<Func<T?>> For { get; set; } = null!;
    [Parameter] public Expression<Func<T?>>? ValueExpression { get; set; }
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public bool Required { get; set; }
    [Parameter] public int Lines { get; set; } = 1;
    [Parameter] public bool ReadOnly { get; set; }
    [Parameter] public InputType InputType { get; set; } = InputType.Text;
    [Parameter] public Variant Variant { get; set; } = Variant.Outlined;
    [Parameter] public string Class { get; set; } = string.Empty;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task OnValueChanged(T newValue)
    {
        Value = newValue;                      
        await ValueChanged.InvokeAsync(newValue); 
    }
}