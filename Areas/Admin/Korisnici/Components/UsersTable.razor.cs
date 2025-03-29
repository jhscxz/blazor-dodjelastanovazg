using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Components;

public partial class UsersTable : ComponentBase
{
    [Inject] public required UserManager<IdentityUser> UserManager { get; set; }
    [Inject] public required IUserService UserService { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    [Inject] public required BreadcrumbService BreadcrumbService { get; set; }

    private MudTable<UserDto> _table = null!;
    private string SearchText { get; set; } = "";
    protected string FilterRole { get; set; } = RoleNames.All;
    private static int RowsPerPage => 10;

    private List<Breadcrumbs.BreadcrumbItem> _breadcrumbItems = new();

    protected override void OnInitialized()
    {
        _breadcrumbItems = BreadcrumbService.Create("Početna", "Admin Nadzorna ploča", "Korisnici");
    }

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
