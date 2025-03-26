using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using DodjelaStanovaZG.DTO;
using DodjelaStanovaZG.Services;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages.EditUser
{
    public partial class ChangePassword : ComponentBase
    {
        [Parameter] public string UserId { get; set; } = null!;
        [Inject] public IPasswordService PasswordService { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;

        private MudForm _form = null!;
        private bool _isValid;
        protected ResetPasswordDto ResetPasswordModel { get; set; } = new();
        protected List<string> ErrorMessages { get; set; } = new();

        protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = new List<Breadcrumbs.BreadcrumbItem>
        {
            new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
            new Breadcrumbs.BreadcrumbItem { Text = "Admin Nadzorna ploča", Url = "/admin" },
            new Breadcrumbs.BreadcrumbItem { Text = "Korisnici", Url = "/admin/users" },
            new Breadcrumbs.BreadcrumbItem { Text = "Promijeni lozinku", CssClass = "text-red-500 font-bold" }
        };

        protected override async Task OnInitializedAsync()
        {
            // Provjeri postoji li korisnik; ako ne, preusmjeri natrag na listu korisnika.
            var user = await PasswordService.GetUserByIdAsync(UserId);
            if (user == null)
            {
                Navigation.NavigateTo("/admin/users");
            }
        }

        protected async Task ChangePasswordAsync()
        {
            await _form.Validate();
            if (!_form.IsValid)
            {
                ErrorMessages.Clear();
                // Opcionalno: ErrorMessages.AddRange(_form.Errors);
                return;
            }

            var result = await PasswordService.ResetPasswordAsync(UserId, ResetPasswordModel.NewPassword);
            if (!result.Succeeded)
            {
                ErrorMessages.Clear();
                ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
                return;
            }

            Navigation.NavigateTo("/admin/users");
        }
    }
}