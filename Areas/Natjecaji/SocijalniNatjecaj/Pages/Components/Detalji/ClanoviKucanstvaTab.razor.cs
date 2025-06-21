using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji
{
    public partial class ClanoviKucanstvaTab : ComponentBase
    {
        [Inject] public ISnackbar Snackbar { get; set; } = null!;
        [Inject] public IUnitOfWork UnitOfWork { get; set; } = null!;
        [Parameter] public long Id { get; set; }
        private SocijalniNatjecajZahtjevDto Zahtjev { get; set; } = null!;
        protected List<SocijalniNatjecajClanDto>? Clanovi { get; set; } = [];

        private async Task AddClan()
        {
            var parameters = new DialogParameters
            {
                { "NewClan", new SocijalniNatjecajClanDto() },
                { "ZahtjevId", Id }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialogReference = await DialogService.ShowAsync<ClanEditFormDialog>("Dodaj člana", parameters, options);
            var result = await dialogReference.Result;

            if (result is { Canceled: false, Data: SocijalniNatjecajClanDto noviClanDto })
            {
                await UnitOfWork.SocijalniZahtjevProcessor.DodajClanaIObradiAsync(Id, noviClanDto);

                Clanovi?.Add(noviClanDto);
                Snackbar.Add("Član kućanstva je uspješno dodan!", Severity.Success);
            }
        }

        private async Task EditClan(long id)
        {
            var clanZaUrediti = Clanovi?.FirstOrDefault(c => c.Id == id);
            if (clanZaUrediti is null)
            {
                Snackbar.Add("Greška: Član nije pronađen.", Severity.Error);
                return;
            }

            var parameters = new DialogParameters
            {
                {
                    "NewClan", new SocijalniNatjecajClanDto
                    {
                        Id = clanZaUrediti.Id,
                        ImePrezime = clanZaUrediti.ImePrezime,
                        Oib = clanZaUrediti.Oib,
                        Srodstvo = clanZaUrediti.Srodstvo,
                        DatumRodjenja = clanZaUrediti.DatumRodjenja,
                        ZahtjevId = Id
                    }
                },
                { "ZahtjevId", Id }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialogReference = await DialogService.ShowAsync<ClanEditFormDialog>("Uredi člana", parameters, options);
            var result = await dialogReference.Result;

            if (result is { Canceled: false, Data: SocijalniNatjecajClanDto azuriraniClan })
            {
                await UnitOfWork.SocijalniZahtjevProcessor.UrediClanaIObradiAsync(azuriraniClan);

                if (Clanovi != null)
                {
                    var index = Clanovi.FindIndex(c => c.Id == azuriraniClan.Id);
                    if (index >= 0)
                        Clanovi[index] = azuriraniClan;
                }

                Snackbar.Add("Član kućanstva je ažuriran.", Severity.Success);
            }
        }

        private async Task DeleteClan(long id)
        {
            bool? confirmed = await DialogService.ShowMessageBox(
                "Brisanje člana",
                "Jeste li sigurni da želite obrisati ovog člana?",
                "Da",
                "Odustani",
                options: new DialogOptions { CloseButton = true });

            if (confirmed != true)
                return;

            try
            {
                await UnitOfWork.SocijalniZahtjevProcessor.ObrisiClanaIObradiAsync(Id, id);

                Clanovi?.RemoveAll(c => c.Id == id);
                Snackbar.Add("Član kućanstva je obrisan.", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Zahtjev = await UnitOfWork.SocijalniZahtjevRead.GetDetaljiAsync(Id);
            Clanovi = Zahtjev.Clanovi;
        }
    }
}
