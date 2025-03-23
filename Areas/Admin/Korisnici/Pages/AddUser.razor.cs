using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public partial class AddUser : ComponentBase
    {
        [Inject] private UserManager<IdentityUser> UserManager { get; set; } = null!;
        [Inject] private NavigationManager Navigation { get; set; } = null!;

        private MudForm _addForm = null!;
        protected AddUserModel UserModel { get; } = new() { Role = "Korisnik" };
        private List<string> ErrorMessages { get; } = [];

        protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
        [
            new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
            new Breadcrumbs.BreadcrumbItem { Text = "Admin Nadzorna ploča", Url = "/admin" },
            new Breadcrumbs.BreadcrumbItem { Text = "Korisnici", Url = "/admin/users" },
            new Breadcrumbs.BreadcrumbItem { Text = "Dodaj korisnika" }
        ];

        /// <summary>
        /// Kreira novog korisnika i dodaje ga u željenu rolu.
        /// </summary>
        private async Task SaveUser()
        {
            ErrorMessages.Clear();

            // Provjera jedinstvenosti email-a i korisničkog imena
            var existingEmailUser = await UserManager.FindByEmailAsync(UserModel.Email);
            if (existingEmailUser != null)
            {
                ErrorMessages.Add("Email adresa je već registrirana.");
            }

            var existingUsernameUser = await UserManager.FindByNameAsync(UserModel.UserName);
            if (existingUsernameUser != null)
            {
                ErrorMessages.Add("Korisničko ime je već zauzeto.");
            }

            if (ErrorMessages.Any())
            {
                return;
            }

            // Kreiramo novog korisnika
            var user = new IdentityUser
            {
                UserName = UserModel.UserName,
                Email = UserModel.Email
            };

            var result = await UserManager.CreateAsync(user, UserModel.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(UserModel.Role))
                {
                    var roleResult = await UserManager.AddToRoleAsync(user, UserModel.Role);
                    if (!roleResult.Succeeded)
                    {
                        ErrorMessages.AddRange(roleResult.Errors.Select(e => e.Description));
                        return;
                    }
                }

                Navigation.NavigateTo("/admin/users");
            }
            else
            {
                ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
            }
        }

        /// <summary>
        /// Ručna validacija i poziv kreiranja korisnika.
        /// </summary>
        private async Task SubmitForm()
        {
            await _addForm.Validate();
            if (_addForm.IsValid)
            {
                await SaveUser();
            }
            else
            {
                ErrorMessages.Clear();
                ErrorMessages.Add("Forma nije validna. Molimo proverite unesene podatke.");
            }
        }

        /// <summary>
        /// Odustajemo i vraćamo se na listu korisnika.
        /// </summary>
        private void Cancel() => Navigation.NavigateTo("/admin/users");

        /// <summary>
        /// Model za unos podataka novog korisnika.
        /// </summary>
        protected class AddUserModel
        {
            [Required(ErrorMessage = "Korisničko ime je obavezno")]
            public string UserName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email je obavezan")]
            [EmailAddress(ErrorMessage = "Email nije ispravan")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Lozinka je obavezna")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Rola je obavezna")]
            public string Role { get; set; } = string.Empty;
        }
    }
}
