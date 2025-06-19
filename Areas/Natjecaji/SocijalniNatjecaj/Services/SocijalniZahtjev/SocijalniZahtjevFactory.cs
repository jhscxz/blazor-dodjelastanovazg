using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;

public class SocijalniZahtjevFactory : ISocijalniZahtjevFactory
{
    public SocijalniNatjecajZahtjev KreirajNovi(SocijalniNatjecajZahtjevDto dto, string? imePrezime, string? oib)
    {
        var zahtjev = new SocijalniNatjecajZahtjev
        {
            NatjecajId = dto.NatjecajId,
            KlasaPredmeta = dto.KlasaPredmeta!.Value,
            DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? DateTime.UtcNow,
            Adresa = dto.Adresa,
            Email = dto.Email,
            RezultatObrade = dto.RezultatObrade!.Value,
            ManualniRezultatObrade = dto.RezultatObrade!.Value,
            NapomenaObrade = dto.NapomenaObrade
        };

        zahtjev.Clanovi = [
            new SocijalniNatjecajClan
            {
                ImePrezime = imePrezime!,
                Oib = string.IsNullOrWhiteSpace(oib) ? null : oib,
                Srodstvo = Srodstvo.PodnositeljZahtjeva,
                Zahtjev = zahtjev
            }
        ];

        zahtjev.KucanstvoPodaci = new() { Zahtjev = zahtjev };
        zahtjev.BodovniPodaci = new() { Zahtjev = zahtjev };
        zahtjev.Bodovi = new() { Zahtjev = zahtjev };

        return zahtjev;
    }

    public SocijalniPrihodi KreirajPrazanPrihod(long kucanstvoId)
        => new()
        {
            Id = kucanstvoId,
            UkupniPrihodKucanstva = 0,
            PrihodPoClanu = 0,
            IspunjavaUvjetPrihoda = true
        };
}