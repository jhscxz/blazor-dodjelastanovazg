using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Enums;

public enum DokazniDokumentVrsta : byte
{
    [Display(Name = "Ugovor o slobodnoj najamnini ili Obrazac 1")]
    UNajmu = 1,

    [Display(Name = "Izjava ili drugi dokument kojim se dokazuje stanovanje kod roditelja ili supružnikovih roditelja")]
    KodRoditelja = 2,

    [Display(Name = "Dokument kojim se dokazuje zaštićeni najam")]
    ZasticeniNajam = 3,

    [Display(Name = "Potvrda o organiziranom stanovanju")]
    OrganiziranoStanovanje = 4,

    [Display(Name = "Isprava kojom se dokazuje status beskučnika")]
    Beskucnik = 5,

    [Display(Name = "Izjava o neposjedovanju nekretnine")]
    NeposjedovanjeNekretnine = 6,

    [Display(Name = "Izjava o izvanbračnoj zajednici")]
    IzvanbracnaZajednica = 7,

    [Display(Name = "Potvrda o statusu osobe pod međunarodnom zaštitom")]
    MedjunarodnaZastita = 8,

    [Display(Name = "Uvjerenje o prebivalištu")]
    Prebivaliste = 9,

    [Display(Name = "Potvrda o dohotku i primicima u prethodnoj godini")]
    Dohodak = 10,

    [Display(Name = "Rodni list / Vjenčani list / Izvadak iz registra")]
    ObiteljskiStatus = 11,

    [Display(Name = "Presuda o razvodu braka i plan roditeljske skrbi")]
    RazvodIRoditeljskiPlan = 12,

    [Display(Name = "Dokaz o školovanju punoljetnog uzdržavanog djeteta")]
    SkolovanjePunoljetnogDjeteta = 13,

    [Display(Name = "Potvrda o invaliditetu (odrasli korisnik)")]
    InvaliditetOdrasli = 14,

    [Display(Name = "Potvrda o invaliditetu (maloljetni korisnik)")]
    InvaliditetMaloljetni = 15,

    [Display(Name = "Dokaz o statusu roditelja njegovatelja")]
    RoditeljNjegovatelj = 16,

    [Display(Name = "Dokaz o statusu korisnika zajamčene minimalne naknade")]
    MinimalnaNaknada = 17,

    [Display(Name = "Dokaz o statusu žrtve seksualnog nasilja u Domovinskom ratu")]
    ZrtvaSeksualnogNasilja = 18,

    [Display(Name = "Dokaz o statusu civilnog stradalnika Domovinskog rata")]
    CivilniStradalnik = 19,

    [Display(Name = "Dokaz o statusu žrtve obiteljskog nasilja")]
    ZrtvaObiteljskogNasilja = 20,

    [Display(Name = "Dokaz o alternativnoj skrbi (udomiteljstvo, dom)")]
    AlternativnaSkrb = 21,

    [Display(Name = "Izvadak iz matice umrlih za preminulog člana")]
    MaticaUmrlih = 22
}
