using DodjelaStanovaZG.Components;
using DodjelaStanovaZG.Data;
using DodjelaStanovaZG.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =============================
// KONFIGURACIJA SERVISA
// =============================

// Registracija DbContext-a
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

// Konfiguracija Identity sustava s podrškom za role
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddRoles<IdentityRole>() // Omogućuje rad s rolama
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(); // Omogućava autorizaciju u aplikaciji

// Konfiguracija Identity kolačića
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Postavlja putanju za prijavu
});

// Identity opcije
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true; // Svaki korisnik mora imati jedinstven email
    options.SignIn.RequireConfirmedAccount = false; // Postavi na `true` ako koristiš email potvrdu
});

// Razor komponenta & autorizacija
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // Svi Razor Pages zahtijevaju login
    options.Conventions.AllowAnonymousToPage("/Identity/Account/Login"); // Login stranica dostupna svima
});

// Registracija servisa
builder.Services.AddScoped<SeedService>(); // Dodaj SeedService
builder.Services.AddScoped<SignInManager<IdentityUser>>();

// =============================
// KONFIGURACIJA APLIKACIJE
// =============================

var app = builder.Build();

// Postavljanje middleware-a za produkciju
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

// Mapiranje Razor komponenti
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .DisableAntiforgery(); // Isključuje Antiforgery (koristi s oprezom!)

app.MapRazorPages(); // Omogućava klasične Razor Pages rute

// Seed podataka nakon pokretanja aplikacije
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedService = services.GetRequiredService<SeedService>();
    await seedService.SeedAdminUser();
}

app.Run();
