using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DodjelaStanovaZG.Controllers;

[ApiController]
[Route("api/export")]
[Authorize]
public class ExportController(
    ILogger<ExportController> logger,
    IWordExportService wordExportService,
    IExcelExportService excelExportService,
    IUnitOfWork unitOfWork)
    : ControllerBase
{
    [HttpGet("zapisnik/{id:long}")]
    public async Task<IActionResult> GetZapisnik(long id)
    {
        var zahtjev = await unitOfWork.SocijalniZahtjevRead.GetZahtjevByIdAsync(id);
        logger.LogInformation("Zahtjev za ID {Id} => POSTOJI, bodovi: {Bodovi}", id,
            zahtjev?.Bodovi == null ? "NULL" : "OK");

        if (zahtjev?.Bodovi == null)
            return NotFound();

        var bytes = await wordExportService.GenerirajIzvjestajAsync(zahtjev, zahtjev.Bodovi);
        var fileName = $"Zapisnik_{id}.docx";

        return File(bytes,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            fileName);
    }
    
    [HttpGet("socijalni/{natjecajId:long}/excel")]
    public async Task<IActionResult> ExportSocijalniExcel(long natjecajId, [FromQuery] RezultatObrade? filter)
    {
        var bytes = await excelExportService.ExportNatjecajAsync(natjecajId, filter);
        var fileName = $"Socijalni_{natjecajId}.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}