using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

public class SocijalniBodovniDto
{
    [Required(ErrorMessage = "Ukupni prihod kućanstva je obavezan.")]
    public StambeniStatusKucanstva? StambeniStatusKucanstva { get; init; }
    public SastavKucanstva? SastavKucanstva { get; init; }
    
    public byte BrojUzdrzavanePunoljetneDjece { get; set; }
    public bool PrimateljZajamceneMinimalneNaknade { get; set; }
    public bool StatusRoditeljaNjegovatelja { get; set; }
    public bool KorisnikDoplatkaZaPomoc { get; set; }
    public byte BrojOdraslihKorisnikaInvalidnine { get; set; }
    public byte BrojMaloljetnihKorisnikaInvalidnine { get; set; }
    public bool ZrtvaObiteljskogNasilja { get; set; }
    public byte BrojOsobaUAlternativnojSkrbi { get; set; }
    public bool PodnositeljIznad55Godina { get; set; }
    public byte BrojMjeseciObranaSuvereniteta { get; set; }
    public byte BrojClanovaZrtavaSeksualnogNasilja { get; set; }
    public byte BrojCivilnihStradalnika { get; set; }
    
    public decimal? UkupniPrihodKucanstva { get; init; }
    public decimal? PrihodPoClanu { get; init; }
    public decimal? PostotakProsjeka { get; init; }
    public bool IspunjavaUvjetPrihoda { get; init; }
    public byte BrojMaloljetneDjece { get; init; }
}