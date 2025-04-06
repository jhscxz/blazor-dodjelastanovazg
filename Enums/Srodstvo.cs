using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Enums;

public enum Srodstvo
{
    [Display(Name = "Podnositelj zahtjeva")]
    PodnositeljZahtjeva = 1,

    [Display(Name = "Bračni drug")]
    BracniDrug = 2,

    [Display(Name = "Izvanbračni drug")]
    IzvanbracniDrug = 3,

    [Display(Name = "Životni partner")]
    ZivotniPartner = 4,

    [Display(Name = "Neformalni životni partner")]
    NeformalniZivotniPartner = 5,

    [Display(Name = "Srodnik po krvnoj liniji")]
    SrodnikPoKrvnojLiniji = 6,

    [Display(Name = "Srodnik po pobočnoj liniji")]
    SrodnikPoPobocnojLiniji = 7,

    [Display(Name = "Partner srodnika")]
    PartnerSrodnika = 8,

    [Display(Name = "Životni ili neformalni partner srodnika")]
    ZivotniIliNeformalniPartnerSrodnika = 9,

    [Display(Name = "Pastorak")]
    Pastorak = 10,

    [Display(Name = "Posvojenik")]
    Posvojenik = 11,

    [Display(Name = "Posvojitelj")]
    Posvojitelj = 12,

    [Display(Name = "Uzdržavani član")]
    UzdrzavaniClan = 13
}