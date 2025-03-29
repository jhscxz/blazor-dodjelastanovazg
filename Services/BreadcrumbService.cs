using DodjelaStanovaZG.Components.UI;

namespace DodjelaStanovaZG.Services;

public class BreadcrumbService
{
    private readonly Dictionary<string, (string? Url, string? CssClass)> _map = new()
    {
        ["Početna"] = ("/", null),
        ["Admin Nadzorna ploča"] = ("/admin", null),
        ["Korisnici"] = ("/admin/users", null),
        ["Profil"] = ("/profile", null),
        ["Promjena lozinke"] = ("/admin", "text-red-500 font-bold"),
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