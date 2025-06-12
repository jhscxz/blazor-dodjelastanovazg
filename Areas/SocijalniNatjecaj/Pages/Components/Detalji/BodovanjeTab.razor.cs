using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Models;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class BodovanjeTab : ComponentBase
{
    [Inject] public ISocijalniBodoviService BodoviService { get; set; } = default!;
    [Parameter] public long Id { get; set; }

    protected SocijalniNatjecajBodovi? Bodovi { get; set; } 
    protected List<BodovniRed> Redovi { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Bodovi = await BodoviService.GetByIdAsync(Id);

        if (Bodovi != null)
        {
            Redovi = new List<BodovniRed>
            {
                new("Stambeni status", "Stambeni status", Bodovi.BodoviStambeniStatus.ToString()),
                new("Sastav kućanstva", "Ukupno po članu", Bodovi.BodoviPoClanu.ToString()),
                new("Sastav kućanstva", "Maloljetna djeca", Bodovi.BodoviMaloljetni.ToString()),
                new("Sastav kućanstva", "Uzdržavana punoljetna djeca", Bodovi.BodoviPunoljetniUzdrzavani.ToString()),
                new("Sastav kućanstva", "Sastav", Bodovi.BodoviSastavKucanstva.ToString()),
                new("Socijalno-zdravstveni status", "Zajamčena minimalna naknada", Bodovi.BodoviZajamcenaNaknada.ToString()),
                new("Socijalno-zdravstveni status", "Roditelj njegovatelj", Bodovi.BodoviNjegovatelj.ToString()),
                new("Socijalno-zdravstveni status", "Doplatak za pomoć i njegu", Bodovi.BodoviDoplatakZaNjegu.ToString()),
                new("Socijalno-zdravstveni status", "Invalidnina (odrasli)", Bodovi.BodoviOdraslihInvalidnina.ToString()),
                new("Socijalno-zdravstveni status", "Invalidnina (maloljetni)", Bodovi.BodoviMaloljetnihInvalidnina.ToString()),
                new("Posebne okolnosti", "Žrtva nasilja", Bodovi.BodoviZrtvaNasilja.ToString()),
                new("Posebne okolnosti", "Alternativna skrb", Bodovi.BodoviAlternativnaSkrb.ToString()),
                new("Posebne okolnosti", "Iznad 55", Bodovi.BodoviIznad55.ToString()),
                new("Posebne okolnosti", "Branitelj (mjeseci * 0.5)", Bodovi.BodoviObrana.ToString("N1")),
                new("Posebne okolnosti", "Seksualno nasilje (DR)", Bodovi.BodoviSeksualnoNasilje.ToString()),
                new("Posebne okolnosti", "Civilni stradalnik (DR)", Bodovi.BodoviCivilniStradalnici.ToString()),
                new("Ukupno", "Ukupno bodova", Bodovi.UkupnoBodova.ToString())
            };
        }
    }
}
