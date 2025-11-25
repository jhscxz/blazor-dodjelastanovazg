using DodjelaStanovaZG.DTO;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

public class SocijalniNatjecajBodovniPodaciDto : AuditableDto
{
    public long ZahtjevId { get; set; }
    public byte BrojUzdrzavanePunoljetneDjece { get; set; }
    public bool PrimateljZajamceneMinimalneNaknade { get; set; }
    public bool StatusRoditeljaNjegovatelja { get; set; }
    public bool KorisnikDoplatkaZaPomoc { get; set; }
    public byte BrojOdraslihKorisnikaInvalidnine { get; set; }
    public byte BrojMaloljetnihKorisnikaInvalidnine { get; set; }
    public bool ZrtvaObiteljskogNasilja { get; set; }
    public byte BrojOsobaUAlternativnojSkrbi { get; set; }
    public byte BrojMjeseciObranaSuvereniteta { get; set; }
    public byte BrojClanovaZrtavaSeksualnogNasilja { get; set; }
    public byte BrojCivilnihStradalnika { get; set; }

}