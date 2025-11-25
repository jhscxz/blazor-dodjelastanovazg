using DodjelaStanovaZG.Components.UI;
using Microsoft.Extensions.Logging;

namespace DodjelaStanovaZG.Services;

public class BreadcrumbService(ILogger<BreadcrumbService> logger)
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
            var found = _map.TryGetValue(trimmed, out var val);
            var (url, css) = found
                ? val
                : (null, null);

            if (!found)
                logger.LogWarning("Unknown breadcrumb label: {Label}", trimmed);
            
            return new Breadcrumbs.BreadcrumbItem
            {
                Text = trimmed,
                Url = url,
                CssClass = css
            };
        }).ToList();
    }
}