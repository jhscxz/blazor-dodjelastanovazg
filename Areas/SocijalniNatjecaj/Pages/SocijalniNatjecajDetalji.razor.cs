using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public class SocijalniNatjecajDetaljiBase : ComponentBase
{
    [Inject] public required ISocijalniNatjecajDetaljiService SocijalniNatjecajDetaljiService { get; set; }
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Parameter] public long NatjecajId { get; set; }

    protected SocijalniNatjecajDto Detalji { get; set; } = new();

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", CssClass = "text-red-500 font-bold" }
    ];

    protected override async Task OnInitializedAsync()
    {
        Detalji = await SocijalniNatjecajDetaljiService.GetDetaljiAsync(NatjecajId);
    }
}