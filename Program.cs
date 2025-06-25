using DodjelaStanovaZG.Components;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);

// =============================
// KONFIGURACIJA SERVISA
// =============================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ImaPristupAdmin",
        policy => policy.RequireRole("Management"));
    options.AddPolicy("MozeUnositi",
        policy => policy.RequireRole("Referent", "Management"));
});


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
    options.Conventions.AllowAnonymousToPage("/Identity/Account/Register");
    options.Conventions.AllowAnonymousToPage("/Identity/Account/RegisterConfirmation");
    options.Conventions.AllowAnonymousToPage("/Identity/Account/ConfirmEmail");
    options.Conventions.AllowAnonymousToPage("/Identity/Account/ResendEmailConfirmation");
});

builder.Services.AddApplicationServices();
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

RuntimeHelpers.RunClassConstructor(typeof(DodjelaStanovaZG.Helpers.MappingExtensions).TypeHandle);

// Dodaj hrvatski jezik kao default
var culture = new CultureInfo("hr-HR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture);
    options.SupportedCultures = [culture];
    options.SupportedUICultures = [culture];
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
app.UseAntiforgery();

app.UseRequestLocalization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.MapRazorPages();

// Pokreni seeding preko naredbe "dotnet run -- seed"
if (args.Contains("seed"))
{
    await DatabaseSeeder.SeedAsync(app);
    return;
}

app.MapControllers();
app.Run();
