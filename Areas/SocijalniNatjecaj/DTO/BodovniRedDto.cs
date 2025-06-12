namespace DodjelaStanovaZG.Areas.SocijalniNatjecaj.DTO;

public class BodovniRed(string kategorija, string kriterij, string vrijednost)
{
    public string Kategorija { get; set; } = kategorija;
    public string Kriterij { get; set; } = kriterij;
    public string Vrijednost { get; set; } = vrijednost;
}