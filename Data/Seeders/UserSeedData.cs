using DodjelaStanovaZG.Data.Seeders.Models;

namespace DodjelaStanovaZG.Data.Seeders;

public static class UserSeedData
{
    public static SeedUserModel GetManagement() => new() { Username = "jhusic", Email = "jhsc.xz@gmail.com", Password = "Admin@123", Roles = ["Management"] };

    private const string SectionName = "SeedUsers";
    
    public static SeedUserModel GetManagement(IConfiguration configuration) =>
        configuration.GetSection($"{SectionName}:Management").Get<SeedUserModel>() ?? new SeedUserModel();

    public static List<SeedUserModel> GetUsers(IConfiguration configuration) =>
        configuration.GetSection($"{SectionName}:Users").Get<List<SeedUserModel>>() ?? [];
}
