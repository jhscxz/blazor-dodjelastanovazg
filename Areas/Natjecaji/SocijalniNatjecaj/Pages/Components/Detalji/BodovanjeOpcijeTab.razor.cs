using DodjelaStanovaZG.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Pages.Components.Detalji
{
    public partial class BodovanjeOpcijeTab
    {
        [Parameter] public long Id { get; set; }

        [Inject] private ISocijalniBodovniPodaciService BodovniService { get; set; } = default!;

        private DateTime CreatedAt { get; set; }
        private string? CreatedBy { get; set; }
        private string? CreatedByUserName { get; set; }
        private DateTime? UpdatedAt { get; set; }
        private string? UpdatedBy { get; set; }
        private string? UpdatedByUserName { get; set; }
        
        // Sastav
        private byte BrojUzdrzavanihPunoljetnih;

        // Socijalno-zdravstveni status
        private bool PrimateljZMN;
        private bool StatusNjegovatelja;
        private bool KorisnikDoplatka;
        private byte BrojOdraslihInvalidnina;
        private byte BrojMaloljetnihInvalidnina;

        // Posebne okolnosti
        private bool ZrtvaObiteljskogNasilja;
        private byte BrojAlternativnaSkrb1829;
        private byte BrojMjeseciObrana;
        private byte BrojZrtavaSeksualnogNasilja;
        private byte BrojCivilnihStradalnika;

        protected override async Task OnInitializedAsync()
        {
            var dto = await BodovniService.GetAsync(Id);

            BrojUzdrzavanihPunoljetnih = dto.BrojUzdrzavanePunoljetneDjece;

            PrimateljZMN = dto.PrimateljZajamceneMinimalneNaknade;
            StatusNjegovatelja = dto.StatusRoditeljaNjegovatelja;
            KorisnikDoplatka = dto.KorisnikDoplatkaZaPomoc;
            BrojOdraslihInvalidnina = dto.BrojOdraslihKorisnikaInvalidnine;
            BrojMaloljetnihInvalidnina = dto.BrojMaloljetnihKorisnikaInvalidnine;

            ZrtvaObiteljskogNasilja = dto.ZrtvaObiteljskogNasilja;
            BrojAlternativnaSkrb1829 = dto.BrojOsobaUAlternativnojSkrbi;
            BrojMjeseciObrana = dto.BrojMjeseciObranaSuvereniteta;
            BrojZrtavaSeksualnogNasilja = dto.BrojClanovaZrtavaSeksualnogNasilja;
            BrojCivilnihStradalnika = dto.BrojCivilnihStradalnika;
            
            CreatedAt = dto.CreatedAt;
            CreatedBy = dto.CreatedBy;
            CreatedByUserName = dto.CreatedByUserName;
            UpdatedAt = dto.UpdatedAt;
            UpdatedBy = dto.UpdatedBy;
            UpdatedByUserName = dto.UpdatedByUserName;
        }
    }
}