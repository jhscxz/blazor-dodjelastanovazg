using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;

public class SocijalniZahtjevFormHandler(
    IUnitOfWork unitOfWork,
    ISocijalniBodoviService bodoviService,
    NavigationManager navigation) : ISocijalniZahtjevFormHandler
{
    public async Task<(bool Success, List<string> Errors)> SubmitAsync(SocijalniNatjecajZahtjevDto model, int? rezultatObrade)
    {
        var errors = new List<string>();

        if (rezultatObrade is null)
        {
            errors.Add("Rezultat obrade je obavezan.");
            return (false, errors);
        }

        model.RezultatObrade = (RezultatObrade)rezultatObrade;

        try
        {
            var zahtjev =
                await unitOfWork.SocijalniZahtjevProcessor.KreirajZahtjevAsync(model, model.ImePrezime, model.Oib);

            await unitOfWork.SaveChangesAsync();
            await bodoviService.IzracunajIBodujAsync(zahtjev.Id);

            navigation.NavigateTo($"/socijalni/detalji/{zahtjev.Id}");
            return (true, []);
        }
        catch (Exception ex)
        {
            errors.Add($"Greška prilikom spremanja: {ex.Message}");
            return (false, errors);
        }
    }
}