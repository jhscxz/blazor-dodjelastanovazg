using DodjelaStanovaZG.Areas.Admin.Natjecaji.DTO;
using DodjelaStanovaZG.Components.UI;
using DodjelaStanovaZG.Helpers;
using DodjelaStanovaZG.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Natjecaji;

public partial class NatjecajForm : ComponentBase
{
    [Parameter] public int? Klasa { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    [Inject] public required IUnitOfWork UnitOfWork { get; set; }
    private MudForm _form = null!;
    protected NatjecajDto Natjecaj { get; set; } = new();
    private List<string> ErrorMessages { get; set; } = [];

    private string FormTitle => Klasa == null ? "Dodaj natječaj" : $"Uredi natječaj ({Klasa})";

    protected List<Breadcrumbs.BreadcrumbItem> BreadcrumbItems => BreadcrumbProvider.AdminNatjecajForm(Klasa);

    protected override async Task OnInitializedAsync()
    {
        if (Klasa is not null)
        {
            var existing = await UnitOfWork.NatjecajiService.GetByKlasaAsync(Klasa.Value);

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
            ? await UnitOfWork.NatjecajiService.CreateAsync(Natjecaj)
            : await UnitOfWork.NatjecajiService.UpdateAsync(Klasa.Value, Natjecaj);

        if (!result)
        {
            ErrorMessages.Add("Dogodila se pogreška pri spremanju.");
            return;
        }

        Navigation.NavigateTo("/admin/natjecaji");
    }

    protected void Povratak() => Navigation.NavigateTo("/admin/natjecaji");
}