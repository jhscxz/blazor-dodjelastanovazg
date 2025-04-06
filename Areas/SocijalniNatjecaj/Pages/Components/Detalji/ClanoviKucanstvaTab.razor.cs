using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages.Components.Detalji
{
    public partial class ClanoviKucanstvaTab : ComponentBase
    {
        [Inject] public ISnackbar Snackbar { get; set; } = null!;
        [Inject] public ISocijalniNatjecajDetaljiService SocijalniNatjecajService { get; set; } = null!;

        [Parameter] public List<SocijalniNatjecajClanDto> Clanovi { get; set; } = [];
        [Parameter] public long Id { get; set; }

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

            var dialogReference = await DialogService.ShowAsync<ClanFormDialog>("Dodaj člana", parameters, options);
            var result = await dialogReference.Result;

            if (result is { Canceled: false, Data: SocijalniNatjecajClanDto noviClanDto })
            {
                var zahtjev = await SocijalniNatjecajService.GetZahtjevByIdAsync(Id);
                var noviClan = SocijalniNatjecajService.ConvertToEntity(noviClanDto, zahtjev);
                await SocijalniNatjecajService.AddClanAsync(noviClan);

                Clanovi.Add(noviClanDto);
                Snackbar.Add("Član kućanstva je uspješno dodan!", Severity.Success);
            }
        }

        private async Task EditClan(long id)
        {
            var clanZaUrediti = Clanovi.FirstOrDefault(c => c.Id == id);
            if (clanZaUrediti is null)
            {
                Snackbar.Add("Greška: Član nije pronađen.", Severity.Error);
                return;
            }

            var parameters = new DialogParameters
            {
                { "NewClan", new SocijalniNatjecajClanDto
                    {
                        Id = clanZaUrediti.Id,
                        ImePrezime = clanZaUrediti.ImePrezime,
                        Oib = clanZaUrediti.Oib,
                        Srodstvo = clanZaUrediti.Srodstvo,
                        DatumRodjenja = clanZaUrediti.DatumRodjenja
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

            var dialogReference = await DialogService.ShowAsync<ClanFormDialog>("Uredi člana", parameters, options);
            var result = await dialogReference.Result;

            if (result is { Canceled: false, Data: SocijalniNatjecajClanDto azuriraniClan })
            {
                var zahtjev = await SocijalniNatjecajService.GetZahtjevByIdAsync(Id);

                var entitet = zahtjev.Clanovi.FirstOrDefault(c => c.Id == azuriraniClan.Id);
                if (entitet != null)
                {
                    entitet.ImePrezime = azuriraniClan.ImePrezime;
                    entitet.Oib = azuriraniClan.Oib;
                    entitet.Srodstvo = azuriraniClan.Srodstvo;
                    entitet.DatumRodjenja = azuriraniClan.DatumRodjenja;
                }

                await SocijalniNatjecajService.SaveChangesAsync();

                var index = Clanovi.FindIndex(c => c.Id == azuriraniClan.Id);
                if (index >= 0)
                    Clanovi[index] = azuriraniClan;

                Snackbar.Add("Član kućanstva je ažuriran.", Severity.Success);
            }
        }

        private async Task DeleteClan(long id)
        {
            bool? confirmed = await DialogService.ShowMessageBox(
                "Brisanje člana",
                "Jeste li sigurni da želite obrisati ovog člana?",
                "Da", // yesText
                "Odustani", // noText
                options: new DialogOptions { CloseButton = true });


            if (confirmed != true)
                return;

            var zahtjev = await SocijalniNatjecajService.GetZahtjevByIdAsync(Id);
            var clanZaBrisanje = zahtjev.Clanovi.FirstOrDefault(c => c.Id == id);
            if (clanZaBrisanje is null)
            {
                Snackbar.Add("Član nije pronađen.", Severity.Error);
                return;
            }

            zahtjev.Clanovi.Remove(clanZaBrisanje);
            await SocijalniNatjecajService.SaveChangesAsync();

            Clanovi.RemoveAll(c => c.Id == id);
            Snackbar.Add("Član kućanstva je obrisan.", Severity.Success);
        }
    }
}
