namespace DodjelaStanovaZG.Areas.Identity.Pages.Account.Model
{
    /// <summary>
    /// Model za gumb za prijavu/odjavu.
    /// </summary>
    public class LoginLogoutButtonModel
    {
        /// <summary>
        /// Tekst koji će se prikazati na gumbu (npr. "Prijavi se" ili "Odjavi se").
        /// </summary>
        public string ButtonText { get; set; } = "Button";
        
        /// <summary>
        /// CSS klase koje definiraju stil gumba (npr. pozadinska boja, hover efekti, padding, itd.).
        /// </summary>
        public string ButtonClasses { get; set; } = "inline-flex items-center gap-2 bg-blue-500 hover:bg-blue-600 text-white font-bold text-sm py-2 px-4 rounded focus:outline-none focus:ring-2 transition";
        
        /// <summary>
        /// Tip ikone koja se koristi – vrijednost može biti "login" ili "logout".
        /// </summary>
        public string IconType { get; set; } = "login";
    }
}