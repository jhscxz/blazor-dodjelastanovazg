using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;

public class SocijalniZahtjevFormHandler(IUnitOfWork unitOfWork, ISocijalniBodoviService bodoviService, NavigationManager navigation, ILogger<SocijalniZahtjevFormHandler> logger) : ISocijalniZahtjevFormHandler
{
    public async Task<(bool Success, List<string> Errors)> SubmitAsync(SocijalniNatjecajZahtjevDto model, int? rezultatObrade)
    {
        var errors = new List<string>();

        if (rezultatObrade is null)
        {
            errors.Add("Rezultat obrade je obavezan.");
            return (false, errors);
        }
        
        var natjecaj = await unitOfWork.NatjecajiService.GetByIdAsync(model.NatjecajId);
        if (natjecaj is null)
        {
            errors.Add($"Natječaj {model.NatjecajId} nije pronađen.");
            return (false, errors);
        }

        model.RezultatObrade = (RezultatObrade)rezultatObrade;

        try
        {
            logger.LogInformation("Kreiranje zahtjeva za natječaj {NatjecajId}", model.NatjecajId);

            var zahtjev = await unitOfWork.SocijalniZahtjevProcessorService.KreirajZahtjevAsync(model, model.ImePrezime, model.Oib);

            await bodoviService.IzracunajIBodujAsync(zahtjev.Id);

            navigation.NavigateTo($"/socijalni/detalji/{zahtjev.Id}");
            logger.LogInformation("Zahtjev {ZahtjevId} uspješno kreiran", zahtjev.Id);

            return (true, []);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Greška prilikom spremanja zahtjeva");
            errors.Add($"Greška prilikom spremanja: {ex.Message}");
            return (false, errors);
        }
    }
}