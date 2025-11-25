using System.ComponentModel.DataAnnotations;

namespace DodjelaStanovaZG.Enums;

public enum SastavKucanstva : byte
{
    [Display(Name = "Samohrani roditelj")]
    SamohraniRoditelj = 1,

    [Display(Name = "Jednoroditeljska obitelj")]
    JednoroditeljskaObitelj = 2,

    [Display(Name = "Samačko kućanstvo")]
    SamackoKucanstvo = 3,

    [Display(Name = "Ostalo")]
    Ostalo = 4
}