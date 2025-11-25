using DodjelaStanovaZG.Areas.Profile.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Areas.Profile;

public partial class Profile : ComponentBase
{
    [Parameter] public string UserId { get; set; } = "";
    private ProfileDto _profileModel = new();
    private bool _isLoaded;
    private List<string> _userRoles = [];
    [Inject] public IDbContextFactory<ApplicationDbContext> ContextFactory { get; set; } = null!;
    [Inject] public UserManager<IdentityUser> UserManager { get; set; } = null!;
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.Profile();
    protected override async Task OnInitializedAsync()
    {

        await using var context = await ContextFactory.CreateDbContextAsync();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
        if (user == null)
        {
            Navigation.NavigateTo("/profile");
            return;
        }

        _profileModel = new ProfileDto
        {
            Id = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? ""
        };

        _userRoles = (await UserManager.GetRolesAsync(user)).ToList();
        _isLoaded = true;
    }

    private void NavigateToChangePassword()
    {
        Navigation.NavigateTo("/profile/change-password");
    }
}