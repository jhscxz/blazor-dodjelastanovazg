using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;
using Mapster;

namespace DodjelaStanovaZG.Helpers;

public static class MappingExtensions
{
    static MappingExtensions()
    {
        TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

        #region Natjecaj <-> NatjecajDto

        TypeAdapterConfig<Natjecaj, NatjecajDto>.NewConfig()
            .Map(dest => dest.Vrsta, src => src.PriustiviIliSocijalni == 2 ? "Socijalni" : "Priuštivi")
            .Map(dest => dest.Status, src => src.Zakljucen == 2 ? "Zaključen" : "Aktivan");

        TypeAdapterConfig<NatjecajDto, Natjecaj>.NewConfig()
            .Map(dest => dest.PriustiviIliSocijalni, src => src.Vrsta == "Socijalni" ? (byte)2 : (byte)1)
            .Map(dest => dest.Zakljucen, src => src.Status == "Zaključen" ? (byte)2 : (byte)1);

        #endregion

        #region Entity → DTO konfiguracije

        TypeAdapterConfig<SocijalniNatjecajZahtjev, SocijalniNatjecajZahtjevDto>
            .NewConfig()
            .Map(dest => dest.ImePrezime, src => src.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)!.ImePrezime)
            .Map(dest => dest.Oib, src => src.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)!.Oib)
            .Map(dest => dest.RowVersion, src => src.RowVersion)
            .Map(dest => dest.Prihod, src => src.KucanstvoPodaci!.Prihod!)
            .Map(dest => dest.PodnositeljIznad55, src => IzracunajPodnositeljIznad55(src))
            .Map(dest => dest.BrojMaloljetneDjece, src => IzracunajBrojMaloljetnih(src))
            .Map(dest => dest.PosjedujeNekretninuZg, src => src.PosjedujeNekretninuZg);

        TypeAdapterConfig<SocijalniNatjecajClan, SocijalniNatjecajClanDto>.NewConfig();
        TypeAdapterConfig<SocijalniNatjecajBodovniPodaci, SocijalniNatjecajBodovniPodaciDto>.NewConfig();
        TypeAdapterConfig<SocijalniNatjecajKucanstvoPodaci, SocijalniKucanstvoPodaciDto>.NewConfig();
        TypeAdapterConfig<SocijalniPrihodi, SocijalniPrihodDto>.NewConfig();

        #endregion

    }

    #region Entity → DTO metode

    public static SocijalniNatjecajZahtjevDto ToDto(this SocijalniNatjecajZahtjev entity) => entity.Adapt<SocijalniNatjecajZahtjevDto>();
    public static SocijalniNatjecajClanDto ToDto(this SocijalniNatjecajClan entity) => entity.Adapt<SocijalniNatjecajClanDto>();
    public static SocijalniKucanstvoPodaciDto ToDto(this SocijalniNatjecajKucanstvoPodaci entity) => entity.Adapt<SocijalniKucanstvoPodaciDto>();
    public static SocijalniNatjecajBodovniPodaciDto ToDto(this SocijalniNatjecajBodovniPodaci entity) => entity.Adapt<SocijalniNatjecajBodovniPodaciDto>();
    public static NatjecajDto ToDto(this Natjecaj entity) => entity.Adapt<NatjecajDto>();

    #endregion

    #region DTO → Entity metode

    public static SocijalniNatjecajClan ToEntity(this SocijalniNatjecajClanDto dto, long zahtjevId)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var entity = dto.Adapt<SocijalniNatjecajClan>();
        entity.ZahtjevId = zahtjevId;
        return entity;
    }

    #endregion

    #region MapOnto (Patch/Update) metode

    public static void MapOnto(this SocijalniNatjecajBodovniPodaciDto dto, SocijalniNatjecajBodovniPodaci entity) => dto.Adapt(entity);

    public static void MapOnto(this SocijalniNatjecajClanDto dto, SocijalniNatjecajClan entity) => dto.Adapt(entity);

    public static void MapOnto(this SocijalniNatjecajOsnovnoEditDto dto, SocijalniNatjecajZahtjev entity)
    {
        dto.Adapt(entity);
        entity.PosjedujeNekretninuZg = dto.PosjedujeNekretninuZg;
        if (dto.RowVersion is not null) entity.RowVersion = dto.RowVersion;
        if (dto.RezultatObrade.HasValue) entity.ManualniRezultatObrade = dto.RezultatObrade.Value;
    }

    #endregion

    #region helperi

    private static bool IzracunajPodnositeljIznad55(SocijalniNatjecajZahtjev src)
    {
        var podnositelj = src.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva);
        if (podnositelj is null) return false;

        var datum = DateOnly.FromDateTime(src.DatumPodnosenjaZahtjeva);
        var age = datum.Year - podnositelj.DatumRodjenja.Year - (datum < podnositelj.DatumRodjenja.AddYears(datum.Year - podnositelj.DatumRodjenja.Year) ? 1 : 0);

        return age >= 55;
    }

    private static byte IzracunajBrojMaloljetnih(SocijalniNatjecajZahtjev src)
    {
        var datum = DateOnly.FromDateTime(src.DatumPodnosenjaZahtjeva);
        return (byte)(src.Clanovi.Count(c => c.DatumRodjenja.AddYears(18) > datum));
    }

    #endregion
}
