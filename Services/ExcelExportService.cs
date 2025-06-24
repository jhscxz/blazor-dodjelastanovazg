using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Models;
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
            .Include(z => z.KucanstvoPodaci).ThenInclude(k => k!.Prihod)
            .Include(z => z.BodovniPodaci)
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
                sheetData.Append(CreateDataRow(z));
            
            AddColumnWidths(wsPart.Worksheet, sheetData);
            AddTable(wsPart, sheetData, zahtjevi.Count + 1);

            wbPart.Workbook.Save();
        }

        return ms.ToArray();
    }
    
    private static readonly string[] Headers =
    [
        "Klasa predmeta",
        "Ime i prezime",
        "OIB",
        "Osnovanost",
        "Adresa",
        "Useljiva nekretnina ZG/ZG županija",
        "Datum rođenja podnositelja",
        "Datum početka prebivališta",
        "Ukupni godišnji prihod kućanstva",
        "Postotak prosjeka",
        "Prihod po članu kućanstva",
        "Ispunjava uvjet prihoda",
        "Stambeni status kućanstva",
        "Stambeni status kućanstva bodovi",
        "Sastav kućanstva",
        "Sastav kućanstva bodovi",
        "Broj članova kućanstva",
        "Broj članova kućanstva bodovi",
        "Broj maloljetne djece",
        "Maloljetna djeca bodovi",
        "Broj punoljetne djece na školovanju",
        "Punoljetna djeca na školovanju bodovi",
        "Primatelj ZMN",
        "ZMN bodovi",
        "Roditelj njegovatelj",
        "Roditelj njegovatelj bodovi",
        "Doplatak za njegu",
        "Doplatak za njegu bodovi",
        "Broj odraslih korisnika invalidnine",
        "Odrasli invalidnina bodovi",
        "Broj maloljetnih korisnika invalidnine",
        "Maloljetni invalidnina bodovi",
        "Žrtva obiteljskog nasilja",
        "Žrtva obiteljskog nasilja bodovi",
        "Broj osoba u alternativnoj skrbi",
        "Alternativna skrb bodovi",
        "Iznad 55 godina",
        "Iznad 55 godina bodovi",
        "Broj mjeseci obrane suvereniteta",
        "Branitelj bodovi",
        "Broj članova žrtava seksualnog nasilja",
        "Žrtva seksualnog nasilja bodovi",
        "Broj civilnih stradalnika",
        "Civilni stradalnik bodovi",
        "Obradio",
        "Datum obrade",
        "Ukupno bodova"
    ];

    private static Row CreateHeaderRow() => new(
        Headers.Select(CreateTextCell).ToList());

    private static Cell CreateTextCell(string text) =>
        new(new CellValue(text)) { DataType = CellValues.String };
    
        private static Row CreateDataRow(SocijalniNatjecajZahtjev zahtjev)
    {
        var podnositelj = zahtjev.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
        var datumPodnosenja = DateOnly.FromDateTime(zahtjev.DatumPodnosenjaZahtjeva);
        var brojMaloljetnih = zahtjev.Clanovi.Count(c => c.DatumRodjenja.AddYears(18) > datumPodnosenja);
        var godinePodnositelj = podnositelj != null
            ? datumPodnosenja.Year - podnositelj.DatumRodjenja.Year -
              (datumPodnosenja < podnositelj.DatumRodjenja.AddYears(datumPodnosenja.Year - podnositelj.DatumRodjenja.Year)
                  ? 1
                  : 0)
            : 0;

        string UkupnoBodovaLabel() => zahtjev.RezultatObrade switch
        {
            RezultatObrade.Neosnovan => "Neosnovan",
            RezultatObrade.Nepotpun => "Nepotpun",
            _ => zahtjev.Bodovi?.UkupnoBodova.ToString("0.##") ?? string.Empty
        };

        string F(float f, bool dec = false) => dec ? f.ToString("0.00") : f.ToString("0.##");

        var cells = new List<Cell>
        {
            CreateTextCell(zahtjev.KlasaPredmeta.ToString()),
            CreateTextCell(podnositelj?.ImePrezime ?? string.Empty),
            CreateTextCell(podnositelj?.Oib ?? string.Empty),
            CreateTextCell(zahtjev.RezultatObrade.ToString()),
            CreateTextCell(zahtjev.Adresa ?? string.Empty),
            CreateTextCell(zahtjev.PosjedujeNekretninuZG ? "Da" : "Ne"),
            CreateTextCell(podnositelj?.DatumRodjenja.ToString("dd.MM.yyyy") ?? string.Empty),
            CreateTextCell(zahtjev.KucanstvoPodaci?.PrebivanjeOd?.ToString("dd.MM.yyyy") ?? string.Empty),
            CreateTextCell(zahtjev.KucanstvoPodaci?.Prihod?.UkupniPrihodKucanstva.ToString("N2") ?? string.Empty),
            CreateTextCell(zahtjev.KucanstvoPodaci?.Prihod?.PostotakProsjeka?.ToString("N2") ?? string.Empty),
            CreateTextCell(zahtjev.KucanstvoPodaci?.Prihod?.PrihodPoClanu.ToString("N2") ?? string.Empty),
            CreateTextCell(zahtjev.KucanstvoPodaci?.Prihod?.IspunjavaUvjetPrihoda == true ? "Da" : "Ne"),
            CreateTextCell(zahtjev.KucanstvoPodaci?.StambeniStatusKucanstva.GetDisplayName()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviStambeniStatus) : string.Empty),
            CreateTextCell(zahtjev.KucanstvoPodaci?.SastavKucanstva.GetDisplayName()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviSastavKucanstva) : string.Empty),
            CreateTextCell(zahtjev.Clanovi.Count.ToString()),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviPoClanu) : string.Empty),
            CreateTextCell(brojMaloljetnih.ToString()),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviMaloljetni) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.BrojUzdrzavanePunoljetneDjece.ToString()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviPunoljetniUzdrzavani) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.PrimateljZajamceneMinimalneNaknade == true ? "Da" : "Ne"),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviZajamcenaNaknada) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.StatusRoditeljaNjegovatelja == true ? "Da" : "Ne"),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviNjegovatelj) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.KorisnikDoplatkaZaPomoc == true ? "Da" : "Ne"),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviDoplatakZaNjegu) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.BrojOdraslihKorisnikaInvalidnine.ToString()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviOdraslihInvalidnina) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.BrojMaloljetnihKorisnikaInvalidnine.ToString()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviMaloljetnihInvalidnina) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.ZrtvaObiteljskogNasilja == true ? "Da" : "Ne"),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviZrtvaNasilja) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.BrojOsobaUAlternativnojSkrbi.ToString()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviAlternativnaSkrb) : string.Empty),
            CreateTextCell(godinePodnositelj >= 55 ? "Da" : "Ne"),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviIznad55) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.BrojMjeseciObranaSuvereniteta.ToString()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviObrana, true) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.BrojClanovaZrtavaSeksualnogNasilja.ToString()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviSeksualnoNasilje) : string.Empty),
            CreateTextCell(zahtjev.BodovniPodaci?.BrojCivilnihStradalnika.ToString()!),
            CreateTextCell(zahtjev.Bodovi != null ? F(zahtjev.Bodovi.BodoviCivilniStradalnici) : string.Empty),
            CreateTextCell(zahtjev.CreatedByUser?.UserName ?? string.Empty),
            CreateTextCell(zahtjev.CreatedAt.ToShortDateString()),
            CreateTextCell(UkupnoBodovaLabel())
        };

        return new Row(cells);
    }
        
            private static void AddColumnWidths(Worksheet worksheet, SheetData data)
    {
        var widths = new int[Headers.Length];
        foreach (var row in data.Elements<Row>())
        {
            int i = 0;
            foreach (var cell in row.Elements<Cell>())
            {
                var len = cell.CellValue?.Text?.Length ?? 0;
                if (len > widths[i])
                    widths[i] = len;
                i++;
            }
        }

        var columns = new Columns();
        for (var i = 0; i < widths.Length; i++)
        {
            double w = widths[i] + 2;
            columns.Append(new Column
            {
                Min = (uint)(i + 1),
                Max = (uint)(i + 1),
                Width = w,
                CustomWidth = true,
                BestFit = true
            });
        }

        worksheet.InsertAt(columns, 0);
    }

    private static void AddTable(WorksheetPart wsPart, SheetData data, int rowCount)
    {
        var range = $"A1:{GetColumnLetter(Headers.Length)}{rowCount}";

        var tablePart = wsPart.AddNewPart<TableDefinitionPart>();
        var table = new Table
        {
            Id = 1U,
            Name = "ZahtjeviTable",
            DisplayName = "ZahtjeviTable",
            Reference = range,
            TotalsRowShown = false
        };

        table.Append(new AutoFilter { Reference = range });

        var cols = new TableColumns { Count = (uint)Headers.Length };
        for (uint i = 0; i < Headers.Length; i++)
            cols.Append(new TableColumn { Id = i + 1, Name = Headers[i] });
        table.Append(cols);

        table.Append(new TableStyleInfo
        {
            Name = "TableStyleMedium2",
            ShowFirstColumn = false,
            ShowLastColumn = false,
            ShowRowStripes = true,
            ShowColumnStripes = false
        });

        tablePart.Table = table;

        var tableParts = new TableParts { Count = 1U };
        tableParts.Append(new TablePart { Id = wsPart.GetIdOfPart(tablePart) });
        wsPart.Worksheet.Append(tableParts);
    }

    private static string GetColumnLetter(int index)
    {
        var dividend = index;
        var columnName = string.Empty;
        while (dividend > 0)
        {
            var modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo) + columnName;
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }
}