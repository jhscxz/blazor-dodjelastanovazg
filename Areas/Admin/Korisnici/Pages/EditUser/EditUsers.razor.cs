using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages.EditUser
{
    public partial class EditUsers : ComponentBase
    {
        [Parameter] public string UserId { get; set; } = null!;
        [Inject] public UserManager<IdentityUser> UserManager { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;

        protected EditUserModel UserModel { get; set; } = new();
        private MudForm _editForm = null!;
        private bool _isValid;
        protected bool AllowEditing { get; set; }
        private List<string> ErrorMessages { get; } = [];

        protected readonly List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems =
        [
            new() { Text = "Početna", Url = "/" },
            new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
            new() { Text = "Korisnici", Url = "/admin/users" },
            new() { Text = "Uredi korisnika", CssClass = "text-red-500 font-bold" }
        ];

        private bool _firstRender = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_firstRender)
            {
                _firstRender = false;
                await LoadUserDataAsync();
            }
        }

        private async Task LoadUserDataAsync()
        {
            var user = await UserManager.FindByIdAsync(UserId);
            if (user is null)
            {
                Navigation.NavigateTo("/admin/users");
                return;
            }

            UserModel = new EditUserModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? ""
            };

            // Dohvat lockout statusa
            var lockoutEnd = await UserManager.GetLockoutEndDateAsync(user);
            UserModel.IsLocked = lockoutEnd.HasValue && lockoutEnd > DateTimeOffset.UtcNow;
            StateHasChanged();
        }

        private async Task SaveUser()
        {
            await _editForm.Validate();
            _isValid = _editForm.IsValid;
            ErrorMessages.Clear();

            if (!_isValid)
            {
                ErrorMessages.Add("Provjerite unesene podatke.");
                return;
            }

            var user = await UserManager.FindByIdAsync(UserModel.UserId);
            if (user is null)
            {
                Navigation.NavigateTo("/admin/users");
                return;
            }

            // Provjera jedinstvenosti Email i UserName
            if (await EmailExistsAsync(user, UserModel.Email)) return;
            if (await UserNameExistsAsync(user, UserModel.UserName)) return;

            // Ažuriraj osnovne podatke
            user.UserName = UserModel.UserName;
            user.Email = UserModel.Email;
            var updateResult = await UserManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                ErrorMessages.AddRange(updateResult.Errors.Select(e => e.Description));
                return;
            }

            // Ažuriraj lockout status (zaključavanje/otključavanje računa)
            var lockoutResult = UserModel.IsLocked
                ? await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100))
                : await UserManager.SetLockoutEndDateAsync(user, null);
            if (!lockoutResult.Succeeded)
            {
                ErrorMessages.AddRange(lockoutResult.Errors.Select(e => e.Description));
                return;
            }

            Navigation.NavigateTo("/admin/users");
        }

        private async Task<bool> EmailExistsAsync(IdentityUser user, string email)
        {
            var existingEmailUser = await UserManager.FindByEmailAsync(email);
            if (existingEmailUser != null && existingEmailUser.Id != user.Id)
            {
                ErrorMessages.Add("Email adresa je već zauzeta.");
                return true;
            }
            return false;
        }

        private async Task<bool> UserNameExistsAsync(IdentityUser user, string userName)
        {
            var existingUsernameUser = await UserManager.FindByNameAsync(userName);
            if (existingUsernameUser != null && existingUsernameUser.Id != user.Id)
            {
                ErrorMessages.Add("Korisničko ime je već zauzeto.");
                return true;
            }
            return false;
        }

        private void Cancel() => Navigation.NavigateTo("/admin/users");
        private void ChangePassword() => Navigation.NavigateTo($"/admin/users/change-password/{UserId}");

        private string LockLabel => UserModel.IsLocked ? "Otključaj account" : "Zaključaj account";

        protected class EditUserModel
        {
            public string UserId { get; init; } = "";

            [Required(ErrorMessage = "Korisničko ime je obavezno")]
            public string UserName { get; set; } = "";

            [Required(ErrorMessage = "Email je obavezan")]
            [EmailAddress(ErrorMessage = "Email nije ispravan")]
            public string Email { get; set; } = "";

            public bool IsLocked { get; set; }
        }
    }
}
