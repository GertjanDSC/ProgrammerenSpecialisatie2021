# ADO .NET Bulk upload

## Inleiding

We maakten reeds beknopt kennis met het zogenaamde "three-tier" (3-lagen) model bij het werken met een database.

Deze drie lagen zijn de volgende:

- **Presentatielaag**: wat de eindgebruiker ziet (vb. Console, GUI, ...)
- **Business** laag: je "business" logica, wat je met je objecten doet; in essentie alles wat niet op de andere twee lagen zit.
- **Data** laag: per business class een tegenhanger die een object van de class wegschrijft naar of opleest van de database of aanpast in de database, verwijdert uit de database of kijkt of het object reeds in de database zit.

De *data laag* wordt ook wel eens afgekort DAL genoemd. De *business laag* wordt ook wel eens afgekort tot *BLL* of *BAL* of wordt "Domain" genoemd.

Deze manier van omgaan met de database **(DAL) is interessant en leuk vanuit applicatiestandpunt**:

- alle database code bevindt zich op **slechts een duidelijke laag** en zodoende wordt het bijvoorbeeld makkelijk deze laag om te werken tot opslag in een ander type database (bijvoorbeeld MySql in plaats van SqlServer). Zeker wanneer we deze laag in een afzonderlijk project onderbrengen en dus beschikbaar stellen als een aparte assembly, is dit het geval.
- het patroon per class herhaalt zich steeds en in principe is het makkelijk mogelijk om vanuit een beschrijving van enkel de data members van een class (hetzij in een tekstformaat, bijvoorbeeld xml of json, hetzij in bijvoorbeeld C# code) deze **DAL laag volledig te genereren** zodat je hiervoor nauwelijks nog code moet schrijven.

Er zijn echter ook **nadelen** verbonden aan het exclusief gebruiken van deze laag in bepaalde contexten. Een van die contexten is het opladen in de database van heel veel soortgelijke objecten (*bulk upload*):

- vaak wordt **telkens opnieuw per object een connectie** naar de databank aangelegd en terug gesloten in plaats van deze te gebruiken tot alle objecten opgeladen zijn; dit kan afhankelijk van het type database kostelijk zijn in termen van performatie en tijd.
- de insert of update **query wordt telkens opnieuw parsed** door de database: ook dit kan kostelijk zijn.
- er wordt **niet gewerkt met een overkoepelende transactie**. Bij sommige types database verbetert het openen van een transactie bij het begin van het opladen en het pas sluiten van de transactie na het opladen van het laatste object de performantie. Dit is echter niet steeds het geval. We zullen nog zien dat het soms niet mogelijk is met een transactie te werken bij sommige database servers en soms kan het zijn dat een hele grote openstaande transactie andere performantieproblemen kan veroorzaken (dit kan bijvoorbeeld het geval zijn bij SqlServer).

Voor de *bulk upload* problematiek wordt vaak per database server een specifieke oplossing voorzien die veel performanter is dan individueel rij per rij opladen; met bijvoorbeeld MySql moet dit heel anders gebeuren dan met SqlServer. We gaan hier later verder op in.

Opladen van veel gegevens naar de database moet slechts eenmalig gebeuren, vaak bij het begin van het project. Daarom is het perfect aanvaardbaar om hiervoor een specifiek stuk code uit te werken naast de DAL-laag, die meer gebruikt wordt voor **ORM** (*Object-Relational Mapping*: enkel wanneer nodig een beperkt aantal objecten wegschrijven naar of ophalen uit de database).

Een bijkomende bedenking: bij programmeren komt het er vaak op neer om een *trade-off* te maken tussen **geheugen** en **cpu**; dit principe komt erop neer dat als cpu het probleem is, we deze best ontlasten door meer geheugen te gebruiken en omgekeerd. Dit is een balanceringsoefening. Zo ook bij opladen grote sets objecten: vaak is het veel gunstiger om eerst uit een bestand alles in het geheugen op te laden en vervolgens deze objecten in het geheugen via een aan de database eigen mechanisme weg te schrijven naar de database.

Wat verder uitermate belangrijk is bij het wegschrijven van grote sets gegevens naar de database: zorg ervoor dat dit geen *round trips* vereist of bijvoorbeeld een subquery die een zoekopdracht uitvoert per object dat opgeslagen moeten worden. Dit is nefast voor de performantie: het komt er dan in het eerste geval op neer dat de database meermaals betrokken wordt per object, de communicatie met de database vertragend werkt en de database vaak alweer met een andere context bezig is en dus niet optimaal kan functioneren of, in het laatste geval, per op te laden object telkens de hele tabel moet afzoeken. Het is dan veel beter om primary keys zelf in de applicatie te beheren.

## SQLServer

Dependency **System.Data.Client** (indien nodig toevoegen met nuget, bijvoorbeeld voor .NET Core applicaties) bevat specifiek voor het opladen van grote hoeveelheden gelijkaardige gegevens naar de database de namespace **SqlBulkCopy**.

Een dergelijke *SqlBulkCopy* moet gebeuren in twee fasen:

- haal de brongegevens in het geheugen in een objectstructuur. Dit kan een **collection** zijn van instanties van eigen classes of je kan gebruiken maken van class **DataTable** of **DataReader**. 
- schrijf de objecten in het geheugen weg naar de database.

Het in bulk opladen van gegevens naar de database gaat heel snel bij SqlServer met *SqlBulkCopy*: we zullen zien dat dit op het eerste zicht nog veel sneller lijkt te gebeuren dan bij MySql (ongeveer twee keer zo snel). Alleszins verloopt dit veel sneller dan bij het een voor een opladen van de rijen, wat veel tijd en performantie kost. Het bulk mechanisme verwerkt alle gegevens in een enkele keer.

We geven een voorbeeld.

Stel, we hebben een class **Gemeente** met Properties:

````Csharp
        public string GemeenteNaam { get; set; }
        public int NIScode { get; set; }
````

en een tabel **gemeente**:

````Csharp
CREATE TABLE [dbo].[gemeente](
	[Id] [int] NOT NULL,
	[gemeentenaam] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_GEMEENTE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
````

waarbij *_gemeentes* een *Dictionary<int, Gemeente>* is, dan kunnen we slechts eenmaal een connectie openen naar SqlServer, een instantie aanmaken van class **SqlBulkCopy**, vervolgens een geschikte instantie aanmaken van class **DataTable** met de juiste kolommen (gebruik voor het gemak dezelfde namen als de kolomnamen van de tabel in de database), vervolgens je instanties van class gemeente overbrengen naar de instantie van class *DataTable*, je instantie van class *SqlBulkCopy* vertellen met welke kolommen in de database tabel de kolommen van de instantie van class *DataTable* overeenkomen en tot slot de eigenlijke transfer uitvoeren met method **WriteToServer** van class *SqlBulkCopy*:

````Csharp
// Stap 1: stel een gepaste DataTable samen: types moeten overeenkomen met de types van de kolommen van je databanktabel

var table = new DataTable(); // mag maar moet niet een naam hebben
table.Columns.Add("Id", typeof(int));
table.Columns.Add("gemeentenaam", typeof(string));
foreach (var gemeente in _gemeentes.Values)
{
  table.Rows.Add(gemeente.NIScode, gemeente.GemeenteNaam);
}

// Stap 2: bulk upload van DataTable naar de overeenkomstige databanktabel
using (SqlConnection connection = new SqlConnection(ConnStr))
{
  connection.Open();
  // Transaction not allowed!
  using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
  {
    bulkCopy.DestinationTableName = "gemeente";
    bulkCopy.ColumnMappings.Add("Id", "Id"); // welke kolomnaam in de datatable komt overeen met welke kolomnaam in de databanktabel
    bulkCopy.ColumnMappings.Add("gemeentenaam", "gemeentenaam");
    bulkCopy.WriteToServer(table);
  }
}
````

Dit is eigenlijk een **heel eenvoudig stuk code dat relatief supersnel uitvoert** (van een uur naar een dikke minuut vergeleken met het individueel opladen van rijen). Een nadeel is wel dat je **alle gegevens uit tekstbestanden in het geheugen** hebt, maar dat objectmodel kan gebruikt worden om meteen vragen te beantwoorden en wanneer we tabel per tabel de gegevens nog eens in een DataTable steken, kost dit even extra geheugen, maar zo vlug deze gegevens opgeladen zijn, wordt dit geheugen terug vrijgegeven. Alleszins zal de oplossing zich stabieler gedragen dan bij het individueel opladen van rijen!

