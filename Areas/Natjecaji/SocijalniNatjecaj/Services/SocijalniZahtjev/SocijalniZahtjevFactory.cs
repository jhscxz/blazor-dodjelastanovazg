using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevFactory(ILogger<SocijalniZahtjevFactory> logger) : ISocijalniZahtjevFactory
{
    public SocijalniNatjecajZahtjev KreirajNovi(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        logger.LogInformation("Kreiranje novog zahtjeva za natječaj {NatjecajId}", dto.NatjecajId);

        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = dto.Adresa,
            Email = dto.Email,
            PosjedujeNekretninuZg = dto.PosjedujeNekretninuZg,
            RezultatObrade = dto.RezultatObrade!.Value,
            ManualniRezultatObrade = dto.RezultatObrade!.Value,
            NapomenaObrade = dto.NapomenaObrade
        };

        zahtjev.Clanovi =
        [
            new SocijalniNatjecajClan
            {
                ImePrezime = imePrezime!,
                Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
                Srodstvo = Srodstvo.PodnositeljZahtjeva,
                Zahtjev = zahtjev
            }
        ];

        zahtjev.KucanstvoPodaci = new SocijalniNatjecajKucanstvoPodaci { Zahtjev = zahtjev };
        zahtjev.BodovniPodaci = new SocijalniNatjecajBodovniPodaci { Zahtjev = zahtjev };
        zahtjev.Bodovi = new SocijalniNatjecajBodovi { Zahtjev = zahtjev };

        return zahtjev;
    }
}