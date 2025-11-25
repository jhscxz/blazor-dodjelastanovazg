using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages;

public partial class AddUser : ComponentBase
{
    [Inject] private UserManager<IdentityUser> UserManager { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    protected AddUserDto UserModel { get; } = new();
    private MudForm _addForm = null!;

    private List<string> ErrorMessages { get; } = [];

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.AdminUserAdd();
    
    private async Task<bool> ValidateForm()
    {
        await _addForm.Validate();
        if (_addForm.IsValid) return true;

        ErrorMessages.Clear();
        ErrorMessages.Add("Forma nije validna. Molimo provjerite unesene podatke.");
        return false;
    }

    private async Task SubmitForm()
    {
        ErrorMessages.Clear();

        if (!await ValidateForm())
            return;

        var existingEmailUser = await UserManager.FindByEmailAsync(UserModel.Email);
        if (existingEmailUser != null)
            ErrorMessages.Add("Email adresa je već registrirana.");

        var existingUsernameUser = await UserManager.FindByNameAsync(UserModel.UserName);
        if (existingUsernameUser != null)
            ErrorMessages.Add("Korisničko ime je već zauzeto.");

        if (ErrorMessages.Count != 0) return;

        var user = new IdentityUser
        {
            UserName = UserModel.UserName,
            Email = UserModel.Email
        };

        var result = await UserManager.CreateAsync(user, UserModel.Password);
        if (!result.Succeeded)
        {
            ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
            return;
        }

        Navigation.NavigateTo("/admin/users");
    }

    private void Cancel() => Navigation.NavigateTo("/admin/users");
}
