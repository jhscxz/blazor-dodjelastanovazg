using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public class SocijalniNatjecajPregledBase : ComponentBase
{
    [Inject] public required IUnitOfWork UnitOfWork { get; set; }
    [Inject] public NavigationManager Navigation { get; set; } = null!;

    [Parameter] public long NatjecajId { get; set; }

    protected MudTable<SocijalniNatjecajZahtjevDto> Table = null!;
    protected List<SocijalniNatjecajZahtjevDto> Natjecaji { get; set; } = [];

    protected const int TotalColumns = 7;
    protected long? ExpandedRowId { get; set; }

    protected string? SearchText { get; set; }
    protected RezultatObrade? SelectedOsnovanost { get; set; }

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Pregled zapisa", CssClass = "text-red-500 font-bold" }
    ];

    protected void AddZahtjev() =>
        Navigation.NavigateTo($"/socijalni/dodaj/{NatjecajId}");

    protected void NavigateToDetails(SocijalniNatjecajZahtjevDto zahtjevDto) =>
        Navigation.NavigateTo($"/socijalni/detalji/{zahtjevDto.Id}");

    protected void ToggleExpand(long id) =>
        ExpandedRowId = (ExpandedRowId == id) ? null : id;

    protected bool IsRowExpanded(SocijalniNatjecajZahtjevDto row) =>
        row.Id == ExpandedRowId;

    protected string GetExpandIcon(SocijalniNatjecajZahtjevDto row) =>
        IsRowExpanded(row) ? Icons.Material.Filled.ExpandLess : Icons.Material.Filled.ExpandMore;

    protected async Task<TableData<SocijalniNatjecajZahtjevDto>> LoadServerData(TableState state, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.SocijalniZahtjevRead.GetPagedAsync(
            natjecajId: NatjecajId,
            page: state.Page,
            pageSize: state.PageSize,
            sortBy: state.SortLabel,
            sortDirection: state.SortDirection,
            search: SearchText,
            osnovanost: SelectedOsnovanost
        );

        var zahtjevIds = result.Items.Select(x => x.Id).ToList();

        var bodoviMap = (await UnitOfWork.SocijalniBodoviService
            .GetForZahtjeviAsync(zahtjevIds))
            .ToDictionary(x => x.ZahtjevId, x => x);

        foreach (var dto in result.Items)
        {
            if (bodoviMap.TryGetValue(dto.Id, out var bodovi))
                dto.Bodovi = bodovi;
        }

        return new TableData<SocijalniNatjecajZahtjevDto>
        {
            Items = result.Items,
            TotalItems = result.TotalCount
        };
    }

    protected async Task OnSearchChanged(string value)
    {
        SearchText = value;
        await Table.ReloadServerData();
    }

    protected async Task OnOsnovanostChanged(RezultatObrade? value)
    {
        SelectedOsnovanost = value;
        await Table.ReloadServerData();
    }

    #region Helpers

    protected string FormatPrihod(decimal? prihod) =>
        prihod.HasValue ? $"{prihod.Value:N2} €" : "–";

    protected string FormatString(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "–" : value;

    protected string FormatInt(int? value) =>
        value?.ToString() ?? "0";

    protected string FormatOsnovanost(RezultatObrade? rezultat) =>
        rezultat switch
        {
            RezultatObrade.Osnovan => "Osnovan",
            RezultatObrade.Neosnovan => "Neosnovan",
            RezultatObrade.Nepotpun => "Nepotpun",
            RezultatObrade.Greška => "Greška",
            _ => "–"
        };

    #endregion
}
