# Liskov Substitution Design Principle

> Subtypes moeten vervangbaar zijn door hun super types (parent class).

> A program that uses an interface must not be confused by an implementation of that interface.

> Functions that use pointers or references to base classes must be able to use objects of derived classes without knowing it.

Anders gesteld: de IS-A relatie zou vervangen moeten worden door IS-VERVANGBAAR-DOOR. Het Liskov principe is een manier om ervoor te zorgen dat overerving correct gebruikt wordt.

Als voorbeeld werken we met een klasse vierkant die overerft van Rechthoek. De klasse Rechthoek heeft eigenschappen als "Width" en "Height", en vierkant erft deze over. Maar als voor de klasse vierkant de breedte OF de hoogte gekend is, ken je de waarde van de andere ook: dit is tegen het principe van Liskov.

```csharp
public class Rechthoek
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int BerekenOpp()
    {
        return Width * Height;
    }
}
```

De klasse Vierkant erft over van Rechthoek (maar is in programmeren een vierkant wel een rechthoek). Een vierkant is een rechthoek met gelijke breedte en hoogte, en we kunnen de *properties* *virtual* maken in de klasse Rechthoek om dit te realiseren. Rare implementatie, niet? 

```csharp
public class Vierkant:Rechthoek
{
    public override int Width
    {
        get
        {
            return base.Width;
        }

        set
        {
            base.Width = value;
            base.Height = value;
        }
    }

    public override int Height
    {
        get
        {
            return base.Height;
        }

        set
        {
            base.Height = value;
            base.Width = value;
        }
    }
}
```

Client code:

```csharp
 static void Main(string[] args)
{
    Rechthoek r = new Vierkant();

    r.Width = 5;
    r.Height = 10;

    Console.WriteLine(r.BerekenOpp());
}
```

De gebruiker weet dat r een Rechthoek is dus is hij in de veronderstelling dat hij de breedte en hoogte kan aanpassen zoals in de *parent* klasse. Dit in acht genomen zal de gebruiker verrast zijn om 100 te zien ipv 50.

## Oplossen van het LSP probleem

- Code die niet **vervangbaar** is, zorgt ervoor dat polymorfisme niet werkt
- Client code (en dit geval de Main) veronderstelt dat basisklassen kunnen vervangen worden door hun afgeleide klassen (Rechthoek r = new Vierkant())
- Het oplossen van LSP door switch cases brengt een onderhoudsnachtmerrie met zich mee!

```csharp
public abstract class Shape
{
    public abstract int BerekenOpp();
}

public class Rechthoek : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }
    public override int BerekenOpp()
    {
        return Width * Height;
    }
}

public class Vierkant : Shape
{
    public int Side { get; set; }
    public override int BerekenOpp()
    {
        return Side * Side;
    }
}

public class OppBerekenaar
{
    public List<Shape> shapes;
    public int BerekenOppervlakte()
    {
        shapes = new List<Shape>();
        shapes.Add(new Vierkant() { Side = 10 });
        shapes.Add(new Rechthoek(){ Width = 5, Height= 20 });

        int total = 0;
        foreach(Shape s in shapes)
        {
            total += s.BerekenOpp();
        }

        return total;
    }
}
```

Een ander voorbeeld:

```csharp
public interface ICar 
{
     void Drive();
     void PlayRadio();
     void AddLuggage();
}
```

Wanneer we een Formule 1 wagen implementeren, ziet deze er ongeveer als volgt uit:

```csharp
public class FormulaOneCar: ICar 
{
    public void Drive() 
    {
        //Code to make it go super fast
    }

    public void AddLuggage() 
    {
        throw new NotSupportedException("No room to carry luggage, sorry."); 
    }

    public void PlayRadio() 
    {
        throw new NotSupportedException("Too heavy, none included."); 
    }
}
```

**De interface** dient als het contract, en moet je veronderstellen dat alle auto's dit gedrag hebben.

Dit is de essentie van het Liskov Substitution Principle.

### Waarom is het schenden van LSP niet goed?

Gebruik van abstracte klassen betekent dat je in de toekomst makkelijk een subklasse kan toevoegen in de werkende, geteste code. Dit is de essentie van het open closed principe. Maar wanneer je subklassen gebruikt die niet volledig de interface (abstracte klasse) supporteren moet je in de bestaande code speciale gevallen gaan definiëren.

Bijvoorbeeld:

```csharp
public void DoeIets(Bird b)
{
    if(b is Pinguin) 
    {
        //Doe iets met de pinguin
    }
    else 
    {
        //Doe iets anders
    }
}
```

Nog een voorbeeld om het Liskov principe goed te begrijpen. Stel, we willen vogels tekenen op een scherm, voor een game. Volgende klasse lijkt logisch:

```csharp
class Bird 
{
public:
    virtual void SetLocation(double longitude, double latitude) = 0;
    virtual void SetAltitude(double altitude) = 0;
    virtual void Draw() = 0;
};
```

De eerste versie van de game is een groot succes. Versie 2 voegt 12 vogeltypes toe en is een nog groter succes. In versie 3 wordt beslist penguins toe te voegen. Hierbij treedt echter een probleem op:

```csharp
void Penguin::SetAltitude(double altitude)
{
    //altitude can't be set because penguins can't fly
    //this function does nothing
}
```

Wanneer een methode niets anders kan doen dan een Exception opwerpen, schend je waarschijnlijk het Liskov principe!

Een aanlokkelijke oplossing, maar een foute, lijkt de volgende te zijn:

```csharp
//Solution 1: The wrong way to do it
void ArrangeBirdInPattern(Bird* aBird)
{
    Pengiun* aPenguin = dynamic_cast<Pengiun*>(aBird);
    if(aPenguin)
        ArrangeBirdOnGround(aPenguin);
    else
        ArrangeBirdInSky(aBird);
}
```

Zo creeer je eigenlijk een onderhoudsnachtmerrie: er kunnen talloze testen bijkomen.

Een betere oplossing is:

```csharp
//Solution 2: An OK way to do it
void ArrangeBirdInPattern(Bird* aBird)
{
    if(aBird->isFlightless())
        ArrangeBirdOnGround(aBird);
    else
        ArrangeBirdInSky(aBird);
}
```

Zelfs al lijkt de oplossing aanvaardbaar, toch is deze niet meer dan een sticker op de wonde.

Nog beter is het overerving behoorlijk te gebruiken:

```csharp
//Solution 3: Proper inheritance
class Bird 
{
public:
    virtual void Draw() = 0;
    virtual void SetLocation(double longitude, double latitude) = 0;
};

class FlighingBird : public Bird 
{
public:
    virtual void SetAltitude(double altitude) = 0;
};
```
