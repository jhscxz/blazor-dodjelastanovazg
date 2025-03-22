namespace DodjelaStanovaZG.Areas.Admin.Korisnici.DTO
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string? UserName { get; set; }
        public required string? Email { get; set; }
        public required string Roles { get; set; }
    }
}