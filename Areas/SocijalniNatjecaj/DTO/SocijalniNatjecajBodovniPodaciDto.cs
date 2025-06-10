namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

public class SocijalniNatjecajBodovniPodaciDto
{
    public long ZahtjevId { get; set; }

    // Sastav kućanstva
    public byte BrojUzdrzavanePunoljetneDjece { get; set; }

    // Socijalno-zdravstveni status
    public bool PrimateljZajamceneMinimalneNaknade { get; set; }
    public bool StatusRoditeljaNjegovatelja { get; set; }
    public bool KorisnikDoplatkaZaPomoc { get; set; }
    public byte BrojOdraslihKorisnikaInvalidnine { get; set; }
    public byte BrojMaloljetnihKorisnikaInvalidnine { get; set; }

    // Posebne okolnosti
    public bool ZrtvaObiteljskogNasilja { get; set; }
    public byte BrojOsobaUAlternativnojSkrbi { get; set; }
    public byte BrojMjeseciObranaSuvereniteta { get; set; }
    public byte BrojClanovaZrtavaSeksualnogNasilja { get; set; }
    public byte BrojCivilnihStradalnika { get; set; }
    
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedByUserName { get; set; }
    public string? UpdatedByUserName { get; set; }
}