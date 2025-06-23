using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using DodjelaStanovaZG.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DodjelaStanovaZG.Tests.Services;

public class ExcelExportServiceTests
{
    private static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    private static Mock<IDbContextFactory<ApplicationDbContext>> CreateFactory(string dbName)
    {
        var factory = new Mock<IDbContextFactory<ApplicationDbContext>>();
        factory.Setup(f => f.CreateDbContext()).Returns(() => CreateContext(dbName));
        return factory;
    }

    [Fact]
    public async Task ExportNatjecajAsync_WritesRows()
    {
        var dbName = Guid.NewGuid().ToString();
        await using (var context = CreateContext(dbName))
        {
            var zahtjev = new SocijalniNatjecajZahtjev
            {
                Id = 1,
                KlasaPredmeta = 1,
                NatjecajId = 5,
                RezultatObrade = RezultatObrade.Osnovan,
                Clanovi =
                [new SocijalniNatjecajClan
                    {
                        ImePrezime = "Test",
                        Oib = "123",
                        Srodstvo = Srodstvo.PodnositeljZahtjeva,
                        Zahtjev = null
                    }
                ],
                Bodovi = new SocijalniNatjecajBodovi { UkupnoBodova = 10 }
            };
            context.SocijalniNatjecajZahtjevi.Add(zahtjev);
            await context.SaveChangesAsync();
        }

        var factory = CreateFactory(dbName);
        var logger = new Mock<ILogger<ExcelExportService>>();
        var service = new ExcelExportService(factory.Object, logger.Object);

        var bytes = await service.ExportNatjecajAsync(5, null);

        using var ms = new MemoryStream(bytes);
        using var doc = SpreadsheetDocument.Open(ms, false);
        var sheet = doc.WorkbookPart!.WorksheetParts.First().Worksheet;
        var rows = sheet.GetFirstChild<SheetData>()!.Elements<Row>().ToList();

        Assert.Equal(2, rows.Count); // header + data
        Assert.Equal("Klasa predmeta", rows[0].Elements<Cell>().First().CellValue!.Text);
        Assert.Equal("1", rows[1].Elements<Cell>().First().CellValue!.Text);
    }
}