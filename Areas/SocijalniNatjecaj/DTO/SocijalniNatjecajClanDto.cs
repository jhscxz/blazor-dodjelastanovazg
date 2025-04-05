using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO
{
    public class SocijalniNatjecajClanDto
    {
        public long Id { get; set; }
        public string ImePrezime { get; set; } = string.Empty;
        public string? Oib { get; set; }
        public Srodstvo? Srodstvo { get; set; }
    }
}