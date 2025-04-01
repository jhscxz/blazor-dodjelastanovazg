using DodjelaStanovaZG.Enums;
using DodjelaStanovaZG.Models;

namespace DodjelaStanovaZG.Seeders;

public static class DokazniDokumentSeeder
{
    public static List<DokazniDokument> GetObaveznaDokumentacija() =>
    [
        // ==================== KUCANSTVO ====================

        // --- Obavezno ---
        new() { Naziv = "Izjava da nitko ne posjeduje useljivu kuću ili stan (ovjerena kod javnog bilježnika)", Tip = DokumentacijaTip.Kucanstvo, Obavezno = true },

        // --- Neobavezno ---
        new() { Naziv = "Izjava o izvanbračnoj zajednici/partnerstvu s dva svjedoka (ovjerena)", Tip = DokumentacijaTip.Kucanstvo, Obavezno = false },
        new() { Naziv = "Potvrda o statusu osobe pod međunarodnom zaštitom", Tip = DokumentacijaTip.Kucanstvo, Obavezno = false },


        // ==================== CLAN ====================

        // --- Obavezno ---
        new() { Naziv = "Uvjerenje o prebivalištu za sve članove kućanstva", Tip = DokumentacijaTip.Clan, Obavezno = true },
        new() { Naziv = "Potvrda o visini dohotka i primitaka u prethodnoj godini.", Tip = DokumentacijaTip.Clan, Obavezno = true },
        new() { Naziv = "Uvjerenje o neposjedovanju nekretnina na području Grada Zagreba i Zagrebačke županije", Tip = DokumentacijaTip.Clan, Obavezno = true },
        new() { Naziv = "Rodni list, vjenčani list, izvadak iz registra partnerstva itd.", Tip = DokumentacijaTip.Clan, Obavezno = true },

        // --- Neobavezno ---
        new() { Naziv = "Izvadak iz matice umrlih (za preminulog roditelja/skrbnika)", Tip = DokumentacijaTip.Clan, Obavezno = false },


        // ==================== DODATNA ====================

        // --- Neobavezno ---
        new() { Naziv = "Presuda o razvodu braka i plan roditeljske skrbi", Tip = DokumentacijaTip.Dodatna, Obavezno = false },
        new() { Naziv = "Dokaz o školovanju uzdržavanog punoljetnog djeteta", Tip = DokumentacijaTip.Dodatna, Obavezno = false },


        // ==================== POSEBNA OKOLNOST ====================

        // --- Neobavezno ---
        new() { Naziv = "Potvrda o invaliditetu (odrasli korisnik)", Tip = DokumentacijaTip.PosebnaOkolnost, Obavezno = false },
        new() { Naziv = "Potvrda o invaliditetu (maloljetni korisnik)", Tip = DokumentacijaTip.PosebnaOkolnost, Obavezno = false },
        new() { Naziv = "Dokaz o statusu roditelja njegovatelja", Tip = DokumentacijaTip.PosebnaOkolnost, Obavezno = false },
        new() { Naziv = "Dokaz o statusu primatelja minimalne naknade", Tip = DokumentacijaTip.PosebnaOkolnost, Obavezno = false },
        new() { Naziv = "Dokaz o statusu žrtve seksualnog nasilja u Domovinskom ratu", Tip = DokumentacijaTip.PosebnaOkolnost, Obavezno = false },
        new() { Naziv = "Dokaz o statusu civilnog stradalnika Domovinskog rata", Tip = DokumentacijaTip.PosebnaOkolnost, Obavezno = false },
        new() { Naziv = "Dokaz o statusu žrtve obiteljskog nasilja", Tip = DokumentacijaTip.PosebnaOkolnost, Obavezno = false },
        new() { Naziv = "Dokaz o alternativnoj skrbi (dom, udomiteljstvo)", Tip = DokumentacijaTip.PosebnaOkolnost, Obavezno = false }
    ];
}