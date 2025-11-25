using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages;

public class SocijalniNatjecajPregledBase : ComponentBase
{
    [Inject] public required IUnitOfWork UnitOfWork { get; set; }
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public UserManager<IdentityUser> UserManager { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

    protected bool IsManagementUser { get; set; }
    [Parameter] public long NatjecajId { get; set; }
    protected MudTable<SocijalniNatjecajZahtjevDto> Table = null!;
    private long? ExpandedRowId { get; set; }
    protected string? SearchText { get; set; }
    protected RezultatObrade? SelectedOsnovanost { get; set; }

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } = BreadcrumbProvider.SocijalniPregled();

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

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            var identityUser = await UserManager.FindByNameAsync(user.Identity.Name!);
            if (identityUser is not null)
                IsManagementUser = await UserManager.IsInRoleAsync(identityUser, "Management");
        }
    }

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

    var bodoviMap = (await UnitOfWork.SocijalniBodoviService.GetForZahtjeviAsync(zahtjevIds))
        .ToDictionary(x => x.ZahtjevId, x => x);

    var bodovniMap = new Dictionary<long, SocijalniNatjecajBodovniPodaciDto>();
    foreach (var zahtjevId in zahtjevIds)
    {
        try
        {
            var bodovni = await UnitOfWork.SocijalniBodovniPodaciService.GetAsync(zahtjevId);
            bodovniMap[zahtjevId] = bodovni;
        }
        catch
        {
            // ignoriraj
        }
    }

    var clanoviMap = await UnitOfWork.SocijalniClanService.GetForZahtjeviAsync(zahtjevIds);

    foreach (var dto in result.Items)
    {
        // članovi
        if (clanoviMap.TryGetValue(dto.Id, out var clanovi))
            dto.Clanovi = clanovi;

        // bodovi
        if (bodoviMap.TryGetValue(dto.Id, out var bodovi))
            dto.Bodovi = bodovi;

        // bodovni podaci + kućanstvo + izračun
        if (bodovniMap.TryGetValue(dto.Id, out var bodovni))
        {
            var kucanstvo = await UnitOfWork.SocijalniKucanstvoService.GetAsync(dto.Id);
            dto.KucanstvoPodaci = kucanstvo;
            dto.BodovniPodaci = bodovni;

            var datum = dto.DatumPodnosenjaZahtjeva.HasValue
                ? DateOnly.FromDateTime(dto.DatumPodnosenjaZahtjeva.Value)
                : DateOnly.FromDateTime(DateTime.Today);

            var maloljetni = clanovi?
                .Count(c => c.DatumRodjenja.AddYears(18) > datum) ?? 0;

            var podnositelj = clanovi?.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
            var godine = podnositelj?.DatumRodjenja != null
                ? datum.Year - podnositelj.DatumRodjenja.Year -
                  (datum < podnositelj.DatumRodjenja.AddYears(datum.Year - podnositelj.DatumRodjenja.Year) ? 1 : 0)
                : 0;

            dto.Prihod = kucanstvo?.Prihod?.Adapt<SocijalniPrihodDto>() ?? new SocijalniPrihodDto();

            dto.BrojMaloljetneDjece = (byte)maloljetni;
            dto.PodnositeljIznad55 = godine >= 55;
        }
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
    
    protected void ExportExcel()
    {
        var url = Navigation.BaseUri + $"api/export/socijalni/{NatjecajId}/excel";
        if (SelectedOsnovanost.HasValue)
            url += $"?filter={SelectedOsnovanost}";
        Navigation.NavigateTo(url, forceLoad: true);
    }
    
    protected async Task BodujSve()
    {
        try
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.ObradiSveZahtjeveAsync(NatjecajId);
            Snackbar.Add("Bodovanje završeno", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
        }

        await Table.ReloadServerData();
    }

    #region Helpers

    protected static string FormatPrihod(decimal? prihod) =>
        prihod.HasValue ? $"{prihod.Value:N2} €" : "–";

    protected static string FormatString(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "–" : value;

    protected static string FormatInt(int? value) =>
        value?.ToString() ?? "0";

    protected static string FormatFloat(float? value) =>
        value?.ToString("0.##") ?? "0";
    
    protected static string FormatOsnovanost(RezultatObrade? rezultat) =>
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