using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DodjelaStanovaZG.Services;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages.EditUser;

public partial class ResetPassword : ComponentBase
{
    [Parameter] public string UserId { get; set; } = null!;

    [Inject] private IPasswordService PasswordService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private MudForm _form = null!;
    private readonly ResetPasswordDto _resetPasswordModel = new();
    private readonly List<string> _errorMessages = [];

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.AdminUserResetPassword();

    protected override async Task OnInitializedAsync()
    {
        var user = await PasswordService.GetUserByIdAsync(UserId);
        if (user == null)
        {
            Navigation.NavigateTo("/admin/users");
        }
    }

    private async Task ChangePasswordAsync()
    {
        await _form.Validate();

        if (!_form.IsValid) return;

        var result = await PasswordService.ResetPasswordAsync(UserId, _resetPasswordModel.NewPassword);

        _errorMessages.Clear();
        if (!result.Succeeded)
        {
            _errorMessages.AddRange(result.Errors.Select(e => e.Description));
            return;
        }

        Navigation.NavigateTo("/admin/users");
    }
}