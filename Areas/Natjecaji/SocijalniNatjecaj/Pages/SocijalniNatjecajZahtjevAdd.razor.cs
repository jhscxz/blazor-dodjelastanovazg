using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages;

public partial class SocijalniNatjecajZahtjevAdd : ComponentBase, IDisposable
{
    [Inject] public required ISocijalniZahtjevProcessorService ZahtjevProcessorService { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; private set; } = [];
    [Parameter] public long NatjecajId { get; set; }
    [Inject] public required ISocijalniZahtjevFormHandler FormHandler { get; set; }
    protected SocijalniNatjecajZahtjevDto ZahtjevModel { get; set; } = new();
    protected List<string> ErrorMessages { get; } = [];
    private MudForm _form = null!;
    private int? _toggleRezultat;
    private bool _disposed;

    protected override void OnInitialized()
    {
        BreadcrumbItems = BreadcrumbProvider.ZahtjevDodaj(NatjecajId);
        ZahtjevModel.NatjecajId = NatjecajId;
        ZahtjevModel.BodovniPodaci = new();
        ZahtjevModel.KucanstvoPodaci = new();
        ZahtjevModel.Prihod = new();
    }

    private async Task SubmitForm()
    {
        if (_disposed) return;

        await _form.Validate();
        if (!_form.IsValid)
        {
            ErrorMessages.Add("Forma nije validna.");
            return;
        }

        var (success, errors) = await FormHandler.SubmitAsync(ZahtjevModel, _toggleRezultat);
        if (!success)
        {
            ErrorMessages.AddRange(errors);
        }
    }
    
    private void Cancel() => Navigation.NavigateTo($"/socijalni/pregled/{NatjecajId}");
    public void Dispose() => _disposed = true;
}