using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services.IServices;
using DodjelaStanovaZG.Components;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Globalization;
using DodjelaStanovaZG.Infrastructure;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Localization;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);

// =============================
// KONFIGURACIJA SERVISA
// =============================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Identity/Account/Login");
});

builder.Services.AddMudServices();
builder.Services.AddScoped<NatjecajSeedService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<BreadcrumbService>();
builder.Services.AddScoped<INatjecajService, NatjecajService>();
builder.Services.AddScoped<SeedService>();
builder.Services.AddScoped<INatjecajOdabirService, NatjecajOdabirService>();
builder.Services.AddScoped<ISocijalniNatjecajService, SocijalniNatjecajService>();
builder.Services.AddScoped<ISocijalniNatjecajDetaljiService, SocijalniNatjecajDetaljiService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Dodaj hrvatski jezik kao default
var culture = new CultureInfo("hr-HR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture);
    options.SupportedCultures = new[] { culture };
    options.SupportedUICultures = new[] { culture };
});

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});



// =============================
// KONFIGURACIJA APLIKACIJE
// =============================

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .DisableAntiforgery()
    .RequireAuthorization();

app.MapRazorPages();

// Seed baze 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedAsync(services);

    var natjecajSeeder = services.GetRequiredService<NatjecajSeedService>();
    await natjecajSeeder.SeedNatjecajiAsync();
}

app.Run();
