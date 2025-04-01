using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

public class SocijalniNatjecajDto
{
    public int Id { get; set; }
    public int Godina { get; set; }
    public byte PriustiviIliSocijalni { get; set; } // 1 = priuštivi, 2 = socijalni
}