using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DodjelaStanovaZG.Models;

public class Natjecaj
{
    [Key]
    public long Id { get; set; }

    [Required]
    [Range(1, 2)]
    public byte PriustiviIliSocijalni { get; set; } // 1 = priustivi, 2 = socijalni

    [Required]
    public int Klasa { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal ProsjekPlace { get; set; }

    [Required]
    [Range(1, 2)]
    public byte Zakljucen { get; set; } = 1; // 1 = ne, 2 = da

    [Required]
    public DateOnly DatumObjave { get; set; }

    [Required]
    public DateOnly RokZaPrijavu { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}