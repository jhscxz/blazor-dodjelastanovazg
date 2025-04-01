using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Pocetna;

public class HomePageBase : ComponentBase
{
    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new()
        {
            Text = "Početna",
            CssClass = "text-red-500 font-bold",
        }
    ];

    protected List<(string Title, string Description, string NavigateTo, string ButtonText, string Icon)> HomeCards =>
    [
        (
            "Socijalni natječaji",
            "Obrada socijalnog natječaja.",
            "/socijalni-natjecaj",
            "Idi na socijalne natječaje",
            Icons.Material.Filled.List
        ),
        (
            "Priuštivi natječaji",
            "Obrada priuštivog natječaja.",
            "/priuštivi-natjecaj",
            "Idi na priuštive natječeje",
            Icons.Material.Filled.List
        )
    ];
}