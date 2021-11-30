## Single Responsibility Principle

> Een klasse heeft slechts 1 bestaansreden en kan maar 1 reden hebben om te veranderen

Eigenschappen van SRP zijn:

- *coupling*
- *cohesion*

> ***Cohesion***: compacte, coherente samenhang. Lage cohesie betekent dat een klasse zich bezig houdt met heel uiteenlopende taken, en niet gefocust is op de stricte, zuivere taak die zou moeten gebeuren. Hoge cohesie betekent dat een klasse doet wat er moet gebeuren, niet meer en niet minder. Probeer er voor te zorgen dat alle methoden in een klasse betrekking hebben tot een enkel doel, maw er een hoge cohesie heerst.
>
> ***Coupling***: de beperking van de mate waarin een klasse van nog andere klassen afhangt. Hoeveel weet een klasse over de werking (*internals*) van een andere klasse?

**Men streeft naar "low coupling" en "high cohesion"**.

### Waarop letten?

Klassen mogen maar een beperkt aantal instantievariabelen hebben. De methoden van deze klasse moeten één of meerdere van deze variabelen manipuleren.

### Wat bedoelen we met verantwoordelijkheid?

> Een reden tot verandering!

### Een voorbeeld van cohesie

Een klasse met hoge cohesie:

```csharp
class EmailMessage
{
    private string _sendTo;
    private string _subject;
    private string _message;
    
    public EmailMessage(string to, string subject, string message)
    {
        this._sendTo = to;
        this._subject = subject;
        this._message = message;
    }
    public void SendMessage()
    {
        // send message using _sendTo, _subject and _message
    }
}
```

Een voorbeeld van lage cohesie :

```csharp
class EmailMessage
{
    private string _sendTo;
    private string _subject;
    private string _message;
    private string _username;
    
    public EmailMessage(string to, string subject, string message)
    {
        this._sendTo = to;
        this._subject = subject;
        this._message = message;
    }
    
    public void SendMessage()
    {
        // send message using sendTo, subject and message
    }
    
    public void Login(string username, string password)
    {
        this._username = _username;
        // code to login
    }
}
```

### Lage Cohesie

