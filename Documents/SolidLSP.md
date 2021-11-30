# Liskov Substitution Design Principle

TOPROCESS:

- [bron](https://lassala.net/2010/11/04/a-good-example-of-liskov-substitution-principle/)
- !!!!! [bron](http://www.tomdalling.com/blog/software-design/solid-class-design-the-liskov-substitution-principle/)

> Subtypes moeten vervangbaar zijn door hun super types (parent class).
>
> de IS-A relatie zou vervangen moeten worden door IS-VERVANGBAAR DOOR

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
     void drive();
     void playRadio();
     void addLuggage();
}
```

Wat gebeurt er als we een Formule 1 auto hebben:

```csharp
public class FormulaOneCar: ICar 
{
    public void drive() 
    {
        //Code to make it go super fast
    }

    public void addLuggage() 
    {
        throw new NotSupportedException("No room to carry luggage, sorry."); 
    }

    public void playRadio() 
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
    if(b is Pinguin) {
        //Doe iets met de pinguin
    }
    else {
        //Doe iets anders
    }
}
```

