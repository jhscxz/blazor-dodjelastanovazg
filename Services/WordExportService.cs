using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Services;

internal class WordExportService(IWebHostEnvironment env, ILogger<WordExportService> logger) : IWordExportService
{
    public async Task<byte[]> GenerirajIzvjestajAsync(SocijalniNatjecajZahtjev zahtjev, SocijalniNatjecajBodovi bodovi)
    {
        try
        {
            logger.LogInformation("Generiranje izvještaja za zahtjev {Id}", zahtjev.Id);

            var templatePath = Path.Combine(env.ContentRootPath, "Resources", "WordTemplates", "ZapisnikPredlozak.docx");

            await using var templateStream = File.OpenRead(templatePath);
            await using var memoryStream = new MemoryStream();
            await templateStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using (var wordDoc = WordprocessingDocument.Open(memoryStream, true))
            {
                var body = wordDoc.MainDocumentPart?.Document.Body ?? throw new InvalidOperationException("Ne mogu učitati sadržaj dokumenta.");

                var data = BuildPlaceholderDictionary(zahtjev, bodovi);

                foreach (var (placeholder, value) in data) ReplaceText(body, placeholder, value ?? string.Empty);

                wordDoc.MainDocumentPart?.Document.Save();
            }

            logger.LogInformation("Izvještaj generiran za zahtjev {Id}", zahtjev.Id);

            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Greška prilikom generiranja izvještaja za zahtjev {Id}", zahtjev.Id);
            throw;
        }
    }

