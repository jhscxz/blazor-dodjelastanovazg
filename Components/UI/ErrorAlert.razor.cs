using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI;

public partial class ErrorAlert : ComponentBase
{
    [Parameter] public IEnumerable<string>? Messages { get; set; }
}