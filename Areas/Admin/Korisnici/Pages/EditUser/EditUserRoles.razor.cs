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
        protected List<string> UserRoles { get; set; } = new();

        // Lista svih dostupnih rola dohvaćenih iz baze
        protected List<string> AllRoles { get; set; } = new();

        // Odabrana rola iz dropdowna
        protected string NewRoleName { get; set; } = "";

        protected int RowsPerPage { get; set; } = 10;
        protected List<string> ErrorMessages { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadUserRolesAsync();
            await LoadAllRolesAsync();
        }

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

        private async Task LoadAllRolesAsync()
        {
            // Dohvati sve dostupne role iz RoleManager-a.
            // Ako RoleManager.Roles podržava asinkrono dohvaćanje, možete koristiti ToListAsync()
            // Primjer: AllRoles = (await RoleManager.Roles.ToListAsync()).Select(r => r.Name).ToList();
            // Ovdje koristimo sinhrono dohvaćanje:
            AllRoles = RoleManager.Roles.Select(r => r.Name).ToList();
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
