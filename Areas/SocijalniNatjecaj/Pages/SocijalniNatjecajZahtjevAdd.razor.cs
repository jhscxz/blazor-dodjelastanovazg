using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public partial class SocijalniNatjecajZahtjevAdd : ComponentBase, IDisposable
{
    [Inject] public required IUnitOfWork UnitOfWork { get; set; }

    [Inject] public required NavigationManager Navigation { get; set; }

    [Parameter] public long NatjecajId { get; set; }

    protected SocijalniNatjecajZahtjevDto ZahtjevModel { get; set; } = new()
    {
        Bodovni = new SocijalniBodovniDto(),
        DatumPodnosenjaZahtjeva = null,
        ImePrezime = null,
        Oib = null
    };

    private MudForm _form = null!;
    protected List<string> ErrorMessages { get; } = new();

    private int? _toggleRezultat;

    private bool _disposed;

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Dodaj zahtjev", CssClass = "text-red-500 font-bold" },
    ];

    protected override void OnInitialized()
    {
        ZahtjevModel.NatjecajId = NatjecajId;
    }

    private async Task SubmitForm()
    {
        if (_disposed) return;

        ErrorMessages.Clear();

        // Validacija forme
        await _form.Validate();
        if (!_form.IsValid)
        {
            ErrorMessages.Add("Forma nije validna. Provjerite unesene podatke.");
            return;
        }

        // Validacija dodatnih polja
        if (_toggleRezultat == null)
            ErrorMessages.Add("Rezultat obrade je obavezan.");

        if (!string.IsNullOrWhiteSpace(ErrorMessages.FirstOrDefault()))
            return;

        if (_toggleRezultat != null) ZahtjevModel.RezultatObrade = (RezultatObrade)_toggleRezultat.Value;

        try
        {
            var newId = await UnitOfWork.SocijalniNatjecajService.CreateAsync(ZahtjevModel, ZahtjevModel.ImePrezime, ZahtjevModel.Oib);

            if (!_disposed)
                Navigation.NavigateTo($"/socijalni/detalji/{newId}");
        }
        catch (Exception ex)
        {
            if (!_disposed)
                ErrorMessages.Add("Greška prilikom spremanja: " + ex.Message);
        }
    }

    private void Cancel()
    {
        if (!_disposed)
            Navigation.NavigateTo($"/socijalni/pregled/{NatjecajId}");
    }

    public void Dispose()
    {
        _disposed = true;
    }
}
