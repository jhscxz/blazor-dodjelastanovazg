using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages.EditUser
{
    public partial class ChangePassword : ComponentBase
    {
        [Parameter] public string UserId { get; set; } = null!;
        [Inject] public UserManager<IdentityUser> UserManager { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;

        private MudForm _form = null!;
        private bool _isValid;

        protected ChangePasswordInputModel ChangePasswordModel { get; set; } = new();
        protected List<string> ErrorMessages { get; set; } = new();
        
        protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = new List<Breadcrumbs.BreadcrumbItem>
        {
            new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
            new Breadcrumbs.BreadcrumbItem { Text = "Admin Nadzorna ploča", Url = "/admin" },
            new Breadcrumbs.BreadcrumbItem { Text = "Korisnici", Url = "/admin/users" },
            new Breadcrumbs.BreadcrumbItem { Text = "Promijeni lozinku" }
        };
        protected override async Task OnInitializedAsync()
        {
            // Opcionalno: provjera postoji li user
            var user = await UserManager.FindByIdAsync(UserId);
            if (user == null)
            {
                Navigation.NavigateTo("/admin/users");
            }
        }

        protected async Task ChangePasswordAsync()
        {
            await _form.Validate();
            if (!_isValid)
            {
                ErrorMessages.Clear();
                ErrorMessages.Add("Unesite ispravne podatke.");
                return;
            }

            // Provjera da li su unesene lozinke jednake
            if (ChangePasswordModel.NewPassword != ChangePasswordModel.ConfirmPassword)
            {
                ErrorMessages.Clear();
                ErrorMessages.Add("Lozinke se ne podudaraju.");
                return;
            }

            var user = await UserManager.FindByIdAsync(UserId);
            if (user == null)
            {
                Navigation.NavigateTo("/admin/users");
                return;
            }

            // Generiraj token za resetiranje lozinke
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var result = await UserManager.ResetPasswordAsync(user, token, ChangePasswordModel.NewPassword);
            if (!result.Succeeded)
            {
                ErrorMessages.Clear();
                ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
                return;
            }

            // Nakon uspješne promjene lozinke, preusmjeri natrag na listu korisnika
            Navigation.NavigateTo("/admin/users");
        }

        public class ChangePasswordInputModel
        {
            [Required(ErrorMessage = "Nova lozinka je obavezna.")]
            [MinLength(6, ErrorMessage = "Lozinka mora imati najmanje 6 znakova.")]
            public string NewPassword { get; set; } = "";

            [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
            [Compare("NewPassword", ErrorMessage = "Lozinke se ne podudaraju.")]
            public string ConfirmPassword { get; set; } = "";
        }
    }
}
