using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Areas.SocijalniNatjecaj.Services;
using DodjelaStanovaZG.Components;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================
// KONFIGURACIJA SERVISA
// =============================

// Registracija DbContext-a s SQL Serverom
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfiguracija Identity sustava s podrškom za role
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        // Identity opcije
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false; // Promijenite na true ako koristite potvrdu email-a
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Dodavanje autorizacije
builder.Services.AddAuthorization();

// Konfiguracija kolačića za Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Postavlja putanju za prijavu
});

// Registracija Razor komponenti i Razor Pages
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // Svi Razor Pages zahtijevaju login
    options.Conventions.AllowAnonymousToPage("/Identity/Account/Login"); // Login stranica dostupna svima
});

// Registracija dodatnih servisa
builder.Services.AddMudServices();
builder.Services.AddScoped<UserSeedService>();
builder.Services.AddScoped<NatjecajSeedService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<BreadcrumbService>();
builder.Services.AddScoped<INatjecajService, NatjecajService>();
builder.Services.AddScoped<SeedService>();
builder.Services.AddScoped<INatjecajOdabirService, NatjecajOdabirService>();
builder.Services.AddScoped<ISocijalniNatjecajService, SocijalniNatjecajService>();


// =============================
// KONFIGURACIJA APLIKACIJE
// =============================
var app = builder.Build();

// Middleware konfiguracija za produkciju
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Autentifikacija korisnika
app.UseAuthorization();  // Autorizacija pristupa

// Mapiranje Razor komponenti s interaktivnim renderiranjem i isključenim antiforgery zaštitom (koristite s oprezom)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .DisableAntiforgery();

// Mapiranje klasičnih Razor Pages ruta
app.MapRazorPages();

// Seed podataka (primjerice, admin korisnika) nakon pokretanja aplikacije
using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<UserSeedService>();
    await seedService.SeedAdminUser();
    await seedService.SeedAllUsers();
}

using (var scope = app.Services.CreateScope())
{
    var natjecajSeed = scope.ServiceProvider.GetRequiredService<NatjecajSeedService>();
    await natjecajSeed.SeedNatjecajiAsync();
}

app.Run();
