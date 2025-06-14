using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DodjelaStanovaZG.Enums;
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

        // Obrada dokumenta
        using (var wordDoc = WordprocessingDocument.Open(tempPath, true))
        {
            var body = wordDoc.MainDocumentPart?.Document?.Body;

            if (body is null)
                throw new InvalidOperationException("Ne mogu učitati sadržaj dokumenta.");

            var podnositelj = zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);

            ReplaceText(body, "{{ImePrezime}}", podnositelj?.ImePrezime);
            ReplaceText(body, "{{Adresa}}", zahtjev.Adresa);
            ReplaceText(body, "{{DatumRodenja}}", podnositelj?.DatumRodjenja.ToString("dd.MM.yyyy"));
            ReplaceText(body, "{{Oib}}", podnositelj?.Oib);
            ReplaceText(body, "{{UkupnoBodova}}", bodovi.UkupnoBodova.ToString());

            // Dodaj još ReplaceText(...) po potrebi

            wordDoc.MainDocumentPart?.Document?.Save();
        }

        // Tek nakon zatvaranja dokumenta čitamo bajtove
        var bytes = await File.ReadAllBytesAsync(tempPath);
        File.Delete(tempPath);
        return bytes;
    }

    private void ReplaceText(Body body, string placeholder, string? value)
    {
        foreach (var text in body.Descendants<Text>().Where(t => t.Text.Contains(placeholder)))
        {
            text.Text = text.Text.Replace(placeholder, value ?? string.Empty);
        }
    }
}
