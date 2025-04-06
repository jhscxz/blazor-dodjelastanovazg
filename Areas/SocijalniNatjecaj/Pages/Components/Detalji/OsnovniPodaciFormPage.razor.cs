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
        private DateTime? _datumPodnosenja;
        private int? _toggleRezultat;
        private List<string> ErrorMessages { get; set; } = new List<string>(); // List to store error messages

        private List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems { get; } =
        [
            new() { Text = "Početna", Url = "/" },
            new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
            new() { Text = "Detalji zahtjeva", Url = "" },
            new() { Text = "Uredi osnovne podatke", CssClass = "text-red-500 font-bold" }
        ];

        protected override async Task OnInitializedAsync()
        {
            var zahtjev = await DetaljiService.GetZahtjevByIdAsync(ZahtjevId);

            _socijalniNatjecajModel = new SocijalniNatjecajOsnovnoEditDto
            {
                Id = zahtjev.Id,
                KlasaPredmeta = zahtjev.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = zahtjev.DatumPodnosenjaZahtjeva,
                Adresa = zahtjev.Adresa,
                RezultatObrade = zahtjev.RezultatObrade,
                NapomenaObrade = zahtjev.NapomenaObrade,
                NatjecajId = zahtjev.NatjecajId,
            };

            _datumPodnosenja = _socijalniNatjecajModel?.DatumPodnosenjaZahtjeva?.ToDateTime(new TimeOnly(0));

            if (_socijalniNatjecajModel != null)
                _toggleRezultat = (int?)_socijalniNatjecajModel.RezultatObrade;

            BreadcrumbItems[2].Url = $"/socijalni/detalji/{ZahtjevId}";
        }

        private async Task OnValidSubmit()
        {
            // Clear previous errors
            ErrorMessages.Clear();

            // Validate the form manually
            await _form.Validate();

            if (!_form.IsValid || _socijalniNatjecajModel is null)
            {
                // If form is invalid, add error message and do nothing
                ErrorMessages.Add("Forma nije ispravno popunjena. Provjerite označena polja.");
                return; // Prevent form submission
            }

            // If valid, proceed with saving
            _socijalniNatjecajModel.DatumPodnosenjaZahtjeva = _datumPodnosenja.HasValue
                ? DateOnly.FromDateTime(_datumPodnosenja.Value)
                : DateOnly.MinValue;

            // Prevent submission if no valid toggle selection is made
            if (_toggleRezultat == null)
            {
                ErrorMessages.Add("Morate odabrati jedan od rezultata obrade.");
                return; // Prevent form submission
            }
            
            if (_toggleRezultat.HasValue)
            {
                _socijalniNatjecajModel.RezultatObrade = (RezultatObrade)_toggleRezultat.Value;
            }

            var dto = new SocijalniNatjecajOsnovnoEditDto
            {
                Id = _socijalniNatjecajModel.Id,
                KlasaPredmeta = _socijalniNatjecajModel.KlasaPredmeta,
                DatumPodnosenjaZahtjeva = _socijalniNatjecajModel.DatumPodnosenjaZahtjeva,
                Adresa = _socijalniNatjecajModel.Adresa,
                RezultatObrade = _socijalniNatjecajModel.RezultatObrade,
                NapomenaObrade = _socijalniNatjecajModel.NapomenaObrade,
                NatjecajId = _socijalniNatjecajModel.NatjecajId,
            };

            // Proceed to save data if the form is valid
            await DetaljiService.UpdateOsnovniPodaciAsync(ZahtjevId, dto);
            Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=OsnovniPodaci");
        }

        private void OnCancel()
        {
            Navigation.NavigateTo($"/socijalni/detalji/{ZahtjevId}?tab=OsnovniPodaci");
        }
    }
}
