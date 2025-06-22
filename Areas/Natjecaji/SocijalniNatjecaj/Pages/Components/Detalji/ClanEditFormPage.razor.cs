using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class ClanEditFormPage : ComponentBase
{
    [Parameter] public long ZahtjevId { get; set; }
    [Parameter] public long? ClanId { get; set; }
    [Inject] private IUnitOfWork UnitOfWork { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    private SocijalniNatjecajClanDto? _model;
    private MudForm _form = null!;
    private bool IsPodnositelj => _model?.Srodstvo == Srodstvo.PodnositeljZahtjeva;
    private DateTime? _datumRodjenja;
    private List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = [];
    protected override async Task OnInitializedAsync()
    {
        BreadcrumbItems = BreadcrumbProvider.ZahtjevEdit(ZahtjevId, "Uredi člana");
        
        SocijalniNatjecajZahtjevDto zahtjev;
        try
        {
            zahtjev = await UnitOfWork.SocijalniZahtjevRead.GetDetaljiAsync(ZahtjevId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
            Navigation.NavigateTo("/socijalni-natjecaj");
            return;
        }

        var clan = ClanId.HasValue
            ? zahtjev.Clanovi?.FirstOrDefault(c => c.Id == ClanId.Value)
            : null;

        _model = clan is null
            ? new SocijalniNatjecajClanDto { ZahtjevId = ZahtjevId }
            : new SocijalniNatjecajClanDto
            {
                Id = clan.Id,
                ImePrezime = clan.ImePrezime,
                Oib = clan.Oib,
                Srodstvo = clan.Srodstvo,
                DatumRodjenja = clan.DatumRodjenja,
                ZahtjevId = ZahtjevId
            };

        _datumRodjenja = _model.DatumRodjenja != default
            ? _model.DatumRodjenja.ToDateTime(new TimeOnly(0))
            : null;
    }

    private async Task Submit()
    {
        await _form.Validate();
        if (!_form.IsValid || _model is null)
            return;
        
        _model.DatumRodjenja = _datumRodjenja.HasValue
            ? DateOnly.FromDateTime(_datumRodjenja.Value)
            : default;

        try
        {
            if (ClanId.HasValue)
            {
                await UnitOfWork.SocijalniZahtjevProcessorService.UrediClanaIObradiAsync(_model);
            }
            else
            {
                await UnitOfWork.SocijalniZahtjevProcessorService.DodajClanaIObradiAsync(ZahtjevId, _model);
            }

            Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Clanovi");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
        }

        Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Clanovi");
    }

    private void Cancel()
    {
        Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=Clanovi");
    }
}