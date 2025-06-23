using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages.EditUser
{
    public partial class EditUserRoles : ComponentBase
    {
        [Parameter] public string UserId { get; set; } = null!;
        [Inject] public UserManager<IdentityUser> UserManager { get; set; } = null!;
        [Inject] public RoleManager<IdentityRole> RoleManager { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;

        // Lista naziva rola u kojima je korisnik trenutno
        private List<string> UserRoles { get; set; } = [];

        // Lista svih dostupnih rola dohvaćenih iz baze
        private List<string?> AllRoles { get; set; } = [];

        // Odabrana rola iz dropdowna
        protected string NewRoleName { get; set; } = "";

        private int RowsPerPage { get; set; } = 10;
        private List<string> ErrorMessages { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            await LoadUserRolesAsync();
            await LoadAllRolesAsync();
        }
        
        private void OnRoleChanged(string? value) => NewRoleName = value ?? "";

        private async Task LoadUserRolesAsync()
        {
            var user = await UserManager.FindByIdAsync(UserId);
            if (user is null)
            {
                Navigation.NavigateTo("/admin/users");
                return;
            }

            // Dohvati sve role u kojima je korisnik
            var roles = await UserManager.GetRolesAsync(user);
            UserRoles = roles.ToList();
        }

        private Task LoadAllRolesAsync()
        {
            // Dohvati sve dostupne role iz RoleManager-a.
            // Ako RoleManager.Roles podržava asinkrono dohvaćanje, možete koristiti ToListAsync()
            // Primjer: AllRoles = (await RoleManager.Roles.ToListAsync()).Select(r => r.Name).ToList();
            // Ovdje koristimo sinhrono dohvaćanje:
            AllRoles = RoleManager.Roles.Select(r => r.Name).ToList();
            return Task.CompletedTask;
        }

        protected async Task AddUserToRole()
        {
            ErrorMessages.Clear();

            if (string.IsNullOrWhiteSpace(NewRoleName))
            {
                ErrorMessages.Add("Odaberite rolu.");
                return;
            }

            var user = await UserManager.FindByIdAsync(UserId);
            if (user is null)
            {
                Navigation.NavigateTo("/admin/users");
                return;
            }

            var result = await UserManager.AddToRoleAsync(user, NewRoleName);
            if (!result.Succeeded)
            {
                ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
                return;
            }

            // Resetiraj odabranu rolu i ponovno učitaj popis rola u kojima je korisnik
            NewRoleName = "";
            await LoadUserRolesAsync();
        }

        protected async Task RemoveUserFromRole(string roleName)
        {
            ErrorMessages.Clear();

            if (roleName == RoleNames.Management)
            {
                ErrorMessages.Add("Rola 'Management' se ne može ukloniti.");
                return;
            }
            
            var user = await UserManager.FindByIdAsync(UserId);
            if (user is null)
            {
                Navigation.NavigateTo("/admin/users");
                return;
            }

            var result = await UserManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
                return;
            }

            // Osvježi popis rola
            await LoadUserRolesAsync();
        }
    }
}
