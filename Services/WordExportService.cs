using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services.IServices;

namespace DodjelaStanovaZG.Services;

public class WordExportService(IWebHostEnvironment env) : IWordExportService
{
    public async Task<byte[]> GenerirajIzvjestajAsync(SocijalniNatjecajZahtjev zahtjev, SocijalniNatjecajBodovi bodovi)
    {
        var templatePath = Path.Combine(env.ContentRootPath, "Resources", "WordTemplates", "ZapisnikPredlozak.docx");
        var tempPath = Path.GetTempFileName();

        File.Copy(templatePath, tempPath, overwrite: true);

        using (var wordDoc = WordprocessingDocument.Open(tempPath, true))
        {
            var body = wordDoc.MainDocumentPart?.Document?.Body
                       ?? throw new InvalidOperationException("Ne mogu učitati sadržaj dokumenta.");

            var podnositelj = zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
            var datumPodnosenja = DateOnly.FromDateTime(zahtjev.DatumPodnosenjaZahtjeva);
            var brojMaloljetnih = zahtjev.Clanovi.Count(c => c.DatumRodjenja.AddYears(18) > datumPodnosenja);
            var godinePodnositelj = podnositelj != null
                ? datumPodnosenja.Year - podnositelj.DatumRodjenja.Year - (datumPodnosenja < podnositelj.DatumRodjenja.AddYears(datumPodnosenja.Year - podnositelj.DatumRodjenja.Year) ? 1 : 0)
                : 0;

            // Uvjet: svi bodovi su 0 osim ukupnog bodova koji pokazuje status ako je neosnovan ili nepotpun
            var status = zahtjev.RezultatObrade;

            string BodoviOrZero(object? val)
                => status is RezultatObrade.Neosnovan or RezultatObrade.Nepotpun ? "0"
                : (val switch
                {
                    double d => d.ToString("N2"),
                    float f => f.ToString("N2"),
                    int i => i.ToString(),
                    byte b => b.ToString(),
                    _ => val?.ToString() ?? "0"
                });

            string UkupnoBodovaLabel()
                => status is RezultatObrade.Neosnovan ? "Neosnovan"
                : status is RezultatObrade.Nepotpun ? "Nepotpun"
                : bodovi.UkupnoBodova.ToString();

            var data = new Dictionary<string, string?>
            {
                ["{{ImePrezime}}"] = podnositelj?.ImePrezime ?? "",
                ["{{Klasa}}"] = zahtjev.KlasaPredmeta.ToString() ?? "",
                ["{{StatusZahtjeva}}"] = zahtjev.RezultatObrade.ToString(),
                ["{{Adresa}}"] = zahtjev.Adresa ?? "",
                ["{{DatumRodjenja}}"] = podnositelj?.DatumRodjenja.ToString("dd.MM.yyyy"),
                ["{{Oib}}"] = podnositelj?.Oib ?? "",
                ["{{DatumPrebivalista}}"] = zahtjev.KucanstvoPodaci?.PrebivanjeOd?.ToString("dd.MM.yyyy"),
                ["{{UkupniPrihod}}"] = zahtjev.KucanstvoPodaci?.Prihod.UkupniPrihodKucanstva.ToString("N2"),
                ["{{PostotakProsjeka}}"] = zahtjev.KucanstvoPodaci?.Prihod.PostotakProsjeka.ToString(),
                ["{{PrihodPoClanu}}"] = zahtjev.KucanstvoPodaci?.Prihod.PrihodPoClanu.ToString("N2"),
                ["{{IspunjavaUvjetPrihoda}}"] = zahtjev.KucanstvoPodaci?.Prihod.IspunjavaUvjetPrihoda == true ? "Da" : "Ne",
                ["{{StambeniStatus}}"] = zahtjev.KucanstvoPodaci?.StambeniStatusKucanstva.GetDisplayName(),
                ["{{StambeniStatusBodova}}"] = BodoviOrZero(bodovi.BodoviStambeniStatus),
                ["{{SastavKucanstva}}"] = zahtjev.KucanstvoPodaci?.SastavKucanstva.GetDisplayName(),
                ["{{SastavKucanstvaBodova}}"] = BodoviOrZero(bodovi.BodoviSastavKucanstva),
                ["{{BrojClanova}}"] = zahtjev.Clanovi.Count.ToString(),
                ["{{BodoviClanova}}"] = BodoviOrZero(bodovi.BodoviPoClanu),
                ["{{BrojMaloljetnih}}"] = brojMaloljetnih.ToString(),
                ["{{BodoviMaloljetni}}"] = BodoviOrZero(bodovi.BodoviMaloljetni),
                ["{{BrojPunoljetniSkolovanje}}"] = zahtjev.BodovniPodaci?.BrojUzdrzavanePunoljetneDjece.ToString(),
                ["{{BodoviPunoljetniSkolovanje}}"] = BodoviOrZero(bodovi.BodoviPunoljetniUzdrzavani),
                ["{{ZMN}}"] = zahtjev.BodovniPodaci?.PrimateljZajamceneMinimalneNaknade == true ? "Da" : "Ne",
                ["{{BodoviZMN}}"] = BodoviOrZero(bodovi.BodoviZajamcenaNaknada),
                ["{{RoditeljNjegovatelj}}"] = zahtjev.BodovniPodaci?.StatusRoditeljaNjegovatelja == true ? "Da" : "Ne",
                ["{{BodoviRoditeljNjegovatelj}}"] = BodoviOrZero(bodovi.BodoviNjegovatelj),
                ["{{DoplatakNjega}}"] = zahtjev.BodovniPodaci?.KorisnikDoplatkaZaPomoc == true ? "Da" : "Ne",
                ["{{BodoviDoplatakNjega}}"] = BodoviOrZero(bodovi.BodoviDoplatakZaNjegu),
                ["{{OdrasliInvalidnina}}"] = zahtjev.BodovniPodaci?.BrojOdraslihKorisnikaInvalidnine.ToString(),
                ["{{BodoviOdrasliInvalidnina}}"] = BodoviOrZero(bodovi.BodoviOdraslihInvalidnina),
                ["{{MaloljetniInvalidnina}}"] = zahtjev.BodovniPodaci?.BrojMaloljetnihKorisnikaInvalidnine.ToString(),
                ["{{BodoviMaloljetniInvalidnina}}"] = BodoviOrZero(bodovi.BodoviMaloljetnihInvalidnina),
                ["{{ObiteljskoNasilje}}"] = zahtjev.BodovniPodaci?.ZrtvaObiteljskogNasilja == true ? "Da" : "Ne",
                ["{{BodoviObiteljskoNasilje}}"] = BodoviOrZero(bodovi.BodoviZrtvaNasilja),
                ["{{AlternativnaSkrb}}"] = zahtjev.BodovniPodaci?.BrojOsobaUAlternativnojSkrbi.ToString(),
                ["{{BodoviAlternativnaSkrb}}"] = BodoviOrZero(bodovi.BodoviAlternativnaSkrb),
                ["{{Iznad55}}"] = godinePodnositelj >= 55 ? "Da" : "Ne",
                ["{{BodoviIznad55}}"] = BodoviOrZero(bodovi.BodoviIznad55),
                ["{{MjeseciBranitelj}}"] = zahtjev.BodovniPodaci?.BrojMjeseciObranaSuvereniteta.ToString(),
                ["{{BodoviBranitelj}}"] = BodoviOrZero(bodovi.BodoviObrana),
                ["{{ZrtvaSeksualnogNasilja}}"] = zahtjev.BodovniPodaci?.BrojClanovaZrtavaSeksualnogNasilja.ToString(),
                ["{{BodoviZrtvaSeksualnogNasilja}}"] = BodoviOrZero(bodovi.BodoviSeksualnoNasilje),
                ["{{CivilniStradalnik}}"] = zahtjev.BodovniPodaci?.BrojCivilnihStradalnika.ToString(),
                ["{{BodoviCivilniStradalnik}}"] = BodoviOrZero(bodovi.BodoviCivilniStradalnici),
                ["{{Obradio}}"] = zahtjev.CreatedByUser?.UserName ?? "",
                ["{{DatumObrade}}"] = zahtjev.CreatedAt.ToShortDateString(),
                ["{{UkupnoBodova}}"] = UkupnoBodovaLabel()
            };

            foreach (var (placeholder, value) in data)
                ReplaceText(body, placeholder, value);

            wordDoc.MainDocumentPart?.Document?.Save();
        }

        var bytes = await File.ReadAllBytesAsync(tempPath);
        File.Delete(tempPath);
        return bytes;
    }

    private static void ReplaceText(Body body, string placeholder, string? value)
    {
        foreach (var text in body.Descendants<Text>().Where(t => t.Text.Contains(placeholder)))
        {
            text.Text = text.Text.Replace(placeholder, value ?? string.Empty);
        }
    }
}
