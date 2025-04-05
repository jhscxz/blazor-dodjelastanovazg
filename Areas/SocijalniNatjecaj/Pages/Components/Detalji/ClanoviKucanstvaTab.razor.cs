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

        private void EditClan(long id)
        {
            // Implementacija za uređivanje člana, ako je potrebno kasnije...
        }
    }
}
