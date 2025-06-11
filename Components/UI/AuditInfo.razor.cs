using Microsoft.AspNetCore.Components;
using DodjelaStanovaZG.DTO;

namespace DodjelaStanovaZG.Components.UI;

public partial class AuditInfo : ComponentBase
{
    [Parameter]
    public AuditableDto? Model { get; set; }
}