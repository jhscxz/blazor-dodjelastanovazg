using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;
using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

using DodjelaStanovaZG.Components;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Data.Seeders;
using DodjelaStanovaZG.Infrastructure;
using DodjelaStanovaZG.Services;
using Serilog;

#region serilog

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("Pokretanje Blazor Server aplikacije");

#endregion

var builder = WebApplication.CreateBuilder(args);

// ovo interno mijenja ILogger
// ILogger<T> ➝ Serilog-ov Log.ForContext<T>()
builder.Host.UseSerilog();

#region Baza podataka

// AddDbContext registrira ApplicationDbContext za standardni DI s opsegom po zahtjevu.
// AddDbContextFactory omogućuje stvaranje instanci DbContexta po potrebi (race condition issue).
// nisu podržani paralelny queryji nad istom instancom konteksta
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

#endregion

#region Autentikacija i autorizacija

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
    options.AddPolicy("ImaPristupAdmin", policy => policy.RequireRole("Management"));
    options.AddPolicy("MozeUnositi", policy => policy.RequireRole("Referent", "Management"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

#endregion

#region UI i Blazor

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

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});

#endregion

#region Lokalizacija

var culture = new CultureInfo("hr-HR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture);
    options.SupportedCultures = [culture];
    options.SupportedUICultures = [culture];
});

#endregion

#region Sigurnosni i sistemski servisi

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

#endregion

#region Aplikacijski servisi

builder.Services.AddApplicationServices();
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Inicijalizacija mapiranja (Mapster - MappingExtensions)
RuntimeHelpers.RunClassConstructor(typeof(DodjelaStanovaZG.Helpers.MappingExtensions).TypeHandle);

#endregion

#region Konfiguracija aplikacije

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSecurityHeaders();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseRouting();

app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.UseRateLimiter();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization();

app.MapRazorPages();
app.MapControllers();

#endregion

#region Seed podataka

if (args.Contains("seed"))
{
    await DatabaseSeeder.SeedAsync(app);
    return;
}

#endregion

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplikacija se srušila!");
}
finally
{
    Log.CloseAndFlush();
}
