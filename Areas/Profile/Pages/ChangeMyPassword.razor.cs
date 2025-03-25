using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Profile.Pages
{
    [Authorize]
    public partial class ChangeMyPassword
    {
        // Model za promjenu lozinke
        public class ChangePasswordInputModel
        {
            [Required(ErrorMessage = "Stara lozinka je obavezna")]
            public string OldPassword { get; set; } = "";

            [Required(ErrorMessage = "Nova lozinka je obavezna")]
            public string NewPassword { get; set; } = "";

            [Required(ErrorMessage = "Potvrda nove lozinke je obavezna")]
            [Compare("NewPassword", ErrorMessage = "Lozinke se ne podudaraju")]
            public string ConfirmPassword { get; set; } = "";
        }

        // Polja i propertyji koje koristimo u .razor datoteci
        private MudForm _form;
        private bool _isValid = false;
        private List<string> ErrorMessages = new();
        private ChangePasswordInputModel ChangePasswordModel = new();

        // Injekcije
        [Inject] public UserManager<IdentityUser> UserManager { get; set; } = default!;
        [Inject] public SignInManager<IdentityUser> SignInManager { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        // Dohvat AuthenticationState iz Blazora (ako želite provjeru korisnika)
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        // Za breadcrumb (ako koristite vlastitu komponentu)
        protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = new()
        {
            new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
            new Breadcrumbs.BreadcrumbItem { Text = "Profil", Url = "/profile" },
            new Breadcrumbs.BreadcrumbItem { Text = "Promjena lozinke", CssClass = "text-red-500 font-bold" }
        };

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            if (user?.Identity is null || !user.Identity.IsAuthenticated)
            {
                // Ako korisnik nije prijavljen, preusmjeri na login
                Navigation.NavigateTo("/login");
                return;
            }
        }

        private async Task ChangePasswordAsync()
        {
            // Validiraj formu
            await _form.Validate();
            if (!_isValid)
                return;

            // Dohvati trenutno logiranog korisnika
            var authState = await AuthenticationStateTask;
            var userClaims = authState.User;
            var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                ErrorMessages.Add("Nije pronađen ID korisnika.");
                return;
            }

            var identityUser = await UserManager.FindByIdAsync(userId);
            if (identityUser == null)
            {
                ErrorMessages.Add("Korisnik ne postoji.");
                return;
            }

            // Promijeni lozinku
            var result = await UserManager.ChangePasswordAsync(
                identityUser,
                ChangePasswordModel.OldPassword,
                ChangePasswordModel.NewPassword
            );

            if (!result.Succeeded)
            {
                // Prikaži greške
                ErrorMessages.Clear();
                foreach (var error in result.Errors)
                {
                    ErrorMessages.Add(error.Description);
                }
            }
            else
            {
                // Osvježi korisničku autentikaciju nakon promjene lozinke
                await SignInManager.RefreshSignInAsync(identityUser);

                // Preusmjeri natrag na profil ili neku drugu stranicu
                Navigation.NavigateTo("/profile");
            }
        }
    }
}
