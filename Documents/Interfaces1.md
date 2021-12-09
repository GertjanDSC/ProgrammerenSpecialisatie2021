# Interfaces

In C# worden interfaces gebruikt om de *uitwendige verschijningsvorm* van een klasse te beschrijven: een interface kan bekeken worden als een *contract* dat je oplegt aan een klasse. Vaak zijn klassen grote, complexe stukken code bestaande uit tientallen tot honderden methoden en properties. Wanneer we een dergelijke klasse moeten gebruiken, boeit meestal enkel de uitwendige, of publieke, vorm: dit zijn de enige aspecten waarmee je als externe gebruiker van de klasse mee kunt praten. Interfaces zijn als het ware stempels die we op een klasse kunnen plakken om zo te zeggen "deze klasse gebruikt interface xyz". Gebruikers van de klasse hoeven dan niet de hele klasse uit te spitten en weten dat alle klassen met interface xyz dezelfde publieke properties en methoden hebben.

**Merk op dat dit niets met grafische user interfaces te maken heeft in dit geval.**

Een interface is niet meer dan een *belofte*: het zegt enkel welke publieke methoden en properties de klassen bezit. Het zegt echter niets over de effectieve code/implementatie van deze methoden en properties. Daarom spreken we ook van het opleggen van een *contract*.

Volgende code toont hoe we een interface definiëren:

```csharp
interface ISuperHeld
{
    void SchietLasers();
    int VerlaagKracht(bool isZwak);
    int Power{ get;set; }
}
```

Enkele opmerkingen:

- Het woord `class` wordt niet gebruikt, in de plaats daarvan gebruiken we `interface`.
- Het is een goede gewoonte om interfaces met een I te laten starten in hun naamgeving
- Methoden en properties gaan niet vooraf van `public`: interfaces zijn van nature net publiek, dus alle methoden en properties van de interface zijn dat bijgevolg ook.
- Er wordt geen code/implementatie gegeven: iedere methode eindigt ogenblikkelijk met een puntkomma.

Het is in de klassen dat we nu vervolgens verplicht zijn deze methode en properties te implementeren.

> **Een interface is een beschrijving hoe een component een andere component kan gebruiken, zonder te zeggen hoe dit moet gebeuren. De interface is met andere woorden 100% scheiding tussen de methode/Property-signatuur en de eigenlijke implementatie ervan.**

## Interface regels

- Je kan geen membervariabelen (fields) declareren in een interface (dat hoort bij de implementatie)
- Je kan geen constructor declareren
- Je kan geen access specificeren (public, protected, etc): alles is public
- Je kan nieuwe types (bv enum, struct) in een interface declareren.
- Een interface kan niet overerven van een klasse, wel van een andere interface.

## Interfaces en klassen

We kunnen nu aan klassen de stempel ISuperHeld geven zodat programmeurs weten dat die klasse gegarandeerd de methoden `SchietLasers`, `VerlaagKracht` en de property `Power` zal hebben.

Volgende code toont dit:

```csharp
class Zorro: ISuperHeld
{
    public void RoepPaard() { }
    public bool HeeftSnor{ get; set; }
    public void SchietLasers() //interface ISuperHeld
    {
        Console.WriteLine("pewpew");
    }
    int VerlaagKracht(bool isZwak)//interface ISuperHeld
    {
        if(isZwak) return 5;
        return 10;
    }
    int Power{get;set;} //interface ISuperHeld
}
```

Zolang de klasse Zorro niet exact de interface inhoud implementeert zal deze klasse niet gecompileerd kunnen worden.

## Interface notatie

![Interface uml notatie](./interface001.jpg)

## Is keyword met interfaces

We kunnen`is`gebruiken om te weten of een klasse een specifieke interface heeft.

Stel:

```csharp
interface IDeletable { };

class Book: IDeletable { };

class Person { };
```

In actie:

```csharp
Person luke = new Person();
Book gameofthrones = new Book();

if(gameofthrones is IDeletable)
{
    Console.WriteLine("I can delete game of thrones");
}
if(luke is IDeletable)
{
    Console.WriteLine("I can delete luke");
}
```

Output: `I can delete game of thrones`.

## Meerder interfaces

Een nadeel van overerving is dat een klasse maar van 1 klasse kan overerven. Een klasse mag echter wel meerdere interfaces met zich meedragen:

```csharp
interface ISuperHeld { }
interface ICoureur { } 
class Man { }

class Zorro: Man, ISuperHeld
{ }

class Batman:Man, ISuperHeld, ICoureur 
{ }
```

Ook mogen interfaces van elkaar overerven:

```csharp
interface IGod:ISuperHeld
{ }
```

# Why should I care?

![img](./pg012.jfif)

