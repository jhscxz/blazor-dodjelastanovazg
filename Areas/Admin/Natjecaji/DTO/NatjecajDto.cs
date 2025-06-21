using Newtonsoft.Json;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;

public class NatjecajDto
{
    public string Vrsta { get; set; } = string.Empty;
    public int Klasa { get; set; }
    public decimal ProsjekPlace { get; set; }
    public DateOnly DatumObjave { get; set; }
    public DateOnly RokZaPrijavu { get; set; }
    public string Status { get; set; } = "Aktivan";
    
    [JsonIgnore]
    public DateTime? DatumObjaveDateTime
    {
        get => DatumObjave.ToDateTime(TimeOnly.MinValue);
        set
        {
            if (value.HasValue)
                DatumObjave = DateOnly.FromDateTime(value.Value);
        }
    }
    
    [JsonIgnore]
    public DateTime? RokZaPrijavuDateTime
    {
        get => RokZaPrijavu.ToDateTime(TimeOnly.MinValue);
        set
        {
            if (value.HasValue)
                RokZaPrijavu = DateOnly.FromDateTime(value.Value);
        }
    }

}