using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI;

public partial class Container : ComponentBase
{
    [Parameter] public List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = new();
    [Parameter] public RenderFragment? ChildContent { get; set; }
}