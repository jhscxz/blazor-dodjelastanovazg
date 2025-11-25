using Microsoft.AspNetCore.Components;

namespace DodjelaStanovaZG.Components.UI;

public partial class Breadcrumbs : LayoutComponentBase
{
    [Parameter]
    public List<BreadcrumbItem>? Items { get; set; }

    public class BreadcrumbItem
    {
        public BreadcrumbItem() { }

        public BreadcrumbItem(string? text, string? cssClass)
        {
            Text = text;
            CssClass = cssClass;
        }

        public string? Text { get; set; }
        public string? Url { get; set; }
        public string? CssClass { get; set; }
    }
}