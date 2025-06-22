using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class ClanoviKucanstvaTab : ComponentBase
{
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IUnitOfWork UnitOfWork { get; set; } = null!;

    [Parameter] public long Id { get; set; }

    private SocijalniNatjecajZahtjevDto Zahtjev { get; set; } = null!;
    protected List<SocijalniNatjecajClanDto>? Clanovi { get; set; } = [];

    // forma i brisanje
    protected bool IsFormVisible { get; set; }
    protected bool IsEditMode { get; set; }
    protected SocijalniNatjecajClanDto CurrentClan { get; set; } = new();
    protected long? DeletePendingId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Zahtjev = await UnitOfWork.SocijalniZahtjevRead.GetDetaljiAsync(Id);
        Clanovi = Zahtjev.Clanovi;
    }

    protected void StartAddClan()
    {
        IsEditMode = false;
        CurrentClan = new SocijalniNatjecajClanDto { ZahtjevId = Id };
        IsFormVisible = true;
    }

    protected void StartEditClan(long id)
    {
        var clan = Clanovi?.FirstOrDefault(c => c.Id == id);
        if (clan is null)
        {
            Snackbar.Add("Greška: Član nije pronađen.", Severity.Error);
            return;
        }

        IsEditMode = true;
        CurrentClan = new SocijalniNatjecajClanDto
        {
            Id = clan.Id,
            ImePrezime = clan.ImePrezime,
            Oib = clan.Oib,
            Srodstvo = clan.Srodstvo,
            DatumRodjenja = clan.DatumRodjenja,
            ZahtjevId = Id
        };
        IsFormVisible = true;
    }

    protected async Task SubmitClanAsync(SocijalniNatjecajClanDto clan)
    {
        if (IsEditMode)
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.UrediClanaIObradiAsync(clan);
            if (Clanovi is not null)
            {
                var index = Clanovi.FindIndex(c => c.Id == clan.Id);
                if (index >= 0)
                    Clanovi[index] = clan;
            }
            Snackbar.Add("Član kućanstva je ažuriran.", Severity.Success);
        }
        else
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.DodajClanaIObradiAsync(Id, clan);
            Clanovi?.Add(clan);
            Snackbar.Add("Član kućanstva je uspješno dodan!", Severity.Success);
        }

        IsFormVisible = false;
        CurrentClan = new SocijalniNatjecajClanDto();
    }

    protected void CancelEdit()
    {
        IsFormVisible = false;
        CurrentClan = new SocijalniNatjecajClanDto();
    }

    protected void PromptDeleteClan(long id)
    {
        DeletePendingId = id;
    }

    protected async Task ConfirmDeleteClanAsync()
    {
        if (DeletePendingId is null)
            return;

        try
        {
            await UnitOfWork.SocijalniZahtjevProcessorService.ObrisiClanaIObradiAsync(Id, DeletePendingId.Value);
            Clanovi?.RemoveAll(c => c.Id == DeletePendingId.Value);
            Snackbar.Add("Član kućanstva je obrisan.", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
        }

        DeletePendingId = null;
    }

    protected void CancelDelete()
    {
        DeletePendingId = null;
    }
}
