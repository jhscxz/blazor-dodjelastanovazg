using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;

public class NatjecajDto
{
    public string Vrsta { get; set; } = string.Empty;
    [Required(ErrorMessage = "Klasa je obavezna")]
    public int? Klasa { get; set; }

    [Required(ErrorMessage = "Prosjek plaće je obavezan")]
    public decimal? ProsjekPlace { get; set; }

    [Required(ErrorMessage = "Datum objave je obavezan")]
    public DateOnly? DatumObjave { get; set; }

    [Required(ErrorMessage = "Rok za prijavu je obavezan")]
    public DateOnly? RokZaPrijavu { get; set; }
    public string Status { get; set; } = "Aktivan";
    
    [JsonIgnore]
    public DateTime? DatumObjaveDateTime
    {
        get => DatumObjave?.ToDateTime(TimeOnly.MinValue);
        set
        {
            if (value.HasValue)
                DatumObjave = DateOnly.FromDateTime(value.Value);
            else
                DatumObjave = null;
        }
    }
    
    [JsonIgnore]
    public DateTime? RokZaPrijavuDateTime
    {
        get => RokZaPrijavu?.ToDateTime(TimeOnly.MinValue);
        set
        {
            if (value.HasValue)
                RokZaPrijavu = DateOnly.FromDateTime(value.Value);
            else
                RokZaPrijavu = null;
        }
    }

}