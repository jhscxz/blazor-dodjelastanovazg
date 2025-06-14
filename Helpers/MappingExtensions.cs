using DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Helpers;

public static class MappingExtensions
{
    public static SocijalniNatjecajZahtjevDto ToDto(this SocijalniNatjecajZahtjev x)
    {
        var pod = x.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
        return new SocijalniNatjecajZahtjevDto
        {
            Id = x.Id,
            NatjecajId = x.NatjecajId,
            KlasaPredmeta = x.KlasaPredmeta,
            DatumPodnosenjaZahtjeva = x.DatumPodnosenjaZahtjeva,
            Adresa = x.Adresa,
            Email = x.Email,
            ImePrezime = pod?.ImePrezime ?? string.Empty,
            Oib = pod?.Oib,
            RezultatObrade = x.RezultatObrade,
            NapomenaObrade = x.NapomenaObrade,
            Bodovni = new SocijalniBodovniDto(),
            KucanstvoPodaci = x.KucanstvoPodaci?.ToDto(),
            Clanovi = x.Clanovi.Select(c => c.ToDto()).ToList()
        }.WithAuditFrom(x);
    }

    public static SocijalniNatjecajClanDto ToDto(this SocijalniNatjecajClan c)
        => new SocijalniNatjecajClanDto
        {
            Id = c.Id,
            ZahtjevId = c.ZahtjevId,
            ImePrezime = c.ImePrezime,
            Oib = c.Oib,
            Srodstvo = c.Srodstvo,
            DatumRodjenja = c.DatumRodjenja
        }.WithAuditFrom(c);

    public static SocijalniKucanstvoPodaciDto ToDto(this SocijalniNatjecajKucanstvoPodaci k)
        => new SocijalniKucanstvoPodaciDto
        {
            UkupniPrihodKucanstva = k.UkupniPrihodKucanstva,
            PrebivanjeOd = k.PrebivanjeOd,
            StambeniStatusKucanstva = k.StambeniStatusKucanstva,
            SastavKucanstva = k.SastavKucanstva,
            ZahtjevId = k.ZahtjevId
        }.WithAuditFrom(k);

    public static SocijalniNatjecajBodovniPodaciDto ToDto(this SocijalniNatjecajBodovniPodaci b)
        => new SocijalniNatjecajBodovniPodaciDto
        {
            ZahtjevId = b.ZahtjevId,
            BrojUzdrzavanePunoljetneDjece = b.BrojUzdrzavanePunoljetneDjece,
            PrimateljZajamceneMinimalneNaknade = b.PrimateljZajamceneMinimalneNaknade,
            StatusRoditeljaNjegovatelja = b.StatusRoditeljaNjegovatelja,
            KorisnikDoplatkaZaPomoc = b.KorisnikDoplatkaZaPomoc,
            BrojOdraslihKorisnikaInvalidnine = b.BrojOdraslihKorisnikaInvalidnine,
            BrojMaloljetnihKorisnikaInvalidnine = b.BrojMaloljetnihKorisnikaInvalidnine,
            ZrtvaObiteljskogNasilja = b.ZrtvaObiteljskogNasilja,
            BrojOsobaUAlternativnojSkrbi = b.BrojOsobaUAlternativnojSkrbi,
            BrojMjeseciObranaSuvereniteta = b.BrojMjeseciObranaSuvereniteta,
            BrojClanovaZrtavaSeksualnogNasilja = b.BrojClanovaZrtavaSeksualnogNasilja,
            BrojCivilnihStradalnika = b.BrojCivilnihStradalnika
        }.WithAuditFrom(b);

    public static void MapOnto(this SocijalniNatjecajBodovniPodaciDto dto, SocijalniNatjecajBodovniPodaci entity)
    {
        entity.BrojUzdrzavanePunoljetneDjece = dto.BrojUzdrzavanePunoljetneDjece;
        entity.PrimateljZajamceneMinimalneNaknade = dto.PrimateljZajamceneMinimalneNaknade;
        entity.StatusRoditeljaNjegovatelja = dto.StatusRoditeljaNjegovatelja;
        entity.KorisnikDoplatkaZaPomoc = dto.KorisnikDoplatkaZaPomoc;
        entity.BrojOdraslihKorisnikaInvalidnine = dto.BrojOdraslihKorisnikaInvalidnine;
        entity.BrojMaloljetnihKorisnikaInvalidnine = dto.BrojMaloljetnihKorisnikaInvalidnine;
        entity.ZrtvaObiteljskogNasilja = dto.ZrtvaObiteljskogNasilja;
        entity.BrojOsobaUAlternativnojSkrbi = dto.BrojOsobaUAlternativnojSkrbi;
        entity.BrojMjeseciObranaSuvereniteta = dto.BrojMjeseciObranaSuvereniteta;
        entity.BrojClanovaZrtavaSeksualnogNasilja = dto.BrojClanovaZrtavaSeksualnogNasilja;
        entity.BrojCivilnihStradalnika = dto.BrojCivilnihStradalnika;
    }

    public static void MapOnto(this SocijalniNatjecajClanDto dto, SocijalniNatjecajClan entity)
    {
        entity.ImePrezime = dto.ImePrezime;
        entity.Oib = dto.Oib;
        entity.DatumRodjenja = dto.DatumRodjenja;
        entity.Srodstvo = dto.Srodstvo;
    }

    public static void MapOnto(this SocijalniNatjecajOsnovnoEditDto dto, SocijalniNatjecajZahtjev entity)
    {
        entity.KlasaPredmeta = dto.KlasaPredmeta ?? 0;
        entity.DatumPodnosenjaZahtjeva = dto.DatumPodnosenjaZahtjeva ?? default;
        entity.Adresa = dto.Adresa;
        entity.RezultatObrade = dto.RezultatObrade ?? 0;
        entity.NapomenaObrade = dto.NapomenaObrade;
        entity.Email = dto.Email;
    }
    
    public static SocijalniNatjecajClan ToEntity(this SocijalniNatjecajClanDto dto, long zahtjevId)
        => new()
        {
            ZahtjevId = zahtjevId,
            ImePrezime = dto.ImePrezime,
            Oib = dto.Oib,
            DatumRodjenja = dto.DatumRodjenja,
            Srodstvo = dto.Srodstvo,
            Zahtjev = null!
        };
}