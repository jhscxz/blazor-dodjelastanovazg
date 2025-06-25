#nullable enable

using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class OsnovniPodaciEditFormPage
{
    [Parameter] public long ZahtjevId { get; set; }

    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IUnitOfWork UnitOfWork { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private SocijalniNatjecajOsnovnoEditDto? _socijalniNatjecajModel;
    private MudForm? _form;
    private readonly List<string> _errorMessages = [];
    private int? _toggleRezultat;
    private List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbItems = BreadcrumbProvider.ZahtjevEdit(ZahtjevId, "Uredi osnovne podatke");
        
        var zahtjev = await UnitOfWork.SocijalniZahtjevRead.GetDetaljiAsync(ZahtjevId);

        _socijalniNatjecajModel = new SocijalniNatjecajOsnovnoEditDto
        {
            Id = zahtjev.Id,
            KlasaPredmeta = zahtjev.KlasaPredmeta,
            DatumPodnosenjaZahtjeva = zahtjev.DatumPodnosenjaZahtjeva,
            Adresa = zahtjev.Adresa,
            RezultatObrade = zahtjev.RezultatObrade,
            NapomenaObrade = zahtjev.NapomenaObrade,
            Email = zahtjev.Email,
            PosjedujeNekretninuZg = zahtjev.PosjedujeNekretninuZg,
            NatjecajId = zahtjev.NatjecajId,
            RowVersion = zahtjev.RowVersion,
        };

        _toggleRezultat = (int?)_socijalniNatjecajModel.RezultatObrade;
    }

    private async Task Submit()
    {
        _errorMessages.Clear();

        if (_form is null)
        {
            _errorMessages.Add("Forma nije inicijalizirana.");
            return;
        }

        await _form.Validate();

        if (!_form.IsValid || _socijalniNatjecajModel is null)
        {
            _errorMessages.Add("Forma nije ispravno popunjena. Provjerite označena polja.");
            return;
        }

        if (_socijalniNatjecajModel.RezultatObrade is null)
        {
            _errorMessages.Add("Morate odabrati jedan od rezultata obrade.");
            return;
        }

        if (_toggleRezultat.HasValue)
            _socijalniNatjecajModel.RezultatObrade = (RezultatObrade)_toggleRezultat.Value;

        try
        {
            await UnitOfWork.SocijalniZahtjevProcessorService
                .AzurirajOsnovnoIObradiAsync(ZahtjevId, _socijalniNatjecajModel);

            Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=OsnovniPodaci");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
        }
    }

    private void Cancel()
    {
        Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=OsnovniPodaci");
    }
}
