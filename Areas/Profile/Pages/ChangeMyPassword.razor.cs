using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using DodjelaStanovaZG.Services;
using DodjelaStanovaZG.DTO;       // uvoz DTO-a

namespace DodjelaStanovaZG.Areas.Profile.Pages
{
    [Authorize]
    public partial class ChangeMyPassword
    {
        // Sada koristimo DTO umjesto lokalnog modela
        private MudForm _form;
        private bool _isValid = false;
        private List<string> ErrorMessages = new();
        private ChangeMyPasswordDto ChangePasswordModel = new();

        // Injekcije
        [Inject] public IPasswordService PasswordService { get; set; } = default!;
        [Inject] public NavigationManager Navigation { get; set; } = default!;

        // Dohvat AuthenticationState iz Blazora
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        // Breadcrumbs (opcionalno)
        protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = new()
        {
            new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
            new Breadcrumbs.BreadcrumbItem { Text = "Profil", Url = "/profile" },
            new Breadcrumbs.BreadcrumbItem { Text = "Promjena lozinke", CssClass = "text-red-500 font-bold" }
        };

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            if (authState.User?.Identity is null || !authState.User.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/login");
                return;
            }
        }

        private async Task ChangePasswordAsync()
        {
            // Validiraj formu (MudBlazor validacija)
            await _form.Validate();
            if (!_form.IsValid)
                return;

            // Dohvati userId iz claimova
            var authState = await AuthenticationStateTask;
            var userId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ErrorMessages.Clear();

            if (string.IsNullOrWhiteSpace(userId))
            {
                ErrorMessages.Add("Nije pronađen ID korisnika.");
                return;
            }

            // Pozovi servis koji sada sam dohvaća korisnika i izvršava promjenu lozinke
            var result = await PasswordService.ChangeOwnPasswordAsync(
                userId,
                ChangePasswordModel.OldPassword,
                ChangePasswordModel.NewPassword
            );

            if (!result.Succeeded)
            {
                // Prikaži sve greške
                foreach (var error in result.Errors)
                {
                    ErrorMessages.Add(error.Description);
                }
            }
            else
            {
                // Ako je promjena uspješna, preusmjeri natrag na profil
                Navigation.NavigateTo($"/profile/{userId}");
            }
        }
    }
}
