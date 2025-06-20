// Helpers/MappingExtensions.cs
#nullable enable
using Mapster;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;
using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Helpers;

/// <summary>
/// Central place for mapping between Socijalni natječaj entiteti ↔ DTO‑i.
/// Od .NET9 koristimo Mapster Source Generator za compile‑time, reflection‑free projekte.
/// </summary>
public static partial class MappingExtensions
{
    static MappingExtensions()
    {
        // Globalna postavka: ignoriramo null vrijednosti kod DTO → entity mapa
        TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);

        // ----- Entity → DTO --------------------------------------------------
        TypeAdapterConfig<SocijalniNatjecajZahtjev, SocijalniNatjecajZahtjevDto>
            .NewConfig()
            .Map(dest => dest.ImePrezime,
                 src => src.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)!.ImePrezime)
            .Map(dest => dest.Oib,
                 src => src.Clanovi.FirstOrDefault(c => c.Srodstvo == Srodstvo.PodnositeljZahtjeva)!.Oib);

        // Ostali entiteti imaju 1‑na‑1 imena fieldova pa ne traže dodatnu konfiguraciju
    }

    // ------------------- Entity → DTO --------------------------------------

    public static SocijalniNatjecajZahtjevDto ToDto(this SocijalniNatjecajZahtjev entity)
        => entity.Adapt<SocijalniNatjecajZahtjevDto>();

    public static SocijalniNatjecajClanDto ToDto(this SocijalniNatjecajClan entity)
        => entity.Adapt<SocijalniNatjecajClanDto>();

    public static SocijalniKucanstvoPodaciDto ToDto(this SocijalniNatjecajKucanstvoPodaci entity)
        => entity.Adapt<SocijalniKucanstvoPodaciDto>();

    public static SocijalniNatjecajBodovniPodaciDto ToDto(this SocijalniNatjecajBodovniPodaci entity)
        => entity.Adapt<SocijalniNatjecajBodovniPodaciDto>();

    // ------------------- DTO → Entity --------------------------------------

    public static SocijalniNatjecajClan ToEntity(this SocijalniNatjecajClanDto dto, long zahtjevId)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var entity = dto.Adapt<SocijalniNatjecajClan>();
        entity.ZahtjevId = zahtjevId;
        return entity;
    }

    // ------------------- MapOnto / Patch‑map -------------------------------

    public static void MapOnto(this SocijalniNatjecajBodovniPodaciDto dto, SocijalniNatjecajBodovniPodaci entity)
        => dto.Adapt(entity);

    public static void MapOnto(this SocijalniNatjecajClanDto dto, SocijalniNatjecajClan entity)
        => dto.Adapt(entity);

    public static void MapOnto(this SocijalniNatjecajOsnovnoEditDto dto, SocijalniNatjecajZahtjev entity)
    {
        dto.Adapt(entity);

        // Manualne nadogradnje – Mapster neće dotaknuti RowVersion jer je byte[]
        if (dto.RowVersion is not null)
            entity.RowVersion = dto.RowVersion;

        // Manualni rezultat obrade update‑amo samo kad je poslan
        if (dto.RezultatObrade.HasValue)
            entity.ManualniRezultatObrade = dto.RezultatObrade.Value;
    }
}
