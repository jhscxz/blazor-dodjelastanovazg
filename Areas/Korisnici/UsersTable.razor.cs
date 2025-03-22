using DodjelaStanovaZG.DTO;
using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Korisnici
{
    public partial class UsersTableBase : ComponentBase
    {
        [Inject] public UserManager<IdentityUser> UserManager { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }

        protected MudTable<UserDto> table;

        protected string SearchText { get; set; } = "";
        protected string FilterRole { get; set; } = RoleNames.All;
        protected int RowsPerPage { get; set; } = 5;

        protected async Task OnValueChanged(string newValue)
        {
            SearchText = newValue;
            if (table != null)
            {
                await table.ReloadServerData();
            }
        }

        protected async Task OnFilterRoleChanged()
        {
            if (table != null)
            {
                await table.ReloadServerData();
            }
        }

        protected async Task<TableData<UserDto>> LoadUsers(TableState state, CancellationToken cancellationToken)
        {
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

        protected async Task DeleteUser(string userId)
        {
            bool success = await UserService.DeleteUserAsync(UserManager, userId);
            if (success && table != null)
            {
                await table.ReloadServerData();
            }
        }
    }
}