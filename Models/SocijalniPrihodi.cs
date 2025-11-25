using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DodjelaStanovaZG.Models
{
    public class SocijalniPrihodi : AuditableEntity
    {
        [Key, ForeignKey(nameof(KucanstvoPodaci))]
        public long Id { get; set; }
        public SocijalniNatjecajKucanstvoPodaci? KucanstvoPodaci { get; set; } = null!;
        [Required]
        [Precision(10, 2)]
        public decimal UkupniPrihodKucanstva { get; set; }
        [Precision(10, 2)]
        public decimal PrihodPoClanu { get; set; }
        [Precision(7, 2)]
        public decimal? PostotakProsjeka { get; set; } 
        public bool IspunjavaUvjetPrihoda { get; set; } 
    }
}