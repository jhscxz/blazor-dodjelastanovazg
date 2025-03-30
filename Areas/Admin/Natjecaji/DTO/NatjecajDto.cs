namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;

public class NatjecajDto
{
    public string Vrsta { get; set; } = string.Empty;
    public int Klasa { get; set; }
    public decimal ProsjekPlace { get; set; }
    public DateTime? DatumObjave { get; set; }
    public DateTime? RokZaPrijavu { get; set; }
    public string Status { get; set; } = "Aktivan";
}