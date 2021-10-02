# Generics

Talen als C# bieden goede mogelijkheden voor strong typing. Dit houdt in dat de compiler tijdens het compileren weet van welk type een variabele is en daardoor welke methoden en properties daarvoor beschikbaar zijn. Dit is gewenst, omdat op deze wijze de compiler goed kan controleren of er geen methoden worden aangeroepen die niet bestaan voor een bepaald type. Hoe meer compile-time gecontroleerd kan worden, hoe minder risico er is op fouten (bv exceptions) die pas at run-time naar voren komen.

Run-time fouten zijn lastiger te detecteren, omdat deze alleen optreden op het moment dat de "foute" regel code uitgevoerd wordt en dus alleen door middel van testen en gebruik van het systeem boven water kunnen komen wat dus vaak resulteert in bugs. Via generieke klassen kunnen we aan strong typing doen en zo al een groot deel van potentiele problemen at compile-time opvangen. 

# Generieke methoden

Vaak schrijf je methoden die hetzelfde doen, maar waarvan enkel het type van de parameters en/of het returntype verschilt. Stel dat je een methode hebt die de elementen in een array onder elkaar toont. Je wil dit werkende hebben voor arays van ints, string, etc. Zonder generics moeten we dan per type een methode schrijven:

```csharp
public static void ToonArray(int[] array)
{
    foreach (var i in array)
    {
        Console.WriteLine(i);
    }
}

public static void ToonArray(string[] array)
{
    foreach (var i in array)
    {
        Console.WriteLine(i);
    }
}
```

Dankzij generics kunnen we nu het deel dat *generiek* moet zijn aanduiden (in dit geval met `T`) en onze methode eenmalig definiëren:

```csharp
public static void ToonArray<T>(T[] array)
{
    foreach (T i in array)
    {
        Console.WriteLine(i);
    }
}
```

# Generic types

In het volgende codevoorbeeld is te zien hoe een eigen generic struct in C# gedefinieerd en gebruikt kan worden. Merk het gebruik van de aanduiding T , deze geeft aan dat hier een type (zoals int, double, Student,etc.) zal worden ingevuld tijdens het compileren.

## 

De typeparameter T wordt pas voor de specifieke instantie van de generieke klass of type ingevuld bij het compileren. Hierdoor kan de compiler per instantie controleren of alle parameters en variabelen die in samenhang met het generieke type gebruikt worden wel kloppen.

De afspraak is om .NET een T te gebruiken indien het type nog dient bepaald te worden (dit is niet verplicht). [Meer info](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/introduction-to-generics)

## Voorbeeld generic type

We wensen een `struct` te maken die de locatie in X,Y,Z richting kan bewaren. We willen echter zowel `float`, `double` als `int` gebruiken om deze X,Y,Z coördinaten in bij te houden:

```csharp
struct Location<T>
{
    public T X;
    public T Y;
    public T Z;
}
```

We kunnen deze struct nu als volgt gebruiken:

```csharp
static void Main(string[] args)
{
    Location<int> plaats;
    plaats.X = 34;
    plaats.Y = 22;
    plaats.Z = 56;

    Location<double> plaats2;
    plaats2.X = 34.5;
    plaats2.Y = 22.2;
    plaats2.Z = 56.7;

    Location<string> plaats3;
    plaats3.X = "naast de kerk";
    plaats3.X = "links van de bakker";
    plaats3.Z = "onder het hotel";

}
```

# Why should I care?

![img](./pg012.jfif)

Generics zijn een zeer krachtig concept van C#. De eerste maanden zal je vooral bestaande generieke klassen gebruiken die de .NET programmeurs reeds voor je hebben geprogrammeerd. Dankzij generics konden zij klassen schrijven die veel polyvalenter zijn omdat ze niet gebonden zijn aan een bepaald type. *Heb ik die klassen al gebruikt?* Wie weet. Maar als je straks de [generieke collections ontdekt, dan zal je direct verkocht zijn. Nog enkele hoofdstukken geduld dus!

# Klassen en generics

Niets houdt ons nu tegen om generieke klassen te maken. We kunnen bijvoorbeeld volgende klasse maken die we kunnen gebruiken met eender welk type om de meetwaarde van een meting in op te slaan:

```csharp
public class Meting<T>
{
    private T waarde;
    //constructor
    public Meting(T waardein)
    {
        waarde = waardein;
    }
    //Public property
    public T Waarde { get { return waarde; } set { waarde = value ;} }
    public override string ToString()
    {
        return "Deze meting heeft als waarde:"+ waarde.ToString();
    }
}
```

Een voorbeeldgebruik van dit nieuwe type kan zijn:

