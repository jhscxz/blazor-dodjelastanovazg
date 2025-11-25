using System.Security.Claims;
using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.DTO;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Services;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Profile.Pages;

[Authorize]
public partial class ChangeMyPassword : ComponentBase
{
    [Inject] private IPasswordService PasswordService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
    private MudForm _form = null!;
    private readonly List<string> _errorMessages = [];
    private readonly ChangeMyPasswordDto _changePasswordModel = new();

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.ChangeMyPassword();

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        if (authState.User.Identity is not { IsAuthenticated: true })
        {
            Navigation.NavigateTo("/login");
        }
    }

    private async Task ChangePasswordAsync()
    {
        await _form.Validate();
        if (!_form.IsValid) return;

        var authState = await AuthenticationStateTask;
        var userId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier);

        _errorMessages.Clear();
        if (string.IsNullOrWhiteSpace(userId))
        {
            _errorMessages.Add("Nije pronađen ID korisnika.");
            return;
        }

        var result = await PasswordService.ChangeOwnPasswordAsync(
            userId,
            _changePasswordModel.OldPassword,
            _changePasswordModel.NewPassword
        );

        if (!result.Succeeded)
        {
            _errorMessages.AddRange(result.Errors.Select(e => e.Description));
        }
        else
        {
            Navigation.NavigateTo($"/profile/{userId}");
        }
    }
}