# WSL2

## Een nieuwe installatie "Run as administrator"

```powershell
wsl --install -d Ubuntu
```

- -d ubuntu: optioneel (default)
- Tip: installeer via Windows Store "Windows Terminal"

## Update van een bestaande installatie:

```powershell
wsl --update
```

Herstart bij een update WSL (opgelet, wsl herstarten betekent docker stoppen):

```powershell
wsl --shutdown
```

## Introductie

Vraag je je als DBA wel eens af of er een manier is om verschillende SQL Server versies/edities en op verschillende OS te testen zonder langdurige voorbereidingen en installaties? Die is er: Docker biedt een gemakkelijke manier.

Dit artikel beschrijft hoe je docker-containers op Windows 10 kan uitvoeren voor SQL Server 2019 op Linux. Het behandelt ook hoe je een gegevensdirectory kunt delen met de host voor databases en hoe u SQL Server Agent op containers kunt starten. Microsoft trok de ondersteuning voor SQL Server op Windows onder docker terug.

## Docker installeren op Windows 10

Download Docker Desktop voor Windows van https://www.docker.com/docker-windows. Volg gewoon de instructies om de software te installeren. Eenmaal voltooid zouden we op de taakbalk het docker pictogram moeten zien (het kan in het verborgen pictogrammenvak staan).



![image-20210817152202880](C:\Users\u2389\source\repos\Databanken2021\Images\docker5)

![image-20210817152308475](C:\Users\u2389\source\repos\Databanken2021\Images\docker6)

![image-20210817152348201](C:\Users\u2389\source\repos\Databanken2021\Images\docker7)

![image-20210817152548459](C:\Users\u2389\source\repos\Databanken2021\Images\docker8)

![image-20210817152615633](C:\Users\u2389\source\repos\Databanken2021\Images\docker9)

![image-20210817152721350](C:\Users\u2389\source\repos\Databanken2021\Images\docker10)

![image-20210817152759525](C:\Users\u2389\source\repos\Databanken2021\Images\docker11)

Na het starten van Docker Desktop kan je bepalen hoe en wanneer Docker Desktop voortaan actief is op je systeem:

![image-20210817152938889](C:\Users\u2389\source\repos\Databanken2021\Images\docker12)

![img](C:\Users\u2389\source\repos\Databanken2021\Images\docker1)

Docker draait standaard in Linux container modus. Als je een Windows container wilt draaien, schakel dan over naar Windows containers modus door rechts te klikken op het docker icoon; dit is echter niet nodig voor SQLServer 2019.

![img](C:\Users\u2389\source\repos\Databanken2021\Images\docker2)



Klik op het docker icoon, en het docker venster verschijnt. We kunnen de huidige beschikbare images, draaiende containers, enz. controleren. Ook kunnen we het gebruiken om containers te starten/stoppen/verwijderen, enz. We zullen de docker GUI niet behandelen. In dit artikel zullen we voornamelijk commandoregels gebruiken om containers te besturen.

## Voor Linux containers

Docker desktop draait standaard in Linux-containermodus.

### Download SQL Server Linux images

Open een cmd venster, voer het volgende uit om de SQL Server 2019 image te downloaden van Microsoft docker hub.

```plain
docker pull mcr.microsoft.com/mssql/server:2019-latest
```

### Draai de docker image voor SQL Server 2019

Laten we poort 1436 mappen op de host voor SQL Server 2019. We zullen ook enterprise editie specificeren met de "MSSQL_PID=Enterprise" optie. Zonder deze instelling draait een container standaard developer edition.

Een paar caveats. Ten eerste, zorg ervoor dat u een complex wachtwoord instelt, anders zult u later niet in staat zijn om verbinding te maken omdat de container niet zal starten. Ten tweede, zorg ervoor dat je dubbele aanhalingstekens ("") gebruikt voor parameters, vooral voor SA_PASSWORD. Ik heb gemerkt, voor Linux, dat de container binnen enkele seconden afbreekt als ik enkel aanhalingsteken ('') of geen aanhalingstekens gebruik.

Hier is het commando om een container te starten en vervolgens de connectie te testen (je kan bijvoorbeeld "exit" intikken bij de prompt). Let op: je geeft hier best een eigen paswoord op in plaats van 1Secure*Password1:

```plain
docker run --name sql_2019_1436 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=1Secure*Password1" -e "MSSQL_PID=Enterprise" -p 1436:1433 -d mcr.microsoft.com/mssql/server:2019-latest
docker exec -it sql_2019_1436 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa
```

![image-20210817153131995](C:\Users\u2389\source\repos\Databanken2021\Images\docker13)

Als dit is uitgevoerd, kunnen we "docker ps -a" uitvoeren om een lijst van containers te zien. We kunnen zien dat zowel sql_2017 als sql_2019_1436 draaien.

## Verbinding maken met SQL Server instance via SSMS

![img](C:\Users\u2389\source\repos\Databanken2021\Images\docker3)

![image-20210817153703552](C:\Users\u2389\source\repos\Databanken2021\Images\docker14)

Laten we de SQL Server versie controleren:

![image-20210817153812986](C:\Users\u2389\source\repos\Databanken2021\Images\docker15)

De docker container stoppen en verwijderen kan via volgende commandolijn-opdrachten:

```plain
docker stop sql_2019_1436
docker rm sql_2019_1436
```

# Installeer MySQL server onder WSL2

Om dit te laten werken, moet Windows 10 versie 1903 zijn bij gebruik van een Intel x64-processor of versie 2004 bij gebruik van een ARM64-processor. Om dit te controleren, klikt u op *Instellingen*: *Over*.

```
docker pull mysql
```

Met MySQL standaard poort:

```
docker run --name ms -p 3306:3306 -e MYSQL_ROOT_PASSWORD=password mysql
```

Installeer workbench onder Windows 10

https://dev.mysql.com/downloads/workbench/

![image-20210817161634419](C:\Users\u2389\source\repos\Databanken2021\Images\docker16)

"Other downloads", "No thanks", installeer.

Indien er nog geen "local" instantie herkend wordt, maak er een aan: klik op "+" en geef de connectie een naam. Druk op "test" knop en geef je paswoord in. Bewaar de nieuwe connectieinformatie.

Gebruik Docker Desktop om SQLServer en MySQL te starten.

# OPTIONEEL: NIET INSTALLEREN

Voorwaarden voor Linux GUI apps:

- Windows 10 Insider Preview build 21362+
- Driver voor vGPU, bijvoorbeeld https://developer.nvidia.com/cuda/wsl.

## Update je packages in je distributie (installeert gnome desktop)

```sh
sudo apt update
sudo apt upgrade
sudo apt install gedit -y
sudo apt install gimp -y
sudo apt install nautilus -y
sudo apt install vlc -y
sudo apt install x11-apps -y
```

Test: xcalc, xclock, xeyes

## Google Chrome

```sh
cd /tmp
sudo wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
sudo dpkg -i google-chrome-stable_current_amd64.deb
sudo apt install --fix-broken -y
sudo dpkg -i google-chrome-stable_current_amd64.deb
```

Test: google-chrome

## Microsoft Teams for Linux

```sh
cd /tmp
sudo curl -L -o "./teams.deb" "https://teams.microsoft.com/downloads/desktopurl?env=production&plat=linux&arch=x64&download=true&linuxArchiveType=deb"
sudo apt install ./teams.deb -y
```

Test: teams