```csharp
    Meting<int> m1 = new Meting<int>(44);
    Console.WriteLine(m1);

    Meting<string> m2 = new Meting<string>("slechte meting");
    Console.WriteLine(m2);
```

Of we zouden zelfs een extra klasse kunnen aanmaken genaamd Student

```csharp
public class Student
{
    public string Naam;
    public Student(string n){Naam= n;} //Constructor
    public override string  ToString()
    {
        return Naam;
    }
}
```

En dan als volgt een nieuwe meting met daarin een student aanmaken. Let goed op de constructor-aanroep van zowel Meting als Student!

```csharp
Meting<Student> m3= new Meting<Student>(new Student("Tim Dams"));
Console.WriteLine(m3);
```

De uitvoer van dit programma zou dan zijn: ![img](./pg026.png)

# Meerdere types in generics

Zoals reeds eerder vermeld is de T aanduiding enkel maar een afspraak. Je kan echter zoveel T-parameters meegeven als je wenst. Stel dat je bijvoorbeeld een klasse wenst te maken waarbij 2 verschillende types kunnen gebruikt worden. De klassedefinitie zou er dan als volgt uit zien:

```csharp
class DataBewaarder<Type1, Type2>
{
    private Type1 waarde1;
    private Type2 waarde2;
    //Constructor
    public DataBewaarder(Type1 w1, Type2 w2)
    {
        waarde1 = w1;
        waarde2 = w2;
    }
    public override string ToString()
    {
        return "Waarde1: " + waarde1 + "\nWaarde2: " + waarde2;
    }
}
```

Het gebruik ervan:

```csharp
DataBewaarder<int, string> d1 = new DataBewaarder<int, string>(4, "Ok");
```

Met als uitvoer: ![img](./pg027.png)

# Constraints

We willen vaak voorkomen dat bepaalde types wel of niet gebruikt kunnen worden in je zelfgemaakte generieke klasse. Stel bijvoorbeeld dat je een klasse schrijft waarbij je de CompareTo() methode wenst te gebruiken. Dit gaat enkel indien het type in kwestie de IComparable interface implementeert. We kunnen als constraint ("beperking") dan opgeven dat de volgende klasse enkel kan gebruikt worden door klassen die ook effectief die interface implementeren (en dus de CompareTo()-methoden hebben). Het geel deel geeft de constraint aan. We zeggen dus letterlijk ("waar T overerft van IComparable"):

```csharp
public class Wijziging<T> where T : IComparable
{
    public T vorigewaarde;
    public T huidigewaarde;
    public Wijziging(T vorig, T huidig)
    {
        vorigewaarde = vorig;
        huidigewaarde = huidig;
    }

    public bool IsGestegen()
    {
        if (huidigewaarde.CompareTo(vorigewaarde) > 0)
            return true;
        else return false;
    }
}
```

Volgende gebruik van deze klasse zou dan True op het scherm tonen:

```csharp
Wijziging<double> w = new Wijziging<double>(3.4, 3.65);
Console.WriteLine(w.IsGestegen());
```

## Mogelijke constraints

Verschillende zaken kunnen als constraint optreden. Naast de verplichting dat een bepaalde interface moet worden geïmplementeerd kunnen ook volgende constraints gelden([meer info](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)):

- Enkel value types
- Enkel klassen
- Moet default constructor hebben
- Moet overerven van een bepaalde klasse

# Why should I care?

![img](./pg012.jfif)

Goh, eerlijk is eerlijk: in je prille programmeursbestaan zal je niet heel vaak de nood zien om je eigen generieke klasse te schrijven. Laat staan dat je ook nog eens allerlei constraints gaat definiëren.

# Collections

## Nadelen van arrays

We hebben al eerder gezien hoe we arrays kunnen gebruiken om gegevens van hetzelfde type in één datastructuur voor te stellen. Hoewel arrays handig zijn, hebben ze ook een aantal nadelen:

- Je weet niet altijd van tevoren hoe groot de array zal worden, nochtans moeten we op voorhand ruimte reserveren door op te geven hoe groot de array moet zijn. Hierdoor kan het voorkomen dat we veel computergeheugen reserveren voor een array die mogelijk nooit volledig gevuld zal zijn.
- Omgekeerd kan het ook zijn dat we onze array te klein definiëren en we dus niet alle data kwijtraken in de array. We kunnen dit, omslachtig, oplossen door een nieuwe, grotere array te definiëren en daar de oude naar te kopiëren, inclusief te nieuwe data.
- Wanneer de array value-types bevat (int, double, struct, etc.) en deze array kopiëren naar een nieuwe array waar we vervolgens een waarde aanpassen, dan zal enkel de waarde in de gekopieerde array veranderen, niet in de originele. Indien we echter een array van objecten (referentietypes) maken en in een kopie een referentie aanpassen, dan zal deze ook aangepast worden in de originele array.

