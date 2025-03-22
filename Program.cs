using DodjelaStanovaZG.Areas.Admin.Korisnici.Services;
using DodjelaStanovaZG.Components;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Components.Server;
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
builder.Services.AddScoped<SeedService>();
builder.Services.AddMudServices();
builder.Services.AddScoped<IUserService, UserService>();

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
    var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();
    await seedService.SeedAdminUser();
}

// Pokretanje seeda za ostale korisnike
/*using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();
    await seedService.SeedAllUsers();
}
*/


app.Run();
