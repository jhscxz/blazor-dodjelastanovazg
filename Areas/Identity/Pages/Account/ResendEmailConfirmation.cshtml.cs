using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace DodjelaStanovaZG.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResendEmailConfirmationModel(
    UserManager<IdentityUser> userManager,
    IEmailSender emailSender) : PageModel
{
    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await userManager.FindByEmailAsync(Input.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Korisnik s navedenim e-mailom ne postoji.");
            return Page();
        }

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            values: new { area = "Identity", userId = user.Id, code });

        await emailSender.SendEmailAsync(Input.Email, "Confirm your email",
            $"Molimo potvrdite račun klikom na <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>ovaj link</a>.");

        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
    }
}