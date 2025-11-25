using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class BodovanjeTab : ComponentBase
{
    [Inject] public ISocijalniBodoviService BodoviService { get; set; } = null!;
    [Inject] public ISocijalniBodovnaGreskaService GreskaService { get; set; } = null!;
    [Inject] public NavigationManager Nav { get; set; } = null!;
    [Parameter] public long Id { get; set; }
    private SocijalniNatjecajBodovi? Bodovi { get; set; }
    private List<BodovniRed> Redovi { get; set; } = [];
    private List<SocijalniNatjecajBodovnaGreska> Greske { get; set; } = [];
    private SocijalniNatjecajClan? Podnositelj { get; set; }

    protected override async Task OnInitializedAsync()
    {

        Bodovi = await BodoviService.GetByIdAsync(Id);
        Podnositelj = Bodovi?.Zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);

        if (Bodovi != null)
        {
            Greske = await GreskaService.GetByZahtjevIdAsync(Bodovi.ZahtjevId);

            Redovi =
            [
                new BodovniRed("Stambeni status", "Stambeni status bodovi", Bodovi.BodoviStambeniStatus.ToString()),
                new BodovniRed("Sastav kućanstva", "Sastav kućanstva bodovi", Bodovi.BodoviSastavKucanstva.ToString()),
                new BodovniRed("Sastav kućanstva", "Ukupno bodova za broj članova kućanstva", Bodovi.BodoviPoClanu.ToString()),
                new BodovniRed("Sastav kućanstva", "Ukupno bodova za maloljetnu djecu", Bodovi.BodoviMaloljetni.ToString()),
                new BodovniRed("Sastav kućanstva", "Ukupno bodova za uzdržavanu punoljetnu djecu",
                    Bodovi.BodoviPunoljetniUzdrzavani.ToString()),
                new BodovniRed("Socijalno-zdravstveni status", "Bodovi za zajamčenu minimalnu naknadu",
                    Bodovi.BodoviZajamcenaNaknada.ToString()),

                new BodovniRed("Socijalno-zdravstveni status", "Bodovi za status roditelja njegovatelja",
                    Bodovi.BodoviNjegovatelj.ToString()),
                new BodovniRed("Socijalno-zdravstveni status", "Bodovi za članove koji su korisnici doplatka za pomoć i njegu",
                    Bodovi.BodoviDoplatakZaNjegu.ToString()),

                new BodovniRed("Socijalno-zdravstveni status",
                    "Bodovi za odrasle članove koji su korisnici osobne invalidnine ili inkluzivnog dodatka",
                    Bodovi.BodoviOdraslihInvalidnina.ToString()),

                new BodovniRed("Socijalno-zdravstveni status",
                    "Bodovi za maloljetne članove koji su korisnici osobne invalidnine ili inkluzivnog dodatka",
                    Bodovi.BodoviMaloljetnihInvalidnina.ToString()),

                new BodovniRed("Posebne okolnosti", "Bodovi za status žrtve obiteljskog nasilja",
                    Bodovi.BodoviZrtvaNasilja.ToString()),
                new BodovniRed("Posebne okolnosti", "Bodovi za članove u dobi 18-29 koji su bili u sustavu alternativne skrbi",
                    Bodovi.BodoviAlternativnaSkrb.ToString()),
                new BodovniRed("Posebne okolnosti", "Bodovi za podnositelja koji je stariji od 55 godina",
                    Bodovi.BodoviIznad55.ToString()),
                new BodovniRed("Posebne okolnosti", "Bodovi za broj mjeseci proveden u obrani suvereniteta",
                    Bodovi.BodoviObrana.ToString("N1")),
                new BodovniRed("Posebne okolnosti",
                    "Bodovi za članove koji su žrtve seksualnog nasilja za vrijeme oružane agresije tijekom Domovinskog rata",
                    Bodovi.BodoviSeksualnoNasilje.ToString()),
                new BodovniRed("Posebne okolnosti", "Bodovi za status stradalnika iz Domovinskog rata",
                    Bodovi.BodoviCivilniStradalnici.ToString())
            ];

            if (Greske.Count == 0)
            {
                Redovi.Add(new BodovniRed("Ukupno", "Ukupno bodova", Bodovi.UkupnoBodova.ToString()));
            }
        }


    }

    private static string GetStatusText(RezultatObrade? status)
    {
        return status switch
        {
            RezultatObrade.Nepotpun => "Nepotpun",
            RezultatObrade.Neosnovan => "Neosnovan",
            RezultatObrade.Osnovan => "Osnovan",
            RezultatObrade.Greška => "Greška",
            _ => "Nepoznato"
        };
    }
    
    private void GenerirajZapisnik()
    {
        var url = Nav.BaseUri + $"api/export/zapisnik/{Id}";
        Nav.NavigateTo(url, forceLoad: true); // pokreće download
    }
}