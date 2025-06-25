using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI;

public partial class ListHeader<TFilter> : ComponentBase
{
    [Parameter] public string AddButtonText { get; set; } = "Dodaj";
    [Parameter] public EventCallback OnAdd { get; set; }
    [Parameter] public string SearchLabel { get; set; } = "Pretraga";
    [Parameter] public string? SearchText { get; set; }
    [Parameter] public EventCallback<string> OnSearchChanged { get; set; }
    [Parameter] public string FilterLabel { get; set; } = "Filter";
    [Parameter] public TFilter? FilterValue { get; set; }
    [Parameter] public EventCallback<TFilter?> OnFilterChanged { get; set; }
    [Parameter] public string FilterWidthClass { get; set; } = "w-[160px]";
    [Parameter] public RenderFragment? FilterOptions { get; set; }
    [Parameter] public string ExportButtonText { get; set; } = "Export";
    [Parameter] public EventCallback OnExport { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}