![lage cohesie](https://timdams.gitbooks.io/csharpfromantwerp/content/assets/20_se/LowCohesion.PNG)

### Hoge Cohesie

![hoge cohesie](https://timdams.gitbooks.io/csharpfromantwerp/content/assets/20_se/HighCohesion.PNG)

De Login methode and username klasse variabele heeft niets te maken met het hoofddoel van de EmailMessage klasse. Daarom zeggen we dat er een lagere cohesie is dan optimaal.

## Een voorbeeld van high coupling

Eens de batterij kapot is, moet je een nieuwe iPad kopen, want de batterij is gesoldeerd in het apparaat en kan dus niet vervangen worden. Bij lage koppeling (loosly coupled) zou je de batterij kunnen vervangen. Ditzelfde gaat op in software engineering.

Een voorbeeld van high coupling:

```csharp
public class A
{
    public string ElementA;

    public string MethodA()
    {
        if(!string.IsNullOrEmpty(ElementA))
            return new B().ElementB;
    }
    
    public string MethodC()
    {
        return new B().MethodB();
    }
}

public class B
{
    public string ElementB;
    
    public string MethodB()
    {
        //..
    }
}
```

Waarom spreken we van *high coupling* of *tight coupling*? Klasse A instantiëert objecten van klasse B, en heeft toegang tot variabelen (elementB). Op deze manier is klasse A erg afhankelijk van klasse B. Waarom afhankelijk? Als we beslissen om een extra parameter toe te voegen in de constructor van B en de default constructor private te maken, dan moeten we elk gebruik van klasse B aanpassen (aanpassingen in klasse A).

## Wat is de oplossing?

We kunnen *tight coupling* oplossen door de afhankelijkheden om te keren: een extra laag wordt geïntroduceerd. We kunnen bijvoorbeeld een interface toevoegen: op deze manier zal klasseA enkel afhankelijk zijn van de interface en niet van de actuele implementatie van klasse B.

```csharp
public class A
{
    public string ElementA;
    
    private ISomeInteface _interface;

    public A(ISomeInterface i)
    {
        _interface = i;
    }

    public string MethodA()
    {
        if(!string.IsNullOrEmpty(ElementA))
            return _interface.ElementB;
    }

    public string MethodC()
    {
        return _interface.MethodB();
    }
}

public interface ISomeInterface
{
    string MethodB();
    string ElementB;
}
```

### SRP voorbeeld

```csharp
public class Werknemer
{
    Database _db;
    
    public Werknemer()
    {
        _db = new Database();
    }
    
    void Insert()
    {
        try {
            string sql = "insert into werknemers(voornaam,achternaam,stad) values ('Tom', 'Peeters', 'Antwerpen')";
            _db.query(sql);
        }
        catch(Exception e)
        {
            // Log error
            System.IO.File.WriteAllText(@"c:\Error.txt", e.ToString());
        }
    }

    void Delete()
    {

    }

    void Update()
    {

    }
}
```

De werknemer klasse is verantwoordelijk voor CRUD operaties, maar ook voor het loggen van errors: meer dan 1 verantwoordelijkheid. Indien we beslissen om niet meer naar een bestand te loggen, moeten we de klasse aanpassen.

Daarom is het beter om dit als volgt te coderen:

```csharp
public class Werknemer
{
    Database _db;
    FileLogger _logger;
    
    public Werknemer()
    {
        _db = new Database();
        _logger = new FileLogger();
    }
    void Insert()
    {
        try 
        {
            string sql = "insert into werknemers(voornaam,achternaam,stad) values ('Tom', 'Peeters', 'Antwerpen')";
            _db.query(sql);
        }
        catch(Exception e)
        {
            //Log error
            _logger.Log(e.ToString());
        }
    }  
}
```

De klasse `FileLogger`:

```csharp
public class FileLogger
{
    public void Log(string error)
    {
        System.IO.File.WriteAllText(@"c:\Error.txt", e.ToString());
    }
}
```

Met deze versie van de klasse `FileLogger` verhoog je de "coupling" graad, en voeg je met andere woorden best een extra laag toe, bijvoorbeeld een interface:

```csharp
public interface ILogger
{
    void Log(string error);
}

public class FileLogger:ILogger
{
}

public class Werknemer
{
    ILogger log;
    public Werknemer(ILogger _log)
    {
        log = _log
    }
}

static void Main(string[] args) 
{

    ILogger log = new FileLogger();
    Werknemer wn = new Werknemer(log);
}
```

Single responsibility speelt niet enkel op klasse-niveau maar ook op method-niveau.

### Single Responsibility op method niveau

#### Probleemstelling

Er is je gevraagd om software te schrijven voor een online video shop. Het programma berekent en print de rekening van een klant bij onze online shop. Onderstaande paragraaf geeft ons de voorbeeldcode van het programma. We zullen deze oplossing  analyseren en bekijken hoe we de code kunnen verbeteren. Aan het programma wordt meegegeven welke film de klant heeft gehuurd, en voor hoe lang. Daarna wordt de rekening gemaakt – afhankelijk van hoe lang de film gehuurd geweest is, en welk type film (nieuwe release, kinder, gewone). 

UML:

![movie architectuur](https://timdams.gitbooks.io/csharpfromantwerp/content/assets/20_se/moviearchitectuur.PNG)

### Voorbeeld van de MAIN functie

(altijd goed om je architectuur uit te testen door in je main een voorbeeld applicatie te laten draaien)

```csharp
static void Main(string[] args) 
{ 
    List<Customer> _list = new List<Customer>(); 

    Customer c = new Customer("Peeters"); 
    c.AddRental(new Rental(new Movie("Godfather", 0),3)); 
    _list.Add(c); 

    Customer c2 = new Customer("Vandeperre"); 
    c2.AddRental(new Rental(new Movie("Lion King", 2),2)); 
    _list.Add(c2); 


    Customer c3 = new Customer("Verlinden"); 
    c3.AddRental(new Rental(new Movie("Rundskop", 1),4)); 
    _list.Add(c3); 


    Customer c4 = new Customer("Dams"); 
    c4.AddRental(new Rental(new Movie("Top Gun", 0),1)); 
    _list.Add(c4); 

    foreach (Customer cust in _list) 
    { 
        Console.WriteLine( cust.Statement() ); 
    } 
}
```

### Movie klasse .. een simpele klasse

```csharp
public class Movie 
{ 
    public  const int CHILDRENS = 2; 
    public  const int REGULAR = 0; 
    public  const int NEW_RELEASE = 1;      

    public Movie(string title, int priceCode) 
    { 
        Title = title; 
        PriceCode = priceCode; 
    } 

    public int PriceCode { get; set; } 

    public string Title { get; set; } 
}
```

### Rental klasse

Deze klasse stelt voor hoe lang een klant een bepaalde film gehuurd heeft.

```csharp
public class Rental 
{ 
    private Movie _movie; 

    public Rental(Movie movie, int daysRented) 
    { 
        _movie = movie; 
        DaysRented = daysRented; 
    } 

    public int DaysRented { get; set; } 

    public Movie GetMovie() 
    { 
        return _movie; 
    } 
}
```

### Customer klasse

Deze klasse representeert de klant van de winkel:

```csharp
public class Customer
{

    List<Rental> _rentals = new List<Rental>();

    public Customer(string name)
    {
        Name = name;
    }

    public void AddRental(Rental arg)
    {
        _rentals.Add(arg);
    }

    public string Name { get; }

    public string Statement()
    {
        double totalAmount = 0;

        string result = "Rental Record for " + Name + "\n";

        foreach (Rental r in _rentals)
        {
            double thisAmount = 0;
            switch (r.GetMovie().PriceCode)
            {
                case Movie.REGULAR:
                    thisAmount += 2;
                    if (r.DaysRented > 2)
                    {
                        thisAmount += (r.DaysRented - 2) * 1.5;
                    }
                    break;

                case Movie.NEW_RELEASE:
                    thisAmount += r.DaysRented * 3;
                    break;

                case Movie.CHILDRENS:
                    thisAmount += 1.5;
                    if (r.DaysRented > 3)
                    {
                        thisAmount += (r.DaysRented - 3) * 1.2;
                    }
                    break;
            }

            //Show figures for this rental 
            result += "\t" + r.GetMovie().Title + "\t" + thisAmount.ToString() + "\n";
            totalAmount += thisAmount;
        }

        //Add footer lines 
        result += "Amount owned is " + totalAmount.ToString() + "\n";

        return result;

    }
}
```

## Analyse van onze architectuur

Voor een dergelijke (*simpele*) applicatie is design/architectuur niet zo belangrijk. We zien echter dat dit niet echt object georiënteerde code is, wat een invloed heeft op het gemak waarmee de toepassing kan uitgebreid en veranderd worden.

Enkele bemerkingen: de statement functie in onze Customer klasse is te lang en doet te veel. Veel zaken die we hier in doen, zouden naar andere klasses overgedragen moeten worden.

Ook al werkt ons programma (mooi geschreven code of lelijke code speelt echt geen rol voor een compiler), we moeten ons steeds het volgende afvragen: als in onze applicatie toevoegingen of veranderingen moeten aangebracht worden, moet er *iemand* zijn die dit kan klaar spelen, en een zwak gedesigned systeem is moeilijk te veranderen. Het vergt dan heel wat analysetijd van de programmeur om je programma te doorgronden.

Een voorbeeld van verandering: stel dat je klant vraagt om je rekening ook op een webpagina in HTML af te drukken. Welke impact heeft dit op je programma? Als we naar onze code kijken, merken we dat voor dergelijke vraagstelling het niet mogelijk is code te hergebruiken. Dus moeten we een nieuwe functie maken, die veel gedrag van de reeds bestaande statement functie kopieert. Op zich nog niet echt een probleem, want met wat copy-paste werk kan je de statement functie dupliceren en hernoemen naar htmlstatement() en de result string aanpassen met bijvoorbeeld: `result+=”<b>”blabla</b>”`.

Maar bedenk eens wat je allemaal moet doen als één regel in het rekening maken verandert? Je moet zowel aanpassingen maken in de statement als de htmlstatement functie, wat gegarandeerd fouten (bugs) zal introduceren!

Nog een andere opmerking. Als de winkel beslist om de classificatie (gewone film, kinder, nieuwe release) te veranderen, maar nog niet zeker is hoe, kan het zijn dat ze je vragen de mogelijke ideeën uit te testen. Dat heeft dan ook een invloed op hoe kosten voor films en huurpunten worden berekend. Als professionele software ontwikkelaar in spe ga ik je reeds verwittigen dat dergelijke veranderingen heel regelmatig voorkomen!

De `statement()` functie is de plaats waar de veranderingen in classificatie en berekeningen gebeuren. Dus ook niet te vergeten consistente veranderingen te maken in de `htmlstatement()` functie. Als de berekeningsmethodes steeds complexer worden, zal het met ons design ook steeds moeilijker worden om deze veranderingen door te voeren.

Wat nu volgt zijn voorstellen om onze software architectuur stap voor stap te veranderen totdat we object georiënteerde code hebben geschreven die ons in staat stelt dergelijke veranderingen op een *makkelijke* manier te realiseren.

### Analyseren van de statement functie

Tracht steeds korte functies/methodes te schrijven. Tracht lange functies onder te verdelen in kleinere delen. Kleinere stukken code zijn veel eenvoudiger te onderhouden! Om een functie te verdelen tracht je bij elkaar horende blokken te vinden. Een goede manier is om naar lokale scope variabelen te zoeken. Bijvoorbeeld thisAmount en Rental r, waarbij r niet wordt veranderd, terwijl thisAmount wel. Elke variabele die niet wordt veranderd, kunnen we als argument doorgeven. Indien er variabelen zijn die wel worden veranderd kunnen we, indien er maar 1 is, deze terug retourneren.

We zoeken in onze statement() functie naar deze lijnen code:

```csharp
switch( r.GetMovie().PriceCode ) 
{ 
    case Movie.REGULAR: 
        thisAmount += 2; 
        if (r.GetDaysRented() > 2) 
        { 
            thisAmount += (r.GetDaysRented() - 2) * 1.5; 
        } 
        break; 

    case Movie.NEW_RELEASE: 
        thisAmount += r.GetDaysRented() * 3; 
        break; 

    case Movie.CHILDRENS: 
        thisAmount += 1.5; 
        if (r.GetDaysRented() > 3) 
        { 
            thisAmount += (r.GetDaysRented() - 3) * 1.5; 
        } 
        break; 
}
```

En maken hiervoor een aparte functie:

```csharp
private double AmountFor(Rental r) 
{ 
    double thisAmount=0; 
    switch (r.GetMovie().GetPriceCode()) 
    { 
        case Movie.REGULAR: 
            thisAmount += 2; 
            if (r.GetDaysRented() > 2) 
            { 
                thisAmount += (r.GetDaysRented() - 2) * 1.5; 
            } 
            break; 

        case Movie.NEW_RELEASE: 
            thisAmount += r.GetDaysRented() * 3; 
            break; 

        case Movie.CHILDRENS: 
            thisAmount += 1.5; 
            if (r.GetDaysRented() > 3) 
            { 
                thisAmount += (r.GetDaysRented() - 3) * 1.5; 
            } 
            break; 
    } 
    return thisAmount; 
}
```

Terwijl we in de statement functie deze verandering maken:

```csharp
foreach (Rental r in _rentals) 
{ 
    double thisAmount = 0; 
    thisAmount = AmountFor(r); 
    //...
```

#### Analyse van AmountFor functie

Als we naar onze nieuwe AmountFor(Rental r) functie kijken, valt het op dat we hier met Rental data werken, en eigenlijk geen data van de customer klasse gebruiken. In de meeste gevallen moeten functies/methodes in die klasse staan vanwaar ze data gebruiken, dus in dit geval van de Rental klasse.

```csharp
public double GetCharge() 
{ 
    double result = 0; 
    switch (GetMovie().GetPriceCode()) 
    { 
        case Movie.REGULAR: 
            result += 2; 
            if (GetDaysRented() > 2) 
            { 
                result += (GetDaysRented() - 2) * 1.5; 
            } 
            break; 

        case Movie.NEW_RELEASE: 
            result += GetDaysRented() * 3; 
            break; 

        case Movie.CHILDRENS: 
            result += 1.5; 
            if (GetDaysRented() > 3) 
            { 
                result += (GetDaysRented() - 3) * 1; 
            } 
            break; 
    } 
    return result; 
}
```

Bij deze heb ik ook de naam van de functie veranderd in GetCharge(), omwille van de duidelijkheid. Tracht altijd naamgevingen te gebruiken die direct duidelijk maken wat je programmeert. Dus in de Customer klasse staat nu

```csharp
public string Statement() 
{ 
    double totalAmount = 0; 
    int frequentRenterPoints = 0; 

    string result = "Rental Record for " + GetName() + "\n"; 

    foreach (Rental r in _rentals) 
    { 
        double thisAmount = 0; 
        thisAmount += r.GetCharge(); 

    // etc.
```

Het klasse diagramma is nu veranderd naar:

![movie architectuur](https://timdams.gitbooks.io/csharpfromantwerp/content/assets/20_se/moviearchitectuur2.PNG)

Als we terug naar de statement() functie kijken dan is de variabele thisAmount redundant, en veranderen we naar:

```csharp
public string Statement() 
{ 
    double totalAmount = 0; 
    int frequentRenterPoints = 0; 

    string result = "Rental Record for " + GetName() + "\n"; 

    foreach (Rental r in _rentals) 
    { 


        //Show figures for this rental 
        result += "\t" + r.GetMovie().GetTitle() + "\t" + r.GetCharge().ToString() + "\n"; 
        totalAmount += r.GetCharge(); 
    } 

    //Add footer lines 
    result += "Amount owned is " + totalAmount.ToString() + "\n"; 

    return result; 
}
```

Best is om tijdelijke variabelen te verwijderen, omdat je makkelijk vergeet waarvoor ze dienen. Je zou in bovenstaand geval toch kunnen kiezen voor een temporary variabele thisAmount, omdat de getCharge() tweemaal wordt opgeroepen dus tweemaal een berekening maakt, als we dan naar performantie kijken.

In de Customer klasse:

```csharp
private double getTotalCharge() 
{ 
    double result = 0; 

    foreach (Rental r in _rentals) 
    { 
        result += r.GetCharge(); 
    } 

    return result; 
}
```

Met de statement functie als:

```csharp
public string Statement() 
{ 
    string result = "Rental Record for " + GetName() + "\n"; 

    foreach (Rental r in _rentals) 
    { 

        //Show figures for this rental 
        result += "\t" + r.GetMovie().GetTitle() + "\t" +  r.GetCharge().ToString() + "\n";                 
    } 

    //Add footer lines 
    result += "Amount owned is " +getTotalCharge().ToString() + "\n"; 
    result += "You earned " + getTotalFrequentRenterPoints().ToString() + "frequent renter points"; 

    return result; 
}
```

#### HTMLStatement() functie

In plaats van tekst te loggen wil ik mijn prijsberekening naar een HTML pagina schrijven. Dit is nu vrij simpel, en bij veranderingen in de prijsberekening moet ik de customer klasse niet meer aanpassen!

```csharp
public string HtmlStatement() 
{ 
    string result = "<h1>Rental Record for " + GetName() + "</h1>"; 

    foreach (Rental r in _rentals) 
    { 
        //Show figures for this rental 
        result += "<p>" + r.GetMovie().GetTitle() + " &nbsp; " + r r.GetCharge().ToString() + "</br></p"; 
    } 

    //Add footer lines 
    result += "<p>Amount owned is " + getTotalCharge().ToString() + "</br>"; 
    result += "You earned " + getTotalFrequentRenterPoints().ToString() + "frequent renter points</p>"; 

    return result; 
}
```

Bij een verandering aan de berekening, of toevoeging van nieuwe types films worden de statement functies niet meer gewijzigd, waardoor we duidelijk meer onderhoudvriendelijke code hebben geschreven.

```csharp
public double GetCharge() 
{ 
    double thisAmount = 0; 
    switch (GetMovie().GetPriceCode()) 
    { 
        case Movie.REGULAR: 
            thisAmount += 2; 
            if (GetDaysRented() > 2) 
            { 
                thisAmount += (GetDaysRented() - 2) * 1.5; 
            } 
            break; 

        case Movie.NEW_RELEASE: 
            thisAmount += GetDaysRented() * 3; 
            break; 

        case Movie.CHILDRENS: 
            thisAmount += 1.5; 
            if (GetDaysRented() > 3) 
            { 
                thisAmount += (GetDaysRented() - 3) * 1; 
            } 
            break; 
    } 
    return thisAmount; 
}
```

Het valt hier op dat we in de Rental klasse met een Movie object werken. Logischerwijze zou deze functie beter in de movie klasse staan. Het is een slecht idee om een switch te doen op een attribuut van een ander object!

We moeten dan wel het aantal huurdagen meegeven als parameter van deze nieuwe functie. Dus eigenlijk gebruikt deze functie 2 stukken data – type film, en aantal huurdagen. Waarom dan toch naar Movie klasse brengen, en daysRented meegeven als argument? Wel , de voorgestelde veranderingen gingen om type film (wat te doen als nieuw type wordt geïntroduceerd ), daarom is het logisch om de type informatie zo compact mogelijk te bundelen (in 1 functie ipv 2 functies (als je het type zou doorgeven als parameter)).

De Rental klasse:

```csharp
public double GetCharge() 
{ 
    return GetMovie().GetCharge(DaysRented);              
}
```

In de klasse Movie zit nu:

```csharp
public double GetCharge(int daysRented) 
{ 
    double result = 0; 
    switch (GetPriceCode()) 
    { 
        case Movie.REGULAR: 
            result += 2; 
            if (daysRented > 2) 
            { 
                result += (daysRented - 2) * 1.5; 
            } 
            break; 

        case Movie.NEW_RELEASE: 
            result += daysRented * 3; 
            break; 

        case Movie.CHILDRENS: 
            result += 1.5; 
            if (daysRented > 3) 
            { 
                result += (daysRented - 3) * 1; 
            } 
            break; 
    } 
    return result; 
}
```

## SRP, the "Law of Demeter"

Dit is het principe van "least knowledge", is een object-oriented software design principe. Een methode van een object mag enkel wie oproepen:

- het object zelf
- een argument van de methode
- elk object dat in de methode aangemaakt is
- alle properties en variabelen van het object zelf