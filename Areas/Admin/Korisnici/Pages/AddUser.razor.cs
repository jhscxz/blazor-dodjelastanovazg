using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public partial class AddUser : ComponentBase
    {
        [Inject] private UserManager<IdentityUser> UserManager { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IUserService UserService { get; set; } = default!;

        protected MudForm AddForm = default!;
        protected AddUserModel UserModel { get; set; } = new();
        
        // Ovde ćemo čuvati sve eventualne greške
        protected List<string> ErrorMessages { get; set; } = new();

        protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = new()
        {
            new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
            new Breadcrumbs.BreadcrumbItem { Text = "Admin Nadzorna ploča", Url = "/admin" },
            new Breadcrumbs.BreadcrumbItem { Text = "Korisnici", Url = "/admin/users" },
            new Breadcrumbs.BreadcrumbItem { Text = "Dodaj korisnika" }
        };

        /// <summary>
        /// Metoda koja kreira novog korisnika i dodaje ga u željenu rolu.
        /// </summary>
        protected async Task SaveUser()
        {
            // Očistimo prethodne greške pre svakog kreiranja
            ErrorMessages.Clear();

            // Provera da li već postoji korisnik s istim email-om
            var existingEmailUser = await UserManager.FindByEmailAsync(UserModel.Email);
            if (existingEmailUser != null)
            {
                ErrorMessages.Add("Email adresa je već registrirana.");
            }

            // Provera da li već postoji korisnik s istim username-om
            var existingUsernameUser = await UserManager.FindByNameAsync(UserModel.UserName);
            if (existingUsernameUser != null)
            {
                ErrorMessages.Add("Korisničko ime je već zauzeto.");
            }

            // Ako već postoje greške, ne nastavljamo dalje
            if (ErrorMessages.Count > 0)
            {
                return;
            }

            // Kreiramo novog IdentityUser-a
            var user = new IdentityUser
            {
                UserName = UserModel.UserName,
                Email = UserModel.Email
            };

            var result = await UserManager.CreateAsync(user, UserModel.Password);
            if (result.Succeeded)
            {
                // Ako je selektovana rola, dodajemo korisnika i u tu rolu
                if (!string.IsNullOrWhiteSpace(UserModel.Role))
                {
                    var roleResult = await UserManager.AddToRoleAsync(user, UserModel.Role);
                    if (!roleResult.Succeeded)
                    {
                        // Ako dodela role ne uspe, ispišemo greške
                        ErrorMessages.AddRange(roleResult.Errors.Select(e => e.Description));
                        return;
                    }
                }

                // Ako je sve uspešno, preusmeravamo na listu korisnika
                Navigation.NavigateTo("/admin/users");
            }
            else
            {
                // Ispisujemo Identity greške
                ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
            }
        }

        /// <summary>
        /// Ručna validacija i poziv kreiranja korisnika.
        /// </summary>
        protected async Task SubmitForm()
        {
            await AddForm.Validate();
            if (AddForm.IsValid)
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
        protected void Cancel() => Navigation.NavigateTo("/admin/users");

        /// <summary>
        /// Model za unos podataka novog korisnika
        /// </summary>
        public class AddUserModel
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
