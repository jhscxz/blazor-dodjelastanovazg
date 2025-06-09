using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.Pages.Components.Detalji
{
    public partial class BodovanjeOpcijeTab
    {
        [Parameter] public long Id { get; set; }

        // Sastav
        private byte BrojUzdrzavanihPunoljetnih { get; set; } = 1;

        // Socijalno-zdravstveni status
        private bool PrimateljZMN { get; set; } = true;
        private bool StatusNjegovatelja { get; set; } = false;
        private bool KorisnikDoplatka { get; set; } = true;
        private byte BrojOdraslihInvalidnina { get; set; } = 2;
        private byte BrojMaloljetnihInvalidnina { get; set; } = 1;

        // Posebne okolnosti
        private bool ZrtvaObiteljskogNasilja { get; set; } = false;
        private byte BrojAlternativnaSkrb1829 { get; set; } = 1;
        private byte BrojMjeseciObrana { get; set; } = 12;
        private byte BrojZrtavaSeksualnogNasilja { get; set; } = 0;
        private byte BrojCivilnihStradalnika { get; set; } = 1;
    }
}