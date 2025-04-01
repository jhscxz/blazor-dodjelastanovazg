using System.ComponentModel.DataAnnotations;
using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Models;

public class DokazniDokument
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Naziv { get; set; } = string.Empty;

    [Required]
    public DokumentacijaTip Tip { get; set; } // Clan, Kucanstvo, itd.

    public bool Obavezno { get; set; } = false;

    public ICollection<DostavljenaDokumentacijaClan> DostavljeniDokumenti { get; set; } = [];
}