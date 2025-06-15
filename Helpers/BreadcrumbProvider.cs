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

    // Dodaj dalje po potrebi...
}