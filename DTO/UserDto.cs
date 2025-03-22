namespace DodjelaStanovaZG.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        // Dodano svojstvo za role
        public string Roles { get; set; }
    }
}