In kleine projecten lijken interfaces wat overkill, en dat zijn ze vaak wel. Van zodra je een iets complexer project krijgt met meerdere klasse die onderling met elkaar allerlei zaken moeten doen, dan zijn interfaces je dikke vrienden! Je hebt misschien al over de [SOLID programmeerprincipes gehoord](https://en.wikipedia.org/wiki/SOLID)?

And if not, niet erg. Samengevat zegt SOLID dat we een bepaalde hoeveelheid abstractie inbouwen enerzijds (zodat we niet de gore details van klassen moeten kennen om er mee te programmeren) anderzijds dat er een zogenaamde 'separation of concerns' (SoC) moet zijn (ieder deel/klasse/module van je code heeft een specifieke opdracht).

Met interfaces kunnen we volgens SOLID programmeren: het boeit ons niet meer wat er in de klasse zit, we kunnen gewoon aan de interfaces van een klasse zien wat hij kan doen. Handig toch!

Ok, als de helft van bovenstaande zinnen je wat filosofisch overkwamen, don't worry maar geloof ons: **als je tegen interfaces kan programmeren i.p.v. klasse dan zal je code vaak een pak beter worden in de long run**.

Toch nog een voorbeeld? Beeld je in dat je een complexe klasse `DiskWriter` hebt die je programma gebruikt om data van en naar de harde schijf te schrijven. De klasse implementeert een interface `IData` die twee methoden heeft (`ReadData()` en `WriteData`). Als je later beslist om je data naar een online server te schrijven en niet naar de harde schijf, dan kan je gewoon die klasse schrijven (bv. `InternetWriter`) en vervolgens ook de `IData` interface laten implementeren. Al je andere code moet dan niet aangepast worden! Ze (je andere klassen) kunnen gewoon blijven zeggen `ReadData` en `WriteData` en weten misschien zelfs niet dat hun data niet meer naar de HD maar naar het internet wordt gestuurd. 

# Voorbeeld: Presidenten en interfaces

In het hoofdstuk Polymorfisme bespraken we een voorbeeld van een klasse `President` die enkele `Advisor`-klassen gebruikt om hem te helpen.

Een nadeel van die voorgaande aanpak is dat al onze Advisors maar 1 "job" kunnen hebben: ze erven allemaal over van `Advisor` en kunnen nergens anders van overerven (multiple inheritance is niet toegestaan in C#). Via interfaces kunnen we dit oplossen. Een advisor gaan we dan eerder als een "bij-job" beschouwen en niet de hoofdreden van een klasse.

We definiëren daarom eerst een nieuwe interface `IAdvisor`:

```csharp
interface IAdvisor
{
    void Advise();
}
```

Vanaf nu kan eender *wie* die deze interface implementeert de President advies geven. Hoera! En daarnaast kan die klasse echter ook nog tal van andere zaken doen. Beeld je bijvoorbeeld een CEO van een bedrijf in die ook adviseur van de President wilt zijn. De bestaande klasse is bijvoorbeeld:

```csharp
class MicrosoftCEO: CEO   // CEO kan een parentklasse zijn die elders bijvoorbeeld algemene CEO-concepten beschrijft
{
    public void EarnBigBucks()
    { 
       Console.WriteLine("I'm getting rich!!!");       
    }
    public void FireDepartement()
     { 
       Console.WriteLine("You're all fired!");       
    }
}
```

Nu we de interface `IAdvisor` hebben, kunnen we deze klasse aanvullen met deze interface:

```csharp
class MicrosoftCEO: CEO, IAdvisor
{
    public void Advise()
    { 
        Console.WriteLine("I think you should allow our monopoly. *Grin*");
    }

    public void EarnBigBucks()
    { 
       Console.WriteLine("I'm getting rich!!!");       
    }
    
    public void FireDepartement()
    { 
       Console.WriteLine("You're all fired!");       
    }
}
```

De CEO kan dus z'n bestaande job blijven uitoefenen maar ook als adviseur optreden.

Ook de `President` moet aangepast worden om nu met een lijst van `IAdvisor` ipv `Advisor` te werken:

```csharp
public class MisterPresident
{
    public void RunTheCountry()
    {
        List<IAdvisor> allMinisters = new List<IAdvisor>();
        allMinisters.Add(new MicrosoftCEO);
        //Ask advise from each:
        foreach(IAdvisor minister in allMinisters)
        {
            minister.Advise();
        }
    }
}
```

De eerder beschreven `MilitaryAdvisor`,`ScienceAdvisor` en `EconomyAdvisor` dienen ook niet meer van de abstracte klasse `Advisor`(deze zou je kunnen verwijderen) over te erven en kunnen gewoon de interface implementeren:

```csharp
class MilitaryAdvisor:IAdvisor
{
    public void Advise()
    {
        IncreaseTroopNumbers();
        ImproveSecurity();
        PayContractors();
    }

    private void IncreaseTroopNumbers() 
    {
      //...
    }
    private void ImproveSecurity() 
    {
       //...
    }
    private void PayContractors() 
    {
      //...
 }
}
```

