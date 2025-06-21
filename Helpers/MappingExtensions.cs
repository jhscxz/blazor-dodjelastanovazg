#nullable enable
using Mapster;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Helpers;

/// <summary>
/// Centralno mjesto za mapiranje između entiteta i DTO‑a za Socijalni natječaj.
/// Korištenje Mapster Source Generator za performansno efikasno mapiranje bez refleksije.
/// </summary>
public static partial class MappingExtensions
{
    static MappingExtensions()
    {
        // Globalna postavka: ignoriramo null vrijednosti kod DTO → entity mapa
        TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

        // ---------- Entity → DTO konfiguracije ----------

        TypeAdapterConfig<SocijalniNatjecajZahtjev, SocijalniNatjecajZahtjevDto>
            .NewConfig()
            .Map(dest => dest.ImePrezime,
                 src => src.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)!.ImePrezime)
            .Map(dest => dest.Oib,
                 src => src.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)!.Oib);

        TypeAdapterConfig<SocijalniPrihodi, SocijalniPrihodDto>
            .NewConfig();

        // ---------- DTO → Entity konfiguracije (po potrebi dodavati ručno) ----------
    }

    // ---------- Entity → DTO metode ----------

    public static SocijalniNatjecajZahtjevDto ToDto(this SocijalniNatjecajZahtjev entity)
        => entity.Adapt<SocijalniNatjecajZahtjevDto>();

    public static SocijalniNatjecajClanDto ToDto(this SocijalniNatjecajClan entity)
        => entity.Adapt<SocijalniNatjecajClanDto>();

    public static SocijalniKucanstvoPodaciDto ToDto(this SocijalniNatjecajKucanstvoPodaci entity)
        => entity.Adapt<SocijalniKucanstvoPodaciDto>();

    public static SocijalniNatjecajBodovniPodaciDto ToDto(this SocijalniNatjecajBodovniPodaci entity)
        => entity.Adapt<SocijalniNatjecajBodovniPodaciDto>();

    // ---------- DTO → Entity metode ----------

    public static SocijalniNatjecajClan ToEntity(this SocijalniNatjecajClanDto dto, long zahtjevId)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var entity = dto.Adapt<SocijalniNatjecajClan>();
        entity.ZahtjevId = zahtjevId;
        return entity;
    }

    // ---------- Patch / MapOnto metode ----------

    public static void MapOnto(this SocijalniNatjecajBodovniPodaciDto dto, SocijalniNatjecajBodovniPodaci entity)
        => dto.Adapt(entity);

    public static void MapOnto(this SocijalniNatjecajClanDto dto, SocijalniNatjecajClan entity)
        => dto.Adapt(entity);

    public static void MapOnto(this SocijalniNatjecajOsnovnoEditDto dto, SocijalniNatjecajZahtjev entity)
    {
        dto.Adapt(entity);

        if (dto.RowVersion is not null)
            entity.RowVersion = dto.RowVersion;

        if (dto.RezultatObrade.HasValue)
            entity.ManualniRezultatObrade = dto.RezultatObrade.Value;
    }
}