## Collecties: geavanceerde arrays

Collecties zijn speciale objecten waarin een reeks van andere objecten kan worden gerefereerd. Daar collecties zelf ook objecten zijn, heeft dit een paar gevolgen:

- Er moet eerst een instantie van een collectie aangemaakt worden voor je ze kan gebruiken (m.b.v. `new` keyword)
- Collecties hebben properties die onder andere het aantal objecten in collectie weergeeft.
- Collecties hebben methoden om onder andere objecten toe te voegen en verwijderen
- Daar collecties beschreven zijn in klassen kan je de typische object-georiënteerde eigenschappen hierop toepassen: je kan collecties overerven, encapsuleren (composiet), etc.

Er bestaan een hele resem collecties en je hebt er zelfs al van gebruikt, namelijk de List<>-klasse (weliswaar de generic variant, de niet-generieke versie heet ArrayList). Volgende tabel geeft een beschrijving van de meest gebruikte collecties die in de System.Collections.Namespace zitten [Bron MSDN](http://msdn.microsoft.com/en-us/library/system.collections.aspx)

| Klasse       | Beschrijving                                                 |
| ------------ | ------------------------------------------------------------ |
| `ArrayList`  | Standaard collectie die dynamisch kan groeien (zelfde als de List<> generieke variant). |
| `BitArray`   | Array die enkel Booleans (true/false) bevat. Handig om dus een bit-voorstelling te doen. |
| `HashTable`  | Ieder element bevat een waarde en een bijhorende sleutel als index. |
| `Queue`      | Stelt een first-in, first-out (FIFO) lijst voor.             |
| `SortedList` | Soortgelijk als HashTable, een sleutel stelt de index voor, elementen worden echter onmiddellijk gerangschikt . |
| `Stack`      | Stelt een Last-in, first-out (LIFO) lijst voor.              |

We zullen een aantal van de eerder vernoemde collecte-klassen verderop bekijken, maar we zullen onmiddellijk de generieke varianten bekijken omdat die veel handiger zijn. Het is namelijk zo dat generic collecties meestal bruikbaarder en veiliger zijn dan de gewone collecties die we zonet kort hebben beschreven. Generic collecties zijn ‘strongly types’ en bieden een betere veiligheid op gebied van ‘type safety’, hun gebruik is echter quasi identiek dan dat van de niet generieke collecties.

### Collection namespace

As je een collection-klasse wilt gebruiken, vergeet dan niet de `System.Collection` namespace toe te voegen. Indien je een generieke collection nodig hebt (zie verder) dan dien je de `System.Collections.Generic` namespace te gebruiken

### Type-safety

Een niet-generieke colletion is **niet type-safe** . Een generieke collection is dat wel.

Volgende 2 code-voorbeelden tonen dit.

In het niet-generieke geval zal deze code compileren maar **tijdens de uitvoer** zal de laatste lijn een fout (Exception) geven:

```csharp
ArrayList nietGeneriekeList=new ArrayList();
string naam= "Tim";
nietGeneriekeList.Add(naam);
int leeftijd = (int)nietGeneriekeList[0];
```

Bij een generieke collection zal er bij soortgelijk code een compiler-error optreden (gedrag dat meestal wenselijk is) en de code zal dus niet gecompileerd kunnen worden:

```csharp
List<string> generiekeList=new List<string>();
string naam= "Tim";
generiekeList.Add(naam);
int leeftijd = (int)nietGeneriekeList[0]; // compilererror: cannot convert type string to in
```

# Generic collections

Generic collecties zijn ‘strongly typed’ en bieden een betere veiligheid op gebied van ‘type safety’, hun gebruik is echter quasi identiek aan dat van de niet generieke collecties.

We hebben reeds 1 generic collectie-klasse veel gebruikt, namelijk de List<>-klasse, waarbij we tussen de haakjes het type opgaven dat de List kon bevatten , bv List of List. Met andere woorden, je hebt reeds vorig academiejaar generics gebruikt zonder het goed en wel te beseffen!

Een volledig overzicht van alle mogelijk generic collections vind je hier terug [MSDN](http://msdn.microsoft.com/en-us/library/system.collections.generic.aspx).

We beschrijven nu de werking van een aantal typische collecties, merk op dat deze werking quasi identiek is als die voor de niet-generische versie.

## List<> collectie

Deze werd reeds besproken.

## Queue<> collectie

Een queue (Ned. : rij) stelt een FIFO-lijst voor. Een queue stelt dus de rijen voor die we in het echte leven ook hebben wanneer we bijvoorbeeld aanschuiven aan een ticketverkoop. Met deze klasse kunnen we zo’n rij simuleren en ervoor zorgen dat steeds het ‘eerste/oudste’ element in de rij als eerste wordt behandeld. Nieuwe elementen worden achteraan de rij toegevoegd.

We gebruiken 2 methoden om met een queue te werken:

- `Enqueue(T item)`: Voeg een item achteraan de queue toe.
- `Dequeue()`: geeft het eerste element in de queue terug en verwijdert dit element vervolgens uit de queue.

Voorbeeld:

```csharp
Queue<string> theQueue =  new Queue<string>();
theQueue.Enqueue("I arrived here first!");
theQueue.Enqueue("I arrived second.");
theQueue.Enqueue("I'm last");

Console.WriteLine(theQueue.Dequeue());
Console.WriteLine(theQueue.Dequeue());
```

Dit zal op het scherm tonen:

```
I arrived here first!
I arrived here second.
```

#### Peek

Een andere interessante methode is de **Peek()** methode; hiermee kunnen we kijken in de queue wat het eerste element is, zonder het te verwijderen.

### Stack collectie

Daar waar een queue first-in-first-out is, is een stack last-in first out. M.a.w. het recentst toegevoegde element zal steeds vooraan staan en als eerste verwerkt worden. Je kan dit vergelijken met een stapel waar je steeds bovenop een nieuw object legt.

![img](pg028.png)

We gebruiken volgende 2 methoden om met een stack te werken:

- `Push(T item)`: plaats een nieuw item bovenop de stapel
- `Pop()`: geeft het bovenste element in de stack terug en verwijdert vervolgens dit element van de stack Voorbeeld:

```csharp
Stack<string> theStack =  new Stack<string>();
theStack.Push("I arrived here first!");
theStack.Push("I arrived second.");
theStack.Push("I'm last");

Console.WriteLine(theStack.Pop());
Console.WriteLine(theStack.Pop());
```

Dit zal dus het volgende resultaat geven:

```
I'm last
I arrived second.
```

## Dictionary collectie

In de niet generieke-collections heb je de HashTable, de generische versie is de dictionary. In een dictionary wordt ieder element voorgesteld door een index en de waarde van het element. De sleutel moet een unieke waarde zijn zodat het element kan opgevraagd worden uit de dictionary aan de hand van deze sleutel.

Bij de declaratie van de dictionary dien je op te geven wat het type van de key zal zijn , alsook het type van de waarde (value). In het volgende voorbeeld maken we een dictionary van klanten, iedere klant heeft een unieke ID (de key) alsook een naam (die niet noodzakelijk uniek is):

```
Dictionary<int, string> customers = new Dictionary<int, string>();
customers.Add(123, "Tim Dams");
customers.Add(6463, "James Bond");
customers.Add(666, "The beast");
customers.Add(700, "James Bond");
```

Merk op dat je niet verplicht bent om een int als key te gebruiken, dit mag eender wat zijn, zelfs een klasse.

We kunnen nu met behulp van een foreach-loop alle elementen tonen (merk op dat de foreach-constructie voor eender welke type collectie kan gebruikt worden om doorheen een array te lopen):

```csharp
foreach (var item in customers)
{
    Console.WriteLine(item.Key+ "\t:"+item.Value);
}
```

We kunnen echter ook een specifiek element opvragen aan de hand van de key. Stel dat we de waarde (naam) van de klant met key (id) gelijk aan 123 willen tonen:

```csharp
Console.WriteLine(customers[123]);
```

De key werkt dus net als de index bij gewone arrays, alleen heeft de key nu geen relatie meer met de positie van het element in de collectie.

### Eender welk type voor key en value

De key kan zelfs een string zijn en de waarde een ander type. In het volgende voorbeeld hebben we eerder een klasse Student aangemaakt. We maken nu een student aan en voegen deze toe aan de studentenLijst. Vervolgens willen we de leeftijd van een bepaalde student tonen op het scherm en vervolgens verwijderen we deze student:

```csharp
Dictionary<string, Student> studentenLijst = new Dictionary<string, Student>();
Student stud= new Student();
stud.Naam= "Tim";stud.Leeftijd=24;
studentenLijst.Add("AB12",stud);
Console.WriteLine(studentenLijst["AB12"].Leeftijd);
studentenLijst.Remove("AB12");
```

# Why should I care?

![img](pg012.jfif)

Als je die vraag moet stellen dan heb je niet veel begrepen van arrays en dit hoofdstuk. Arrays zijn fijn en krachtig, toch zal je vaak allerlei extra methoden beginnen schrijven om het werken met je array wat eenvoudiger te maken. Van zodra je code schrijft om sneller elementen aan arrays toe te voegen, verwijderen, verzetten, etc. dan wordt het tijd om te overwegen om een of andere generieke collection te gebruiken.