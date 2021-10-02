# Entity Framework - Oefening 1

We zullen aan de hand van een voorbeeld een introductie geven over Entity Framework (EF) , waarbij we gebruik maken van Visual Studio 2019 en SQLServer 2019.

## Project

We starten met het aanmaken van een nieuw project EFtutorial van het type Console App (.NET 5 of .NET 6). Eenmaal dit project aangemaakt, installeren we de packages die nodig zijn om EF te gaan gebruiken. We selecteren ‘Dependencies’ in het project, en via de rechtermuistoets kunnen we een menu laten verschijnen met daarin ‘Manage NuGet Packages’.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image003.gif) |

Een venster ‘NuGet Package Manager’ opent zich en na het selecteren van ‘Browse’ (links boven) gaan we op zoek naar het package ‘Microsoft.EntityFrameworkCore.SqlServer’ dat we dan ook installeren.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image006.gif) |

Er verschijnen een aantal vensters die je informeren over de aanpassingen die zullen plaatsvinden en een vraag om aan de licentie-voorwaarden te voldoen.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image009.gif) |

![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image012.gif)

 Daarna installeren we nog een tweede package, namelijk Microsoft.EntityFrameworkCore.Tools, dat we zullen gebruiken voor de aanmaak/update van de databank-tabellen.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image015.gif) |

Wanneer beide packages zijn geïnstalleerd zijn deze ook te zien als ‘Dependencies’ in de submap ‘Packages’.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image018.gif) |

## Data model

 In deze tweede stap gaan we het datamodel aanmaken, we opteren namelijk voor een ‘Code First’- aanpak. De klassen die we gaan gebruiken groeperen we in een aparte folder ‘Model’. Een nieuwe folder kan worden aangemaakt door met de rechtermuistoets te klikken op het project en dan via de menus ‘Add’ en ‘New Folder’.

![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image021.gif)

 

De eerste klasse die we aanmaken is de klasse Auteur. Belangrijk hierbij is dat we een property ‘Id’ aanmaken, deze zal later door EF automatisch worden gebruikt als ‘Primary Key’.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image024.gif) |

Een tweede klasse is Uitgeverij en ook hier voorzien we een Id property.

![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image027.gif)![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image030.gif)

Onze derde klasse is Boek en deze klasse verwijst naar een uitgeverij en bevat ook een lijst van auteurs. Belangrijk bij deze klasse is de constructor, voor EF is het noodzakelijk dat er een constructor is die geen verwijzingen heeft naar andere klassen, vandaar dat we de eerste eenvoudige constructor toevoegen die enkel een titel en beschrijving als parameters heeft.

Het resultaat is dan een folder met onze drie klassen.



![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image033.gif)

## Databank migration 

In de derde stap gaan we nu onze klassen laten vertalen naar een databank-model. We beginnen met het toevoegen van een connectie naar de databank in het Server Explorer venster.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image036.gif) |

Voor het toevoegen van een connectie hebben we een ‘data source’ nodig, dit verwijst naar onze databank-server (analoog als bij ADO.NET). We kiezen ook voor een databank, in dit geval werd er reeds een databank boekDB aangemaakt, die we nu selecteren.

![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image039.gif)

 

Eénmaal de connectie naar de databank gelegd kunnen we ook de connectiestring opvragen bij de properties die bij de connection horen.

De belangrijkste klasse bij het gebruik van EF is de context klasse die we overerven van DbContext. In deze klasse geven we aan welke objecten/tabellen we gebruiken door middel van DbSets. Zo verwijst DbSet<Boek> naar de boeken in de databank. Belangrijk is om zeker niet de getter en setters te vergeten, die zijn noodzakelijk om straks de tabellen aan te maken in de databank. Naast de sets met de objecten hebben we ook een functie OnConfiguration() waar we kunnen instellen met welke databank we gaan werken, in dit geval SQL Server die via de meegegeven connectiestring is beschreven.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image042.gif) |

 We kunnen nu door gebruik te maken van de Entity Framework Tools de datatabellen automatisch laten aanmaken. Dit doen we door in het venster Package Manager Console het commando Add- Migration uit te voeren. Indien het venster niet zou open zijn, dan kan je dat doen door in het menu ’View’ te kiezen voor ’Other windows’ en daarin het venster te selecteren.

|      |                                                              |
| ---- | ------------------------------------------------------------ |
|      | ![img](file:///C:/Users/u2389/AppData/Local/Temp/msohtmlclip1/01/clip_image045.gif) |