    private static Dictionary<string, string?> BuildPlaceholderDictionary(SocijalniNatjecajZahtjev zahtjev, SocijalniNatjecajBodovi bodovi)
    {
        var podnositelj = zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
        var datumPodnosenja = DateOnly.FromDateTime(zahtjev.DatumPodnosenjaZahtjeva);
        var brojMaloljetnih = zahtjev.Clanovi.Count(c => c.DatumRodjenja.AddYears(18) > datumPodnosenja);
        var godinePodnositelj = podnositelj != null
            ? datumPodnosenja.Year - podnositelj.DatumRodjenja.Year -
              (datumPodnosenja < podnositelj.DatumRodjenja.AddYears(datumPodnosenja.Year - podnositelj.DatumRodjenja.Year) ? 1 : 0)
            : 0;

        string UkupnoBodovaLabel() => zahtjev.RezultatObrade switch
        {
            RezultatObrade.Neosnovan => "Neosnovan",
            RezultatObrade.Nepotpun => "Nepotpun",
            _ => bodovi.UkupnoBodova.ToString("0.##")
        };

        string F(float f, bool dec = false) => dec ? f.ToString("0.00") : f.ToString("0.##");

        return new Dictionary<string, string?>
        {
            ["{{ImePrezime}}"] = podnositelj?.ImePrezime,
            ["{{Klasa}}"] = zahtjev.KlasaPredmeta.ToString(),
            ["{{StatusZahtjeva}}"] = zahtjev.RezultatObrade.ToString(),
            ["{{Adresa}}"] = zahtjev.Adresa,
            ["{{NekretninaZG}}"] = zahtjev.PosjedujeNekretninuZg ? "Da" : "Ne",
            ["{{DatumRodjenja}}"] = podnositelj?.DatumRodjenja.ToString("dd.MM.yyyy"),
            ["{{Oib}}"] = podnositelj?.Oib,
            ["{{DatumPrebivalista}}"] = zahtjev.KucanstvoPodaci?.PrebivanjeOd?.ToString("dd.MM.yyyy"),
            ["{{UkupniPrihod}}"] = zahtjev.KucanstvoPodaci?.Prihod?.UkupniPrihodKucanstva.ToString("N2"),
            ["{{PostotakProsjeka}}"] = zahtjev.KucanstvoPodaci?.Prihod?.PostotakProsjeka?.ToString("N2"),
            ["{{PrihodPoClanu}}"] = zahtjev.KucanstvoPodaci?.Prihod?.PrihodPoClanu.ToString("N2"),
            ["{{IspunjavaUvjetPrihoda}}"] = zahtjev.KucanstvoPodaci?.Prihod?.IspunjavaUvjetPrihoda == true ? "Da" : "Ne",
            ["{{StambeniStatus}}"] = zahtjev.KucanstvoPodaci?.StambeniStatusKucanstva.GetDisplayName(),
            ["{{StambeniStatusBodova}}"] = F(bodovi.BodoviStambeniStatus),
            ["{{SastavKucanstva}}"] = zahtjev.KucanstvoPodaci?.SastavKucanstva.GetDisplayName(),
            ["{{SastavKucanstvaBodova}}"] = F(bodovi.BodoviSastavKucanstva),
            ["{{BrojClanova}}"] = zahtjev.Clanovi.Count.ToString(),
            ["{{BodoviClanova}}"] = F(bodovi.BodoviPoClanu),
            ["{{BrojMaloljetnih}}"] = brojMaloljetnih.ToString(),
            ["{{BodoviMaloljetni}}"] = F(bodovi.BodoviMaloljetni),
            ["{{BrojPunoljetniSkolovanje}}"] = zahtjev.BodovniPodaci?.BrojUzdrzavanePunoljetneDjece.ToString(),
            ["{{BodoviPunoljetniSkolovanje}}"] = F(bodovi.BodoviPunoljetniUzdrzavani),
            ["{{ZMN}}"] = zahtjev.BodovniPodaci?.PrimateljZajamceneMinimalneNaknade == true ? "Da" : "Ne",
            ["{{BodoviZMN}}"] = F(bodovi.BodoviZajamcenaNaknada),
            ["{{RoditeljNjegovatelj}}"] = zahtjev.BodovniPodaci?.StatusRoditeljaNjegovatelja == true ? "Da" : "Ne",
            ["{{BodoviRoditeljNjegovatelj}}"] = F(bodovi.BodoviNjegovatelj),
            ["{{DoplatakNjega}}"] = zahtjev.BodovniPodaci?.KorisnikDoplatkaZaPomoc == true ? "Da" : "Ne",
            ["{{BodoviDoplatakNjega}}"] = F(bodovi.BodoviDoplatakZaNjegu),
            ["{{OdrasliInvalidnina}}"] = zahtjev.BodovniPodaci?.BrojOdraslihKorisnikaInvalidnine.ToString(),
            ["{{BodoviOdrasliInvalidnina}}"] = F(bodovi.BodoviOdraslihInvalidnina),
            ["{{MaloljetniInvalidnina}}"] = zahtjev.BodovniPodaci?.BrojMaloljetnihKorisnikaInvalidnine.ToString(),
            ["{{BodoviMaloljetniInvalidnina}}"] = F(bodovi.BodoviMaloljetnihInvalidnina),
            ["{{ObiteljskoNasilje}}"] = zahtjev.BodovniPodaci?.ZrtvaObiteljskogNasilja == true ? "Da" : "Ne",
            ["{{BodoviObiteljskoNasilje}}"] = F(bodovi.BodoviZrtvaNasilja),
            ["{{AlternativnaSkrb}}"] = zahtjev.BodovniPodaci?.BrojOsobaUAlternativnojSkrbi.ToString(),
            ["{{BodoviAlternativnaSkrb}}"] = F(bodovi.BodoviAlternativnaSkrb),
            ["{{Iznad55}}"] = godinePodnositelj >= 55 ? "Da" : "Ne",
            ["{{BodoviIznad55}}"] = F(bodovi.BodoviIznad55),
            ["{{MjeseciBranitelj}}"] = zahtjev.BodovniPodaci?.BrojMjeseciObranaSuvereniteta.ToString(),
            ["{{BodoviBranitelj}}"] = F(bodovi.BodoviObrana, true),
            ["{{ZrtvaSeksualnogNasilja}}"] = zahtjev.BodovniPodaci?.BrojClanovaZrtavaSeksualnogNasilja.ToString(),
            ["{{BodoviZrtvaSeksualnogNasilja}}"] = F(bodovi.BodoviSeksualnoNasilje),
            ["{{CivilniStradalnik}}"] = zahtjev.BodovniPodaci?.BrojCivilnihStradalnika.ToString(),
            ["{{BodoviCivilniStradalnik}}"] = F(bodovi.BodoviCivilniStradalnici),
            ["{{Obradio}}"] = zahtjev.CreatedByUser?.UserName,
            ["{{DatumObrade}}"] = zahtjev.CreatedAt.ToShortDateString(),
            ["{{UkupnoBodova}}"] = UkupnoBodovaLabel()
        };
    }

    private static void ReplaceText(Body body, string placeholder, string? value)
    {
        foreach (var text in body.Descendants<Text>().Where(t => t.Text.Contains(placeholder))) text.Text = text.Text.Replace(placeholder, value ?? string.Empty);
    }
}
