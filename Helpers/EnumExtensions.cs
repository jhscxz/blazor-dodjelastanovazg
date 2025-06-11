using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DodjelaStanovaZG.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum? enumValue)
            => enumValue is null
                ? string.Empty
                : enumValue.GetType()
                      .GetField(enumValue.ToString())?
                      .GetCustomAttribute<DisplayAttribute>()?
                      .GetName()
                  ?? enumValue.ToString();
    }
}