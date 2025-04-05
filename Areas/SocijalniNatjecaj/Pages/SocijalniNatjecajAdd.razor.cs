using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public partial class SocijalniNatjecajAdd : ComponentBase, IDisposable
{
    [Inject] public required ISocijalniNatjecajService SocijalniNatjecajService { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }

    [Parameter] public long NatjecajId { get; set; }

    protected SocijalniNatjecajDto ZahtjevModel { get; set; } = new()
    {
        Bodovni = new SocijalniBodovniDto()
    };

    private MudForm _form = null!;

    [Required(ErrorMessage = "Ime i prezime su obavezni.")]
    protected string? ImePrezime { get; set; }
    protected string? Oib { get; set; }

    private DateTime? _datumPodnosenja;
    protected List<string> ErrorMessages { get; } = new();

    private int? _toggle_rezultat;

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
        _datumPodnosenja = null;
    }

    private async Task SubmitForm()
    {
        if (_disposed) return;

        ErrorMessages.Clear();

        await _form.Validate();
        if (!_form.IsValid)
        {
            ErrorMessages.Add("Forma nije validna. Provjerite unesene podatke.");
            return;
        }

        if (_datumPodnosenja == null)
        {
            ErrorMessages.Add("Datum podnošenja zahtjeva je obavezan.");
            return;
        }

        if (!_toggle_rezultat.HasValue)
        {
            ErrorMessages.Add("Rezultat obrade je obavezan.");
            return;
        }

        // Mapiramo odabir iz toggle grupe u DTO
        ZahtjevModel.RezultatObrade = (RezultatObrade)_toggle_rezultat.Value;
        ZahtjevModel.DatumPodnosenjaZahtjeva = DateOnly.FromDateTime(_datumPodnosenja.Value);

        try
        {
            await SocijalniNatjecajService.CreateAsync(ZahtjevModel, ImePrezime!, Oib!);
            if (!_disposed)
                Navigation.NavigateTo($"/socijalni/pregled/{NatjecajId}");
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
