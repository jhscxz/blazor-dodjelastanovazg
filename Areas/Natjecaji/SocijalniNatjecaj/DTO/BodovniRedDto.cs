namespace DodjelaStanovaZG.Areas.Natjecaji.SocijalniNatjecaj.DTO;

public class BodovniRed(string kategorija, string kriterij, string vrijednost)
{
    public string Kategorija { get; } = kategorija;
    public string Kriterij { get; } = kriterij;
    public string Vrijednost { get; } = vrijednost;
}