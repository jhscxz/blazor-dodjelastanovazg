using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev;
using DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.Services.SocijalniZahtjev.ISocijalniZahtjev;
using DodjelaStanovaZG.Data.Seeders;
using DodjelaStanovaZG.Infrastructure;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using DodjelaStanovaZG.Services.Interfaces;

namespace DodjelaStanovaZG.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<NatjecajSeedService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<BreadcrumbService>();
        services.AddScoped<INatjecajService, NatjecajService>();
        services.AddScoped<SeedService>();
        services.AddScoped<INatjecajOdabirService, NatjecajOdabirService>();
        services.AddScoped<ISocijalniBodovniPodaciService, SocijalniBodovniPodaciService>();
        services.AddScoped<ISocijalniClanService, SocijalniClanService>();
        services.AddScoped<ISocijalniKucanstvoService, SocijalniKucanstvoService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<ISocijalniBodoviService, SocijalniBodoviService>();
        services.AddScoped<ISocijalniBodovnaGreskaService, SocijalniBodovnaGreskaService>();
        services.AddScoped<IWordExportService, WordExportService>();
        services.AddScoped<SocijalniZahtjevFactory>();
        services.AddScoped<ISocijalniZahtjevFactory, SocijalniZahtjevFactory>();
        services.AddScoped<ISocijalniZahtjevReadService, SocijalniZahtjevReadService>();
        services.AddScoped<ISocijalniZahtjevWriteService, SocijalniZahtjevWriteService>();
        services.AddScoped<ISocijalniZahtjevProcessor, SocijalniZahtjevProcessor>();
        services.AddScoped<ISocijalniZahtjevGreskaService, SocijalniZahtjevGreskaService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<ISocijalniZahtjevFormHandler, SocijalniZahtjevFormHandler>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}