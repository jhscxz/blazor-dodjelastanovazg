using DodjelaStanovaZG.Components.UI;

namespace DodjelaStanovaZG.Services;

public class BreadcrumbService
{
    private readonly Dictionary<string, (string? Url, string? CssClass)> _map = new()
    {
        ["Početna"] = ("/", null),
        ["Admin Nadzorna ploča"] = ("/admin", null),
        ["Korisnici"] = ("/admin/users", "text-red-500 font-bold"),
        ["Profil"] = ("/profile", null),
        ["Promjena lozinke"] = (null, "text-red-500 font-bold"),
        // Dodaj ostale prema potrebi
    };

    public List<Breadcrumbs.BreadcrumbItem> Create(params string[] labels)
    {
        return labels.Select(label =>
        {
            var trimmed = label.Trim();
            var (url, css) = _map.TryGetValue(trimmed, out var val)
                ? val
                : (null, null);

            return new Breadcrumbs.BreadcrumbItem
            {
                Text = trimmed,
                Url = url,
                CssClass = css
            };
        }).ToList();
    }
}