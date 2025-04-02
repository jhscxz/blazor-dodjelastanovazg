using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages;

public partial class SocijalniNatjecajAdd : ComponentBase
{
    [Inject] public required ISocijalniNatjecajService SocijalniNatjecajService { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }

    [Parameter] public long NatjecajId { get; set; }

    protected SocijalniNatjecajDto ZahtjevModel { get; set; } = new();
    private MudForm _form = null!;

    [Required(ErrorMessage = "Ime i prezime su obavezni.")]
    protected string? ImePrezime { get; set; }

    [Required(ErrorMessage = "OIB je obavezan.")]
    protected string? Oib { get; set; }

    private DateTime? _datumPodnosenja;
    protected List<string> ErrorMessages { get; } = new();

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
        ErrorMessages.Clear();

        await _form.Validate();
        if (!_form.IsValid)
        {
            ErrorMessages.Add("Forma nije validna. Provjerite unesene podatke.");
            return;
        }

        try
        {
            await SocijalniNatjecajService.CreateAsync(ZahtjevModel, ImePrezime!, Oib!);
            Navigation.NavigateTo($"/socijalni/pregled/{NatjecajId}");
        }
        catch (Exception ex)
        {
            ErrorMessages.Add("Greška prilikom spremanja: " + ex.Message);
        }
    }

    private void Cancel() => Navigation.NavigateTo($"/socijalni/pregled/{NatjecajId}");
}
