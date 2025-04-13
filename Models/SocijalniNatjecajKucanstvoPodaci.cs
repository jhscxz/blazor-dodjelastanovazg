using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DodjelaStanovaZG.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Models
{
    public class SocijalniNatjecajKucanstvoPodaci : AuditableEntity
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [ForeignKey(nameof(Zahtjev))]
        public long ZahtjevId { get; set; }
        public SocijalniNatjecajZahtjev Zahtjev { get; set; } = null!;
        [Required]
        [Precision(10, 2)]
        public decimal UkupniPrihodKucanstva { get; set; }
        public DateOnly? PrebivanjeOd { get; set; }
        [Required]
        public StambeniStatusKucanstva StambeniStatusKucanstva { get; set; }
        [Required]
        public SastavKucanstva SastavKucanstva { get; set; }
    }
}