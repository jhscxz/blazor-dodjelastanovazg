using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji
{
    public partial class ClanEditFormDialog
    {
        [CascadingParameter] public required IMudDialogInstance MudDialog { get; set; }
        [Parameter] public SocijalniNatjecajClanDto NewClan { get; set; } = new();
        [Parameter] public long ZahtjevId { get; set; }

        private MudForm _form = null!;
        private bool IsPodnositelj => NewClan.Srodstvo == Enums.Srodstvo.PodnositeljZahtjeva;

        private async Task Submit()
        {
            await _form.Validate();
            if (_form.IsValid)
            {
                MudDialog.Close(DialogResult.Ok(NewClan)); 
            }
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }
        private DateTime? DatumRodjenjaProxy
        {
            get => NewClan.DatumRodjenja != default ? NewClan.DatumRodjenja.ToDateTime(new TimeOnly(0, 0)) : null;
            set
            {
                if (value.HasValue)
                    NewClan.DatumRodjenja = DateOnly.FromDateTime(value.Value);
            }
        }

    }
    
}