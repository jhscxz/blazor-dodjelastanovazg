
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class ExcelExportServiceTests
{
    [Fact]
    public async Task ExportNatjecajAsync_WritesRows()
    {
        var dbName = Guid.NewGuid().ToString();
        await using (var context = TestDb.CreateContext(dbName))
        {
            context.SocijalniNatjecajZahtjevi.Add(new SocijalniNatjecajZahtjev
            {
                Id = 1,
                KlasaPredmeta = 1,
                NatjecajId = 5,
                RezultatObrade = RezultatObrade.Osnovan,
                DatumPodnosenjaZahtjeva = DateTime.Today,
                Clanovi = [new()
                    {
                        ImePrezime = "Test",
                        Oib = "123",
                        Srodstvo = Srodstvo.PodnositeljZahtjeva,
                        DatumRodjenja = new DateOnly(2000,1,1),
                        Zahtjev = null
                    }
                ],
                Bodovi = new SocijalniNatjecajBodovi { UkupnoBodova = 10, BodoviStambeniStatus = 5 }
            });
            await context.SaveChangesAsync();
        }

        var factory = TestDb.CreateFactory(dbName);
        var service = new ExcelExportService(factory.Object);

        var bytes = await service.ExportNatjecajAsync(5, null);

        using var ms = new MemoryStream(bytes);
        using var doc = SpreadsheetDocument.Open(ms, false);
        var rows = doc.WorkbookPart!.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>()!.Elements<Row>().ToList();

        Assert.Equal(2, rows.Count); // header + data
        var headerCells = rows[0].Elements<Cell>().ToList();
        Assert.Equal("Klasa predmeta", headerCells[0].CellValue!.Text);
        Assert.Equal("Datum rođenja podnositelja", headerCells[5].CellValue!.Text);
        Assert.Equal("Stambeni status kućanstva bodovi", headerCells[12].CellValue!.Text);

        var dataCells = rows[1].Elements<Cell>().ToList();
        Assert.Equal("1", dataCells[0].CellValue!.Text);
        Assert.Equal("5", dataCells[12].CellValue!.Text);
    }

    [Fact]
    public async Task ExportNatjecajAsync_FiltersByRezultatObrade()
    {
        var dbName = Guid.NewGuid().ToString();
        await using (var context = TestDb.CreateContext(dbName))
        {
            context.SocijalniNatjecajZahtjevi.AddRange(
                new SocijalniNatjecajZahtjev
                {
                    Id = 1, KlasaPredmeta = 1, NatjecajId = 5, RezultatObrade = RezultatObrade.Osnovan,
                    Clanovi = [new()
                        {
                            ImePrezime = "A",
                            Oib = "1",
                            Srodstvo = Srodstvo.PodnositeljZahtjeva,
                            Zahtjev = null
                        }
                    ],
                    Bodovi = new SocijalniNatjecajBodovi { UkupnoBodova = 10 }
                },
                new SocijalniNatjecajZahtjev
                {
                    Id = 2, KlasaPredmeta = 2, NatjecajId = 5, RezultatObrade = RezultatObrade.Neosnovan,
                    Clanovi = [new()
                        {
                            ImePrezime = "B",
                            Oib = "2",
                            Srodstvo = Srodstvo.PodnositeljZahtjeva,
                            Zahtjev = null
                        }
                    ],
                    Bodovi = new SocijalniNatjecajBodovi { UkupnoBodova = 5 }
                },
                new SocijalniNatjecajZahtjev
                {
                    Id = 3, KlasaPredmeta = 3, NatjecajId = 5, RezultatObrade = RezultatObrade.Osnovan,
                    Clanovi = [new()
                        {
                            ImePrezime = "C",
                            Oib = "3",
                            Srodstvo = Srodstvo.PodnositeljZahtjeva,
                            Zahtjev = null
                        }
                    ],
                    Bodovi = new SocijalniNatjecajBodovi { UkupnoBodova = 8 }
                });

            await context.SaveChangesAsync();
        }

        var factory = TestDb.CreateFactory(dbName);
        var service = new ExcelExportService(factory.Object);

        var bytes = await service.ExportNatjecajAsync(5, RezultatObrade.Osnovan);

        using var ms = new MemoryStream(bytes);
        using var doc = SpreadsheetDocument.Open(ms, false);
        var rows = doc.WorkbookPart!.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>()!.Elements<Row>().ToList();

        Assert.Equal(3, rows.Count); // header + two data rows
        Assert.All(rows.Skip(1), r => Assert.Equal("Osnovan", r.Elements<Cell>().ElementAt(3).CellValue!.Text));
    }
}