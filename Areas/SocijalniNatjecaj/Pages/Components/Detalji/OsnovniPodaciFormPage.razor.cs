using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages.Components.Detalji
{
    public partial class OsnovniPodaciFormPage
    {
        [Parameter] public long ZahtjevId { get; set; }

        [Inject] private ISocijalniNatjecajDetaljiService DetaljiService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private SocijalniNatjecajOsnovnoEditDto? _socijalniNatjecajModel;
        private MudForm _form = null!;
        private List<string> ErrorMessages { get; set; } = new();
        private int? _toggleRezultat;
        private List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
        [
            new() { Text = "Početna", Url = "/" },
            new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
            new() { Text = "Detalji zahtjeva", Url = "" },
            new() { Text = "Uredi osnovne podatke", CssClass = "text-red-500 font-bold" }
        ];

        protected override async Task OnInitializedAsync()
        {
            var zahtjev = await DetaljiService.GetDetaljiAsync(ZahtjevId);
            
            Console.WriteLine("⚠️ DEBUG: DatumPodnosenja = " + zahtjev.DatumPodnosenjaZahtjeva);

            _socijalniNatjecajModel = new SocijalniNatjecajOsnovnoEditDto
            {
                Id = zahtjev.Id,
                KlasaPredmeta = zahtjev.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = zahtjev.DatumPodnosenjaZahtjeva,
                Adresa = zahtjev.Adresa,
                RezultatObrade = zahtjev.RezultatObrade,
                NapomenaObrade = zahtjev.NapomenaObrade,
                Email = zahtjev.Email,
                NatjecajId = zahtjev.NatjecajId,
            };
            _toggleRezultat = (int?)_socijalniNatjecajModel.RezultatObrade;
            BreadcrumbItems[2].Url = $"/socijalni/detalji/{ZahtjevId}";
        }

        private async Task OnValidSubmit()
        {
            ErrorMessages.Clear();

            await _form.Validate();

            if (!_form.IsValid || _socijalniNatjecajModel is null)
            {
                ErrorMessages.Add("Forma nije ispravno popunjena. Provjerite označena polja.");
                return;
            }

            if (_socijalniNatjecajModel.RezultatObrade is null)
            {
                ErrorMessages.Add("Morate odabrati jedan od rezultata obrade.");
                return;
            }

            if (_toggleRezultat != null) _socijalniNatjecajModel.RezultatObrade = (RezultatObrade)_toggleRezultat.Value;
            await DetaljiService.UpdateOsnovniPodaciAsync(ZahtjevId, _socijalniNatjecajModel);
            Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=OsnovniPodaci");
        }

        private void OnCancel()
        {
            Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=OsnovniPodaci");
        }
    }
}
