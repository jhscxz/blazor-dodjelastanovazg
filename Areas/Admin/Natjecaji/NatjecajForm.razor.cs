using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Areas.Admin.Natjecaji.Services;
using DodjelaStanovaZG.Components.UI;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji;

public partial class NatjecajForm : ComponentBase
{
    [Parameter] public int? Klasa { get; set; }

    [Inject] public required NavigationManager Navigation { get; set; }
    [Inject] public required INatjecajService NatjecajService { get; set; }

    protected MudForm _form = null!;
    protected NatjecajDto Natjecaj { get; set; } = new();
    protected List<string> ErrorMessages { get; set; } = [];

    protected string FormTitle => Klasa == null ? "Dodaj natječaj" : $"Uredi natječaj ({Klasa})";

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems =>
    [
        new() { Text = "Početna", Url = "/" },
        new() { Text = "Admin Nadzorna ploča", Url = "/admin" },
        new() { Text = "Natječaji", Url = "/admin/natjecaji" },
        new() { Text = Klasa == null ? "Dodaj" : $"Uredi: {Klasa}", CssClass = "text-red-500 font-bold" }
    ];

    protected override async Task OnInitializedAsync()
    {
        if (Klasa is not null)
        {
            var existing = await NatjecajService.GetByKlasaAsync(Klasa.Value);
            if (existing is not null)
                Natjecaj = existing;
        }
    }

    protected async Task SubmitForm()
    {
        ErrorMessages.Clear();
        await _form.Validate();

        if (!_form.IsValid)
        {
            ErrorMessages.Add("Molimo ispravite greške u formi.");
            return;
        }

        bool result = Klasa is null
            ? await NatjecajService.CreateAsync(Natjecaj)
            : await NatjecajService.UpdateAsync(Klasa.Value, Natjecaj);

        if (!result)
        {
            ErrorMessages.Add("Dogodila se pogreška pri spremanju.");
            return;
        }

        Navigation.NavigateTo("/admin/natjecaji");
    }

    protected void Povratak() => Navigation.NavigateTo("/admin/natjecaji");
}
