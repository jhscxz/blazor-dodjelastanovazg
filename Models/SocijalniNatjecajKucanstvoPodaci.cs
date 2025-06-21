using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Models
{
    public class SocijalniNatjecajKucanstvoPodaci : AuditableEntity
    {
        [Key]
        public long Id { get; init; }
        [Required]
        [ForeignKey(nameof(Zahtjev))]
        public long ZahtjevId { get; init; }
        public SocijalniNatjecajZahtjev Zahtjev { get; set; } = null!;
        public DateOnly? PrebivanjeOd { get; set; }
        [Required]
        public StambeniStatusKucanstva StambeniStatusKucanstva { get; set; }
        [Required]
        public SastavKucanstva SastavKucanstva { get; set; }
        public SocijalniPrihodi? Prihod { get; set; }
    }
}