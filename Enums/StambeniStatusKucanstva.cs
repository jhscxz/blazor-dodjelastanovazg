using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Enums;

public enum StambeniStatusKucanstva : byte
{
    [Display(Name = "Status najmoprimca sa slobodno ugovorenom najamninom")]
    SlobodniNajam = 1,

    [Display(Name = "Osoba koja živi u stanu roditelja ili supružnikovih roditelja")]
    KodRoditelja = 2,

    [Display(Name = "Zaštićeni najmoprimac/podstanar ili predmnijevani najmoprimac")]
    ZasticeniNajmoprimac = 3,

    [Display(Name = "Korisnik organiziranog stanovanja")]
    OrganiziranoStanovanje = 4,

    [Display(Name = "Beskućnik")] 
    Beskucnik = 5,

    [Display(Name = "Ostalo")] 
    Ostalo = 6
}