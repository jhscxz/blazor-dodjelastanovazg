using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DodjelaStanovaZG.Helpers;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum? enumValue)
    {
        // Provjera da enumValue nije null
        if (enumValue == null)
        {
            return string.Empty;
        }

        // Dohvat članova enuma
        var fieldInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

        // Ako nije pronađen član, vratiti naziv enum vrijednosti
        if (fieldInfo == null)
        {
            return enumValue.ToString();
        }

        // Dohvat atributa Display za član
        var attribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();

        // Ako atribut postoji, vratiti naziv iz atributa, inače naziv enum vrijednosti
        return attribute?.Name ?? enumValue.ToString();
    }
}