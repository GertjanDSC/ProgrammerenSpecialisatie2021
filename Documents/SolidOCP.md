# Open-Closed Principe (OCP)

Het open/closed principe stelt dat klasses of functies open moeten zijn voor uitbreiding, maar gesloten voor wijziging!

> Open for extension, closed for modification

Gesloten voor wijziging betekent dat het gedrag mag veranderd worden zonder de broncode aan te passen..

Een typisch voorbeeld:

```csharp
public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }
}
```

Nu bouwen we een applicatie die de oppervlakte van een collectie rechthoeken zal berekenen.

```csharp
public class OppBerekenaar
{

    public double Opp(Rechthoek[] shapes)
    {
        double opp = 0;
        foreach (var shape in shapes)
        {
            opp += shape.Width * shape.Height;
        }

        return opp;
    }
}
```

En we schrijven ons testprogramma:

```csharp
static void Main(string[] args)
{

    Rechthoek rh1 = new Rechthoek() { Width = 49, Height = 30 };
    Rechthoek rh2 = new Rechthoek() { Width = 30, Height = 20 };
    Rechthoek rh3 = new Rechthoek() { Width = 22, Height = 10 };
    Rechthoek rh4 = new Rechthoek() { Width = 44, Height = 35 };

    Rechthoek[] rechthoeken = new Rechthoek[4];
    rechthoeken[0] = rh1;
    rechthoeken[1] = rh2;
    rechthoeken[2] = rh3;
    rechthoeken[3] = rh4;

    OppBerekenaar opb = new OppBerekenaar();
    double totaal = opb.Opp(rechthoeken);

    Console.WriteLine("totaal: "+ totaal);
}
```

De volgende vraag komt op: kunnen we het programma uitbreiden zodat we ook de oppervlakte van een cirkel kunnen berekenen?

We passsen de code als volgt aan:

```csharp
public double Opp(Object[] shapes)
{
    double opp = 0;
    foreach (var shape in shapes)
    {
        if(shape is Rechthoek)
            opp += ((Rechthoek)shape).Width * ((Rechthoek)shape).Height; //CAST to Rechthoek


        if (shape is Cirkel)
            opp += ((Cirkel)shape).Straal * ((Cirkel)shape).Straal * Math.PI;  //CAST to cirkel
    }

    return opp;
}
```

Wat later krijgen we de vraag om de OppBereken klasse uit te breiden zodat we ook de oppervlakte van driehoeken kunnen opnemen. Dit druist in tegen het principe "gesloten voor wijziging!"

## OPC oplossing

> Maak gebruik van abstractie.

In .NET betekent abstractie : gebruik maken van interfaces, of abstracte klassen.

Wat is een interface?

**Je hebt geleerd dat een klasse slechts van één klasse kan erven. Een klasse kan echter ook nog interfaces implementeren. Wanneer een klasse een interface implementeert sluit de klasse een** contract **met de compiler dat de klasse zich zal gedragen volgens de interface. Concreet betekent dit dat in de klasse alle eigenschappen (properties) en methoden van de interface moet implementeren. Een interface bevat dus eigenlijk enkel een lijst van eigenschappen en methoden die nog geen concrete invulling hebben.**

Volgens WIKIPEDIA: Een interface in de programmeertaal als Java of C# is een soort abstracte klasse die een interface aanduidt die klassen kunnen implementeren. Een interface wordt aangeduid met het sleutelwoord interface en bevat alleen ongedefinieerde methoden.

Wat is een abstracte klasse?

**In de informatica is een abstracte klasse een klasse die ongedefinieerde methoden kan bevatten. Deze methoden worden geïmplementeerd in een subklasse van de abstracte klasse. Het is niet mogelijk om een object te maken van abstracte klassen maar wel van niet-abstracte subklassen. Door middel van overerving is het wel mogelijk om de methoden die wel gedefinieerd zijn in de abstracte klasse te erven en in de subklassen te gebruiken.**

> Een klasse kan meerdere interfaces implementeren maar alleen van één klasse (rechtstreeks) overerven. Een verschil met abstracte klassen is dat een abstracte klasse wel gedefinieerde methoden kan bevatten maar een interface bevat alleen ongedefinieerde methoden.

Om aan het OPC principe te voldoen moeten we als volgt te werk gaan:

We maken een basis klasse voor rechthoeken, cirkels, driehoeken, andere vormen, en deze definieert een abstracte methode om de oppervlakte te berekenen.

```csharp
public abstract class Vorm
{
    public abstract double Oppervlakte();
}
```

De andere klassen leiden af van vorm:

```csharp
public class Rechthoek: Vorm
{
    public int Width { get; set; }
    public int Height { get; set; }

    public override double Oppervlakte()
    {
        return Width * Height;
    }
}
public class Cirkel:Vorm
{

    public int Straal { get; set; }

    public override double Oppervlakte()
    {
        return Straal * Straal * Math.PI;
    }
}
```

De berekening gebeurt nu als volgt:

```csharp
public class OppBerekenaar
{

    public double Opp(Vorm[] shapes)
    {
        double opp = 0;
        foreach (var shape in shapes)
        {
            opp += shape.Oppervlakte();
        }

        return opp;
    }

}
```

Op deze manier is de OppBerekenaar klasse gesloten voor wijziging, maar toch open voor uitbreiding!

#### In de praktijk

OPC zal je als ervaren programmeur sneller toepassen. Van bij de start van je ontwikkeling zal je niet altijd OPC toepassen, en accepteer dat een klasse veranderd moet worden. Maar bij nog verandering, zorg je ervoor dat je naar het OPC principe refactort.
