using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.DTO;

public class NatjecajDto
{
    public long? Id { get; set; } // null kod kreiranja, postavljen kod ažuriranja

    [Required]
    [Range(1, 2)]
    public byte PriustiviIliSocijalni { get; set; }

    [Required]
    public int Godina { get; set; }

    [Required]
    public int Klasa { get; set; }

    [Required]
    [Range(0, 100000)]
    public decimal ProsjekPlace { get; set; }

    [Required]
    [Range(1, 2)]
    public byte Zakljucen { get; set; } = 1;

    [Required]
    public DateOnly DatumObjave { get; set; }

    [Required]
    public DateOnly RokZaPrijavu { get; set; }
}