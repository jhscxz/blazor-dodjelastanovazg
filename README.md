# Dodjela stanova po socioekonomskom statusu

<img src="./grb-zg-og.png" align="left" alt="Grad Zagreb" width="60">

Aplikacija za provedbu natječaja za dodjelu gradskih stanova prema socioekonomskom statusu. Razvijena je kao dio diplomskog rada i digitalizira unos zahtjeva, automatsko bodovanje prema kriterijima, pregled rezultata i izvoz podataka.

## Ključne mogućnosti
- Upravljanje natječajima (kreiranje, aktiviranje/zatvaranje, definicija rokova).
- Unos i obrada zahtjeva s podacima o kućanstvu, prihodima; evidencija osnovanosti (osnovan/nepotpun/neosnovan).
- Automatizirano bodovanje prema kriterijima socioekonomskog statusa te prikaz ukupnog zbroja bodova.
- Filtriranje i pretraga zahtjeva, detaljni pregled članova kućanstva i uvjeta prihoda.
- Izvoz obrađenih zahtjeva u Excel (OpenXML) te zapis audita i logova (Serilog).
- Uloge i prava pristupa: `Management` (administracija), `Referent` (unos/obrada), `ReferentReadOnly` (pregled).

## Tehnologije
- .NET 9.0, Blazor Server, MudBlazor komponentna biblioteka.
- Entity Framework Core (SQL Server, temporalne tablice) i ASP.NET Core Identity.
- Mapster za mapiranje modela; Serilog za logiranje; OpenXML za Excel izvoz.
- Tailwind 4 (po potrebi za stilove), xUnit + FluentAssertions + Moq za testove.

## Struktura projekta
- `Areas/Natjecaji/SocijalniNatjecaj` – UI i logika za unos, pregled i bodovanje zahtjeva.
- `Areas/Admin` – nadzorna ploča i administracija natječaja/korisnika.
- `Services` – domenski servisi (bodovanje, export, audit, lozinke, breadcrumbs).
- `Data` – EF Core kontekst, migracije i seed podaci (natječaji + korisnici).
- `Tests` – jedinični testovi servisa.

## Pokretanje lokalno
1) **Preduvjeti:** .NET 9 SDK, pristup SQL Server instanci; (opcionalno) Node za rebuild Tailwind-a.  
2) **Konfiguracija baze:** u `appsettings.json` postavi `ConnectionStrings:DefaultConnection` na svoj SQL Server.  
3) **Migracije i inicijalni podaci:**  
   ```bash
   dotnet restore
   dotnet run -- seed
   ```  
   Argument `seed` pokreće migracije i puni bazu početnim natječajima i korisnicima.  
4) **Pokretanje aplikacije:**  
   ```bash
   dotnet run
   ```  
   Aplikacija će se podići na lokalnim HTTPS/HTTP portovima koje ispiše konzola.  
5) **Prijava (primjeri):** korisnički podaci definirani su u `SeedUsers` sekciji `appsettings.json` (npr. `jhusic` / `Admin@123` za ulogu `Management`, `markomarkic` / `User@123` za ulogu `Referent`). 
## Testiranje
Pokreni sve jedinične testove iz root direktorija:
```bash
dotnet test
```

## Napomene
- Za ponovno punjenje baze izmijeni ili isprazni postojeću bazu pa pokreni `dotnet run -- seed`.
- Logovi se spremaju u `Logs/log-*.txt`; ograničenje zahtjeva po korisniku/hostu postavljeno je na 100/min.
- Lokalizacija je zadana na `hr-HR`, a kolačići i HSTS su konfigurirani za rad preko HTTPS-a.
