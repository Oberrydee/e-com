Comment lancer le projet: 

CONFIGURATION DE LA CONNEXION A LA BDD
Le projet dispose d'une configuration pointant vers une base de donnée PstgreSQL. 


Methode 1 (recommandée): utiliser le script de lancement d'une BDD locale au projet (se positionner à la racine du projet et lancer la commande ".\scripts\start-dev-db.ps1" dans un terminal powershell)
    -> lancer ".\scripts\stop-dev-db.ps1" pour l'arrêter

Methode 2: 
- Installer PostgreSQL sur la machine, sélectioner "C:\Program Files\" comme destination lors de l'installation. (les scripts supposent l'emplacement C:\Program Files\PostgreSQL\17\bin)
- Créer la BDD localement avec les paramètres dans appsettings.json
        Details pour la methode 2 avec un outil comme DBeaver:
        - Installer PostgreSQL localement et verifier que le service PostgreSQL est demarre.
        - Ouvrir le fichier `ECommerce.API/appsettings.json` et relever la chaine de connexion dans `ConnectionStrings:DefaultConnection`.
        - Avec la configuration actuelle du projet, les valeurs sont:
        `Host=localhost`
        `Port=5433`
        `Database=ECommerceDb`
        `Username=postgres`
        `Password=postgres`
        - Dans DBeaver:
        `Database` > `New Database Connection`
        choisir `PostgreSQL`
        renseigner:
        `Host`: `localhost`
        `Port`: `5433`
        `Database`: `postgres` pour se connecter au serveur une premiere fois, ou `ECommerceDb` si elle existe deja
        `Username`: `postgres`
        `Password`: `postgres`
        - Tester la connexion avec `Test Connection` puis valider.
        - Si la base `ECommerceDb` n'existe pas encore, creer une nouvelle base via DBeaver:
        clic droit sur la connexion > `Create` > `Database`
        nom de la base: `ECommerceDb`
        - Une fois la base creee, verifier que `appsettings.json` pointe bien vers les memes valeurs.
        - Au lancement de l'API (`dotnet run` dans `ECommerce.API`), les migrations EF Core sont appliquees automatiquement et les tables sont creees dans la base configuree.

LANCEMENT DU SWAGGER UI
Dans le dossier "ECommerce.API", lancer dotnet run dans un terminal powershell

ADRESSE du swagger: "https://localhost:5282/swagger/index.html" 

