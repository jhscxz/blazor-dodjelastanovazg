using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Pages;

public partial class Users : ComponentBase
{
    [Inject] private UserManager<IdentityUser> UserManager { get; set; } = null!;
    [Inject] private IUserService UserService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    private MudTable<UserDto> _table = null!;
    private string SearchText { get; set; } = "";
    private string FilterRole { get; set; } = RoleNames.All;
    private static int RowsPerPage => 10;

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.AdminUsers();

    private async Task OnValueChanged(string newValue)
    {
        SearchText = newValue;
        await _table.ReloadServerData();
    }

    private async Task OnFilterRoleChanged(string? newRole)
    {
        FilterRole = newRole!;
        await _table.ReloadServerData();
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

    private void EditUser(string userId)
    {
        Navigation.NavigateTo($"/admin/users/edit/{userId}");
    }

    private void AddUser()
    {
        Navigation.NavigateTo("/admin/users/add");
    }
}
