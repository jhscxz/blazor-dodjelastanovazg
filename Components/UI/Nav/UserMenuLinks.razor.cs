using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DodjelaStanovaZG.Components.UI.Nav;

public partial class UserMenuLinks : ComponentBase
{
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
    private string _currentUserId = string.Empty;
    private string _currentUserName = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated ?? false)
        {
            _currentUserId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            _currentUserName = user.Identity.Name ?? string.Empty;
        }
    }
}