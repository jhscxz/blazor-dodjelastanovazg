using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using MudBlazor;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages.Components.Detalji
{
    public partial class ClanFormDialog
    {
        [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }
        [Parameter] public SocijalniNatjecajClanDto NewClan { get; set; } = new();
        [Parameter] public long ZahtjevId { get; set; }
        private MudForm _form = null!;
        private DateTime? _datumRodjenja;
        
        private async Task Submit()
        {
            await _form.Validate();
            if (_form.IsValid)
            {
                NewClan.DatumRodjenja = DateOnly.FromDateTime(_datumRodjenja!.Value);
                MudDialog.Close(DialogResult.Ok(NewClan)); 
            }
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}