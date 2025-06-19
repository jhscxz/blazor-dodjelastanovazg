using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji;

public partial class BodovanjeTab : ComponentBase
{
    [Inject] public ISocijalniBodoviService BodoviService { get; set; } = default!;
    [Inject] public ISocijalniBodovnaGreskaService GreskaService { get; set; } = default!;

    [Inject] public IWordExportService WordExportService { get; set; } = default!;
    [Inject] public NavigationManager Nav { get; set; } = default!;
    [Parameter] public long Id { get; set; }

    protected SocijalniNatjecajBodovi? Bodovi { get; set; }
    protected List<BodovniRed> Redovi { get; set; } = new();
    protected List<SocijalniNatjecajBodovnaGreska> Greske { get; set; } = new();
    protected SocijalniNatjecajClan? Podnositelj { get; set; }

    protected override async Task OnInitializedAsync()
    {

        // Dohvati bodove (već imaju ZahtjevId)
        Bodovi = await BodoviService.GetByIdAsync(Id);
        Podnositelj = Bodovi?.Zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);

        if (Bodovi != null)
        {
            // Samo pročitaj postojeće greške iz baze
            Greske = await GreskaService.GetByZahtjevIdAsync(Bodovi.ZahtjevId);

            Redovi = new List<BodovniRed>
            {
                new("Stambeni status", "Stambeni status bodovi", Bodovi.BodoviStambeniStatus.ToString()),
                new("Sastav kućanstva", "Sastav kućanstva bodovi", Bodovi.BodoviSastavKucanstva.ToString()),
                new("Sastav kućanstva", "Ukupno bodova za broj članova kućanstva", Bodovi.BodoviPoClanu.ToString()),
                new("Sastav kućanstva", "Ukupno bodova za maloljetnu djecu", Bodovi.BodoviMaloljetni.ToString()),
                new("Sastav kućanstva", "Ukupno bodova za uzdržavanu punoljetnu djecu", Bodovi.BodoviPunoljetniUzdrzavani.ToString()),
                new("Socijalno-zdravstveni status", "Bodovi za zajamčenu minimalnu naknadu",
                    Bodovi.BodoviZajamcenaNaknada.ToString()),
                new("Socijalno-zdravstveni status", "Bodovi za status roditelja njegovatelja", Bodovi.BodoviNjegovatelj.ToString()),
                new("Socijalno-zdravstveni status", "Bodovi za članove koji su korisnici doplatka za pomoć i njegu",
                    Bodovi.BodoviDoplatakZaNjegu.ToString()),
                new("Socijalno-zdravstveni status", "Bodovi za odrasle članove koji su korisnici osobne invalidnine ili inkluzivnog dodatka",
                    Bodovi.BodoviOdraslihInvalidnina.ToString()),
                new("Socijalno-zdravstveni status", "Bodovi za maloljetne članove koji su korisnici osobne invalidnine ili inkluzivnog dodatka",
                    Bodovi.BodoviMaloljetnihInvalidnina.ToString()),
                new("Posebne okolnosti", "Bodovi za status žrtve obiteljskog nasilja", Bodovi.BodoviZrtvaNasilja.ToString()),
                new("Posebne okolnosti", "Bodovi za članove u dobi 18-29 koji su bili u sustavu alternativne skrbi", Bodovi.BodoviAlternativnaSkrb.ToString()),
                new("Posebne okolnosti", "Bodovi za podnositelja koji je stariji od 55 godina", Bodovi.BodoviIznad55.ToString()),
                new("Posebne okolnosti", "Bodovi za broj mjeseci proveden u obrani suvereniteta", Bodovi.BodoviObrana.ToString("N1")),
                new("Posebne okolnosti", "Bodovi za članove koji su žrtve seksualnog nasilja za vrijeme oružane agresije tijekom Domovinskog rata", Bodovi.BodoviSeksualnoNasilje.ToString()),
                new("Posebne okolnosti", "Bodovi za status stradalnika iz Domovinskog rata", Bodovi.BodoviCivilniStradalnici.ToString())
            };

            if (!Greske.Any())
            {
                Redovi.Add(new("Ukupno", "Ukupno bodova", Bodovi.UkupnoBodova.ToString()));
            }
        }


    }
    protected string GetStatusText(RezultatObrade? status)
    {
        return status switch
        {
            RezultatObrade.Nepotpun => "Nepotpun",
            RezultatObrade.Neosnovan => "Neosnovan",
            RezultatObrade.Osnovan => "Osnovan",
            null => "Nepoznato",
            _ => "Nepoznato"
        };
    }
    
    private void GenerirajZapisnik()
    {
        var url = Nav.BaseUri + $"api/export/zapisnik/{Id}";
        Nav.NavigateTo(url, forceLoad: true); // pokreće download
    }
}