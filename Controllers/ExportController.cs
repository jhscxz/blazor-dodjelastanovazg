using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DodjelaStanovaZG.Controllers;

[ApiController]
[Route("api/export")]
public class ExportController(
    IWordExportService wordExportService,
    IUnitOfWork unitOfWork)
    : ControllerBase
{
    [HttpGet("zapisnik/{id:long}")]
    public async Task<IActionResult> GetZapisnik(long id)
    {
        var zahtjev = await unitOfWork.SocijalniZahtjevRead.GetZahtjevByIdAsync(id);
        Console.WriteLine($"Zahtjev za ID {id} => POSTOJI, bodovi: {(zahtjev?.Bodovi == null ? "NULL" : "OK")}");

        if (zahtjev?.Bodovi == null)
            return NotFound();

        var bytes = await wordExportService.GenerirajIzvjestajAsync(zahtjev, zahtjev.Bodovi);
        var fileName = $"Zapisnik_{id}.docx";

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            fileName);
    }
}