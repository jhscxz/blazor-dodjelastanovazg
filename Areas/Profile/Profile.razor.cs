using DodjelaStanovaZG.Areas.Profile.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Services;
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

    [Inject] public ApplicationDbContext DbContext { get; set; } = null!;
    [Inject] public UserManager<IdentityUser> UserManager { get; set; } = null!;
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Inject] public BreadcrumbService BreadcrumbService { get; set; } = null!;
    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new Breadcrumbs.BreadcrumbItem { Text = "Početna", Url = "/" },
        new Breadcrumbs.BreadcrumbItem { Text = "Moj profil", CssClass = "text-red-500 font-bold" }
    ];
    protected override async Task OnInitializedAsync()
    {

        var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == UserId);
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