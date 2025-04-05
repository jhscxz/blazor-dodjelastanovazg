using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

public class SocijalniBodovniDto
{
    [Required(ErrorMessage = "Ukupni prihod kućanstva je obavezan.")]
    public decimal? UkupniPrihodKucanstva { get; set; }

    [Range(0, 10, ErrorMessage = "Unesite broj između 0 i 10.")]
    public byte? BrojGodinaPrebivanja { get; set; }

    public StambeniStatusKucanstva? StambeniStatusKucanstva { get; set; }

    public SastavKucanstva? SastavKucanstva { get; set; }

    public byte? BrojMaloljetneDjece { get; set; }
    public byte? BrojUzdrzavanePunoljetneDjece { get; set; }

    public byte? BrojMaloljetnihKorisnikaInvalidnine { get; set; }
    public byte? BrojOdraslihKorisnikaInvalidnine { get; set; }

    public byte? ZrtvaObiteljskogNasilja { get; set; }
    public byte? BrojOsobaUAlternativnojSkrbi { get; set; }

    [Range(0, 20, ErrorMessage = "Maksimalno 20 mjeseci.")]
    public byte? BrojMjeseciObranaSuvereniteta { get; set; }

    public byte? BrojClanovaZrtavaSeksualnogNasiljaDomovinskiRat { get; set; }
    public byte? BrojCivilnihStradalnika { get; set; }

    public byte? ManjeOd35Godina { get; set; }
    public byte? ObrazovanjeBaccMaster { get; set; }
}