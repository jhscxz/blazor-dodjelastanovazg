using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace DodjelaStanovaZG.Components.UI.MudInputs;

public partial class DateInput
{
    [Parameter] public DateTime? Value { get; set; }
    [Parameter] public EventCallback<DateTime?> ValueChanged { get; set; }
    [Parameter] public string Label { get; set; } = "Datum";
    [Parameter] public bool Required { get; set; }
    [Parameter] public string DateFormat { get; set; } = "dd.MM.yyyy";
    [Parameter] public CultureInfo Culture { get; set; } = new("hr-HR");

    protected DateTime? InternalValue
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