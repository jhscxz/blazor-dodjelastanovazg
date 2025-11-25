# Dodjela stanova po socioekonomskom statusu

<img src="./grb-zg-og.png" align="left" alt="Grad Zagreb" width="60">

Blazor Server aplikacija koja digitalizira natječaje za dodjelu gradskih stanova: unos zahtjeva, automatsko bodovanje, pregled poretka i izvoz rezultata.

## Što nudi
- Upravljanje natječajima i rokovima; uloge `Management`, `Referent`, `ReferentReadOnly`.
- Unos/obrada zahtjeva s članovima kućanstva, prihodima i dokaznicama; status osnovan/nepotpun/neosnovan.
- Automatizirano bodovanje kriterija, filtriranje i pretraga; izvoz obrađenih zahtjeva u Excel.
- Audit i logovi (Serilog); SQL Server s temporalnim tablicama.

## Tehnologije
.NET 9, Blazor Server + MudBlazor, EF Core + Identity, Mapster, Serilog, OpenXML, xUnit/FluentAssertions/Moq.

## Arhitektura (sažetak)
- Blazor Server UI (MudBlazor) s područjima `Areas/Natjecaji` (unos/bodovanje) i `Areas/Admin` (upravljanje).
- Servisi u `Services` kapsuliraju domenu: bodovanje kriterija, export u Excel, audit, upravljanje lozinkama/breadcrumbs.
- EF Core kontekst u `Data` + migracije/seed; SQL Server s temporalnim tablicama za povijest.
- ASP.NET Core Identity za korisnike/uloge; autorizacija prema rolama (`Management`, `Referent`, `ReferentReadOnly`).
- Mapster za mapiranje DTO ↔ modeli; Serilog za logove u datoteke; OpenXML za izvoz.

## Brzi start
1) Preduvjeti: .NET 9 SDK, pristup SQL Serveru  
2) Postavi `ConnectionStrings:DefaultConnection` u `appsettings.json`.  
3) Migracije + seed podaci:  
   ```bash
   dotnet restore
   dotnet run -- seed
   ```  
4) Pokretanje:  
   ```bash
   dotnet run
   ```  
5) Primjeri računa su u `appsettings.json` pod `SeedUsers` (npr. `jhusic` / `Admin@123`). Promijeni prije produkcije.

## Testovi
```bash
dotnet test
```

## Napomena
Ponovno punjenje baze: isprazni bazu i pokreni `dotnet run -- seed`. Logovi su u `Logs/log-*.txt`.
