namespace DodjelaStanovaZG.Areas.Identity.Pages.Account.Model;

public class LoginLogoutButtonModel
{
    // Tekst na gumbu (npr. "Prijavi se" ili "Odjavi se")
    public string ButtonText { get; set; } = "Button";
    
    // CSS klase za stil gumba (npr. pozadinska boja, hover, padding, itd.)
    public string ButtonClasses { get; set; } = "inline-flex items-center gap-2 bg-blue-500 hover:bg-blue-600 text-white font-bold text-sm py-2 px-4 rounded focus:outline-none focus:ring-2 transition";
    
    // Tip ikone: "login" ili "logout"
    public string IconType { get; set; } = "login";
}