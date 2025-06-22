using DodjelaStanovaZG.Components.UI;

namespace DodjelaStanovaZG.Helpers;

public static class BreadcrumbProvider
{
    public static List<Breadcrumbs.BreadcrumbItem> ZahtjevDodaj(long natjecajId) => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Dodaj zahtjev", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> ZahtjevDetalji(long zahtjevId) => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", Url = $"/socijalni/detalji/{zahtjevId}", CssClass = "text-blue-600" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> Home() => new()
    {
        new() { Text = "Početna", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> SocijalniIndex() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> SocijalniPregled() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Pregled zapisa", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> SocijalniDetalji() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> ZahtjevEdit(long zahtjevId, string title) => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Socijalni natječaji", Url = "/socijalni-natjecaj" },
        new() { Text = "Detalji zahtjeva", Url = $"/socijalni/detalji/{zahtjevId}" },
        new() { Text = title, CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> AdminDashboard() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> AdminNatjecajiPregled() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new() { Text = "Natječaji", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> AdminNatjecajForm(int? klasa) => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new() { Text = "Natječaji", Url = "/admin/natjecaji" },
        new() { Text = klasa == null ? "Dodaj" : $"Uredi: {klasa}", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> AdminUsers() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new(text: "Korisnici", cssClass: "text-red-500 font-bold")
    };

    public static List<Breadcrumbs.BreadcrumbItem> AdminUserAdd() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new() { Text = "Korisnici", Url = "/admin/users" },
        new() { Text = "Dodaj korisnika", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> AdminUserEdit() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new() { Text = "Korisnici", Url = "/admin/users" },
        new() { Text = "Uredi korisnika", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> AdminUserResetPassword() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new() { Text = "Korisnici", Url = "/admin/users" },
        new() { Text = "Promijeni lozinku", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> Profile() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Moj profil", CssClass = "text-red-500 font-bold" }
    };

    public static List<Breadcrumbs.BreadcrumbItem> ChangeMyPassword() => new()
    {
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Profil", Url = "/profile" },
        new() { Text = "Promjena lozinke", CssClass = "text-red-500 font-bold" }
    };
}