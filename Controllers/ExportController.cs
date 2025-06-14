using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DodjelaStanovaZG.Controllers;

[ApiController]
[Route("api/export")]
public class ExportController(IWordExportService wordExportService, ISocijalniZahtjevService zahtjevService)
    : ControllerBase
{
    [HttpGet("zapisnik/{id:long}")]
    public async Task<IActionResult> GetZapisnik(long id)
    {
        
        var zahtjev = await zahtjevService.GetZahtjevByIdAsync(id);
        Console.WriteLine($"Zahtjev za ID {id} => {("POSTOJI")}, bodovi: {(zahtjev?.Bodovi == null ? "NULL" : "OK")}");

        if (zahtjev?.Bodovi == null)
            return NotFound();

        var bytes = await wordExportService.GenerirajIzvjestajAsync(zahtjev, zahtjev.Bodovi);
        var fileName = $"Zapisnik_{id}.docx";

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            fileName);
    }
}