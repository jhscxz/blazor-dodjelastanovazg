using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Components;

public partial class UsersTable : ComponentBase
{
    [Inject] public required UserManager<IdentityUser> UserManager { get; set; }
    [Inject] public required IUserService UserService { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }

    private MudTable<UserDto> _table = null!;
    private string SearchText { get; set; } = "";
    private string FilterRole { get; set; } = RoleNames.All;
    private static int RowsPerPage => 10;
    
    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new(text: "Korisnici", cssClass: "text-red-500 font-bold"),
    ];

    private async Task OnValueChanged(string newValue)
    {
        SearchText = newValue;
        await _table.ReloadServerData();
    }

    private async Task OnFilterRoleChanged(string newRole)
    {
        FilterRole = newRole;
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

    protected void EditUser(string userId)
    {
        Navigation.NavigateTo($"/admin/users/edit/{userId}");
    }

    private void AddUser()
    {
        Navigation.NavigateTo("/admin/users/add");
    }
}
