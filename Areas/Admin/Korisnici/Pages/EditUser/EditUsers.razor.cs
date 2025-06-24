using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Services;
using DodjelaStanovaZG.Services.Interfaces;
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
        [Inject] public IUserContextService UserContextService { get; set; } = null!;

        protected EditUserModel UserModel { get; set; } = new();

        private MudForm _editForm = null!;
        private bool _firstRender = true;
        private bool _isSelf;
        private bool _isValid;
        private readonly List<string> _errorMessages = [];

        protected bool AllowEditing { get; set; }
        protected List<string> ErrorMessages => _errorMessages;

        protected readonly List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems = BreadcrumbProvider.AdminUserEdit();

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
                Cancel();
                return;
            }

            var lockoutEnd = await UserManager.GetLockoutEndDateAsync(user);

            UserModel = new EditUserModel
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                IsLocked = lockoutEnd.HasValue && lockoutEnd > DateTimeOffset.UtcNow
            };

            _isSelf = user.Id == UserContextService.GetCurrentUserId();
            StateHasChanged();
        }

        private async Task SaveUser()
        {
            await _editForm.Validate();
            _isValid = _editForm.IsValid;
            _errorMessages.Clear();

            if (!_isValid)
            {
                _errorMessages.Add("Provjerite unesene podatke.");
                return;
            }

            var user = await UserManager.FindByIdAsync(UserModel.UserId);
            if (user is null)
            {
                Cancel();
                return;
            }

            if (await EmailExistsAsync(user, UserModel.Email) || await UserNameExistsAsync(user, UserModel.UserName))
                return;

            user.UserName = UserModel.UserName;
            user.Email = UserModel.Email;

            var updateResult = await UserManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                AddErrors(updateResult);
                return;
            }

            if (!_isSelf)
            {
                var lockoutEnd = UserModel.IsLocked
                    ? DateTimeOffset.UtcNow.AddYears(100)
                    : (DateTimeOffset?)null;

                var lockoutResult = await UserManager.SetLockoutEndDateAsync(user, lockoutEnd);
                if (!lockoutResult.Succeeded)
                {
                    AddErrors(lockoutResult);
                    return;
                }
                
                if (UserModel.IsLocked)
                {
                    await UserManager.UpdateSecurityStampAsync(user);
                }
            }

            Cancel();
        }

        private async Task<bool> EmailExistsAsync(IdentityUser user, string email)
        {
            var existing = await UserManager.FindByEmailAsync(email);
            if (existing != null && existing.Id != user.Id)
            {
                _errorMessages.Add("Email adresa je već zauzeta.");
                return true;
            }
            return false;
        }

        private async Task<bool> UserNameExistsAsync(IdentityUser user, string username)
        {
            var existing = await UserManager.FindByNameAsync(username);
            if (existing != null && existing.Id != user.Id)
            {
                _errorMessages.Add("Korisničko ime je već zauzeto.");
                return true;
            }
            return false;
        }

        private void AddErrors(IdentityResult result)
        {
            _errorMessages.AddRange(result.Errors.Select(e => e.Description));
        }

        protected void Cancel() => Navigation.NavigateTo("/admin/users");

        protected void ChangePassword() => Navigation.NavigateTo($"/admin/users/change-password/{UserId}");

        protected string LockLabel => UserModel.IsLocked ? "Otključaj account" : "Zaključaj account";

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
