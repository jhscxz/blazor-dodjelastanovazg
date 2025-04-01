namespace DodjelaStanovaZG.Areas.Admin.Korisnici.DTO
{
    public class UserDto
    {
        public required string Id { get; init; }
        public required string? UserName { get; init; }
        public required string? Email { get; init; }
        public required string Roles { get; init; }
        public bool IsLocked { get; init; }
    }
}