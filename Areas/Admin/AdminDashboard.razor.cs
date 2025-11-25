using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Admin;

public partial class AdminDashboard : ComponentBase
{
    protected List<AdminCard> AdminCards = [];

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.AdminDashboard();
    
    protected override void OnInitialized()
    {
        AdminCards =
        [
            new AdminCard
            {
                Title = "Upravljanje korisnicima",
                Description = "Upravljajte korisnicima, resetirajte lozinke, dodajte nove korisnike itd.",
                NavigateTo = "/admin/users",
                ButtonText = "ID NA UPRAVLJANJE KORISNICIMA",
                IconSvg = RenderUsersIcon()
            },
            new AdminCard
            {
                Title = "Upravljanje natječajima",
                Description = "Upravljajte natječajima, uredite popise i omogućite razne dodatne opcije.",
                NavigateTo = "/admin/natjecaji",
                ButtonText = "ID NA UPRAVLJANJE NATJEČAJIMA",
                IconSvg = RenderTendersIcon()
            }
        ];
    }

    private static RenderFragment RenderUsersIcon() => builder =>
    {
        builder.OpenElement(0, "svg");
        builder.AddMultipleAttributes(1, new Dictionary<string, object>
        {
            ["xmlns"] = "http://www.w3.org/2000/svg",
            ["fill"] = "none",
            ["viewBox"] = "0 0 24 24",
            ["stroke-width"] = "1.5",
            ["stroke"] = "currentColor",
            ["class"] = "w-4 h-4"
        });
        builder.AddMarkupContent(2, """
            <path stroke-linecap="round" stroke-linejoin="round"
                  d="M15 19.128a9.38 9.38 0 0 0 2.625.372
                     9.337 9.337 0 0 0 4.121-.952 
                     4.125 4.125 0 0 0-7.533-2.493M15 19.128v-.003
                     c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 0 1 
                     8.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0 1 
                     11.964-3.07M12 6.375a3.375 3.375 0 1 1-6.75 0 
                     3.375 3.375 0 0 1 6.75 0Zm8.25 2.25a2.625 2.625 0 1 1-5.25 0 
                     2.625 2.625 0 0 1 5.25 0Z" />
        """);
        builder.CloseElement();
    };

    private static RenderFragment RenderTendersIcon() => builder =>
    {
        builder.OpenElement(0, "svg");
        builder.AddMultipleAttributes(1, new Dictionary<string, object>
        {
            ["xmlns"] = "http://www.w3.org/2000/svg",
            ["fill"] = "none",
            ["viewBox"] = "0 0 24 24",
            ["stroke-width"] = "1.5",
            ["stroke"] = "currentColor",
            ["class"] = "w-4 h-4"
        });
        builder.AddMarkupContent(2, """
            <path stroke-linecap="round" stroke-linejoin="round"
                  d="M8.25 6.75h12M8.25 12h12m-12 5.25h12
                     M3.75 6.75h.007v.008H3.75V6.75Zm.375 0a.375.375 0 1 1-.75 0 
                     .375.375 0 0 1 .75 0ZM3.75 12h.007v.008H3.75V12Zm.375 0
                     a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm-.375 5.25h.007v.008H3.75v-.008Zm.375 0
                     a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Z" />
        """);
        builder.CloseElement();
    };

    protected class AdminCard
    {
        public string Title { get; init; } = "";
        public string Description { get; init; } = "";
        public string NavigateTo { get; init; } = "";
        public string ButtonText { get; init; } = "";
        public RenderFragment IconSvg { get; init; } = null!;
    }
}
