namespace DodjelaStanovaZG.Data.Seeders.Models;

public class SeedUserModel
{
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public List<string> Roles { get; init; } = [];
}