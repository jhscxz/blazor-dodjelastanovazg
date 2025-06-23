using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Services;

public class ExcelExportService(
    IDbContextFactory<ApplicationDbContext> contextFactory) : IExcelExportService
{
    public async Task<byte[]> ExportNatjecajAsync(long natjecajId, RezultatObrade? filter)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var query = context.SocijalniNatjecajZahtjevi
            .Include(z => z.Clanovi)
            .Include(z => z.Bodovi)
            .Where(z => z.NatjecajId == natjecajId);

        if (filter.HasValue)
            query = query.Where(z => z.RezultatObrade == filter);

        var zahtjevi = await query.AsNoTracking().ToListAsync();

        using var ms = new MemoryStream();
        using (var doc = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook, true))
        {
            var wbPart = doc.AddWorkbookPart();
            wbPart.Workbook = new Workbook();
            var wsPart = wbPart.AddNewPart<WorksheetPart>();

            var sheetData = new SheetData();
            wsPart.Worksheet = new Worksheet(sheetData);

            var sheets = wbPart.Workbook.AppendChild(new Sheets());
            sheets.Append(new Sheet
            {
                Id = wbPart.GetIdOfPart(wsPart),
                SheetId = 1,
                Name = "Zahtjevi"
            });

            sheetData.Append(CreateHeaderRow());

            foreach (var z in zahtjevi)
            {
                var podnositelj = z.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
                sheetData.Append(new Row(new List<Cell>
                {
                    CreateTextCell(z.KlasaPredmeta.ToString()),
                    CreateTextCell(podnositelj?.ImePrezime ?? string.Empty),
                    CreateTextCell(podnositelj?.Oib ?? string.Empty),
                    CreateTextCell(z.Bodovi?.UkupnoBodova.ToString("0.##") ?? string.Empty),
                    CreateTextCell(z.RezultatObrade.ToString())
                }));
            }

            wbPart.Workbook.Save();
        }

        return ms.ToArray();
    }

    private static Row CreateHeaderRow() => new(
        new List<Cell>
        {
            CreateTextCell("Klasa predmeta"),
            CreateTextCell("Ime i prezime"),
            CreateTextCell("OIB"),
            CreateTextCell("Ukupno bodova"),
            CreateTextCell("Osnovanost")
        });

    private static Cell CreateTextCell(string text) =>
        new(new CellValue(text)) { DataType = CellValues.String };
}