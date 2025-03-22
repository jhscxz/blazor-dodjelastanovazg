using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
    
namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Components
{
    public partial class UsersTable : ComponentBase
    {
        [Inject] public required UserManager<IdentityUser> UserManager { get; set; }
        [Inject] public required IUserService UserService { get; set; }
        [Inject] public required NavigationManager Navigation { get; set; }

        protected MudTable<UserDto> Table = default!;

        protected string SearchText { get; set; } = "";
        protected string FilterRole { get; set; } = RoleNames.All;
        protected int RowsPerPage { get; set; } = 5;

        protected async Task OnValueChanged(string newValue)
        {
            SearchText = newValue;
            await Table.ReloadServerData();
        }

        protected async Task OnFilterRoleChanged()
        {
            await Table.ReloadServerData();
        }

        // Potrebno 2 parametra: (TableState, CancellationToken) ako MudTable to traži
        protected async Task<TableData<UserDto>> LoadUsers(TableState state, CancellationToken cancellationToken)
        {
            // Dohvaćamo korisnike iz servisa
            var result = await UserService.GetUsersAsync(UserManager, SearchText, FilterRole, state, cancellationToken);

            return new TableData<UserDto>
            {
                Items = result.Items,
                TotalItems = result.TotalItems
            };
        }

        protected void EditUser(string userId)
        {
            Navigation.NavigateTo($"/admin/users/edit/{userId}");
        }

        // Ako imate i brisanje
        protected async Task DeleteUser(string userId)
        {
            bool success = await UserService.DeleteUserAsync(UserManager, userId);
            if (success)
            {
                await Table.ReloadServerData();
            }
        }
    }
}
