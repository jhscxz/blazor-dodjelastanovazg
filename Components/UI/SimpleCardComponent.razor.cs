using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI;

public partial class SimpleCardComponent : LayoutComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}