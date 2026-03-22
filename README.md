# ECommerce API

Backend ASP.NET Core 8 avec PostgreSQL.

## Resume fonctionnel

Cette API est le coeur metier d'une boutique e-commerce.

Elle permet de :

- gerer l'authentification et les comptes utilisateurs
- exposer le catalogue produits avec leurs informations
- alimenter les parcours d'achat (panier et wishlist)
- recevoir et suivre les demandes de contact

Elle est consommee par le frontend Angular et centralise la logique metier, les regles d'acces et la persistance des donnees.

## Prerequis

Installez uniquement ceci :

1. Git for Windows
https://git-scm.com/download/win
2. .NET 8 SDK stable en x64
https://dotnet.microsoft.com/en-us/download/dotnet/8.0
3. PostgreSQL 17 en x64
https://www.postgresql.org/download/windows/
4. Visual Studio Code si vous voulez editer le projet
https://code.visualstudio.com/

Points importants :

- prendre le SDK .NET 8, pas seulement le Runtime
- prendre une version stable .NET 8, pas une preview 10 ou 11
- les scripts du projet attendent PostgreSQL 17 dans `C:\Program Files\PostgreSQL\17\bin`

## Verification rapide

Apres installation :

```powershell
git --version
dotnet --version
dotnet --list-sdks
dotnet --list-runtimes
Test-Path "C:\Program Files\PostgreSQL\17\bin\psql.exe"
```

Vous devez avoir :

- un SDK .NET 8.x
- Microsoft.NETCore.App 8.x
- Microsoft.AspNetCore.App 8.x
- `True` pour `psql.exe`

## Procedure

```powershell
git clone https://github.com/Oberrydee/e-com.git
cd e-com
Set-ExecutionPolicy -Scope Process Bypass
.\scripts\start-dev-db.ps1
dotnet restore
cd .\ECommerce.API
dotnet run
```

Puis ouvrir :

```text
https://localhost:5282/swagger/index.html
```

Le frontend correspondant est :

```text
https://github.com/Oberrydee/app.git
```

## Configuration utilisee

Connexion PostgreSQL par defaut :

```text
Host=localhost
Port=5433
Database=ECommerceDb
Username=postgres
Password=postgres
```

Le fichier correspondant est `ECommerce.API/appsettings.json`.

## Arret

Arreter l'API avec `Ctrl+C`, puis depuis la racine du projet :

```powershell
.\scripts\stop-dev-db.ps1
```

## Erreurs possibles

### `dotnet` n'est pas reconnu

Le SDK .NET 8 n'est pas installe ou le terminal n'a pas ete relance.

Correction : installer le SDK .NET 8, fermer PowerShell, rouvrir PowerShell, puis relancer :

```powershell
dotnet --version
```

### `No .NET SDKs were found`

Le SDK n'est pas installe correctement.

Correction : reinstaller le SDK .NET 8 stable, rouvrir PowerShell, puis verifier :

```powershell
dotnet --list-sdks
```

### `dotnet build` marche mais `dotnet run` dit `You must install or update .NET`

Le SDK est la, mais le runtime .NET 8 requis n'est pas disponible au bon niveau.

Correction : verifier :

```powershell
dotnet --list-runtimes
```

Vous devez voir au minimum :

- `Microsoft.NETCore.App 8.x`
- `Microsoft.AspNetCore.App 8.x`

Si vous avez seulement une version preview ou un patch trop ancien, reinstallez le dernier SDK .NET 8 stable.

### `PostgreSQL tool not found`

PostgreSQL 17 n'est pas installe au chemin attendu.

Correction 1 : installer PostgreSQL 17 dans :

```text
C:\Program Files\PostgreSQL\17
```

Correction 2 : ou lancer le script avec le bon chemin :

```powershell
.\scripts\start-dev-db.ps1 -PostgresBinPath "D:\PostgreSQL\17\bin"
```

### Le script PowerShell refuse de s'executer

Lancer :

```powershell
Set-ExecutionPolicy -Scope Process Bypass
```

### Echec de connexion a la base sur `localhost:5433`

PostgreSQL n'est pas demarre.

Correction :

```powershell
cd <racine-du-projet>
.\scripts\start-dev-db.ps1
cd .\ECommerce.API
dotnet run
```

### Warning sur le certificat HTTPS

Lancer :

```powershell
dotnet dev-certs https --trust
```

Puis relancer :

```powershell
dotnet run
```