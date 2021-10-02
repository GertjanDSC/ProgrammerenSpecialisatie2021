# Switch

Een `switch` statement is een program-flow element om een veelvoorkomende constructie van if else ... else ... elementen eenvoudiger te tonen. Vaak komt het voor dat we bijvoorbeeld aan de gebruiker vragen om een keuze te maken, bijvoorbeeld kies een getal van 1 tot 10, waarbij ieder getal een ander menu-item uitvoert van het programma:

```csharp
int option;

Console.WriteLine("Kies 1 voor afbreken, 2 voor opslaan, 3 voor laden:");

option = Convert.ToInt32(Console.ReadLine());

if (option == 1)
    Console.WriteLine("Afbreken gekozen");
else if (option == 2)
    Console.WriteLine("Opslaan gekozen");
else if (option == 3)
    Console.WriteLine("Laden gekozen");
else
    Console.WriteLine("Onbekende keuze");
```

Met een `switch` kan dit eenvoudiger. De syntax van een `switch` is een beetje specialer dan de andere programma flow-elementen (`if`, `while`, enzovoort), namelijk als volgt:

```csharp
switch (value)
{
      case constant:
           statements
           break;
      case constant:
           statements
           break;
      default:
           statements
           break;
  }
```

`value` is de waarde of variabele (beide mogen) die wordt gebruikt als test in de switch. Iedere case begint met het `case` keyword gevolgd door de waarde die value moet hebben om in deze case te *springen*. Na het dubbelpunt volgt vervolgens de code die moet uitgevoerd worden in deze `case`. De `case` zelf mag eender welke code bevatten (methoden, nieuwe program flow elementen, etc.), maar moet zeker afgesloten worden met het `break` keyword.

Tijdens de uitvoer zal het programma `value` vergelijken met iedere case constant van boven naar onder. Wanneer een gelijkheid wordt gevonden dan wordt die case uitgevoerd. Indien geen case wordt gevonden die gelijk is aan value dan zal de code binnen de `default`-case uitgevoerd worden (de `else` achteraan indien alle vorige `if else` tests negatief waren).

Het menu van zonet kunnen we herschrijven naar een `switch`:

```csharp
int option;
Console.WriteLine("Kies 1 voor afbreken, 2 voor opslaan, 3 voor laden:");
option = Convert.ToInt32(Console.ReadLine());
switch (option)
{
    case 1:
        Console.WriteLine("Afbreken gekozen");
        break;
    case 2:
        Console.WriteLine("Opslaan gekozen");
        break;
    case 3:
        Console.WriteLine("Laden gekozen");
        break;
    default:
        Console.WriteLine("Onbekende keuze");
        break;
  }
```

## Opgelet:

De case waarden moeten constanten zijn en mogen dus geen variabelen zijn. Constanten zijn de welgekende *literals* (`1`, `"1"`, `1.0`, `1.d`, `'1'`, enzovoort)

# Fall through

Soms wil je dat dezelfde code uitgevoerd wordt bij 2 of meer cases. Je kan ook zogenaamde fall through cases beschrijven:

```csharp
switch (option)
{
    case 1:
        Console.WriteLine("Afbreken gekozen");
        break;
    case 2:
    case 3:
        Console.WriteLine("Laden of opslaan gekozen");
        break;
    default:
        Console.WriteLine("Onbekende keuze");
        break;
  }
```

In dit geval zullen zowel de waarden `2` en `3` resulteren in de zin "Laden of opslaan gekozen" op het scherm.

`switch` is dus een selectie-instructie waarmee één switch-sectie wordt gekozen die moet worden uitgevoerd vanuit een lijst met kandidaten op basis van een patroon dat overeenkomt met de match-expressie. Een switch-instructie bevat een of meer switch-secties. Elke switch-sectie bevat een of meer case labels (een case- of standaardlabel), gevolgd door een of meer instructies. De switch-instructie kan maximaal één standaardlabel bevatten in een switch-sectie.

De switch wordt het beste gebruikt wanneer:

- je één waarde hebt die je wilt vergelijken met een groot aantal mogelijke waarden
- maar voor een overeenkomst moet je echter hooguit een paar coderegels uitvoeren

Het is mogelijk dat er meerdere cases van toepassing zijn, maar in C# worden de eerste overeenkomende expressie geselecteerd.

### Stap 1: maak de switch-instructie

Voeg de volgende code toe in de .NET-editor.

```csharp
int employeeLevel = 200;
string employeeName = "John Smith";

string title = "";

switch (employeeLevel)
{
    case 100:
        title = "Junior Associate";
        break;
    case 200:
        title = "Senior Associate";
        break;
    case 300:
        title = "Manager";
        break;
    case 400:
        title = "Senior Manager";
        break;
    default:
        title = "Associate";
        break;
}

Console.WriteLine($"{employeeName}, {title}");
```

Voer de code uit. De volgende uitvoer wordt weergegeven:

```output
John Smith, Senior Associate
```

Met het trefwoord `switch` wordt het doel van het onderstaande codeblok gedefinieerd. Naast het trefwoord staat de match-expressie tussen haakjes `(employeeLevel)`.

In het codeblok staan een of meer *switch-secties*. Elke switch-sectie heeft een of meer labels. Een label begint met het trefwoord `case` en een overeenkomend patroon. De runtime vergelijkt de waarde van de match-expressie met elk overeenkomende patroon totdat er een overeenkomst wordt gevonden.

Zodra de runtime een overeenkomend label heeft gevonden, wordt de code in die specifieke switch-sectie uitgevoerd.

Er kan slechts één switch-sectie worden uitgevoerd. Het trefwoord `break` is een van de verschillende methoden om een switch-sectie te beëindigen en letterlijk uit de switch-instructie te halen. Als je het trefwoord `break` (of optioneel het trefwoord `return`) vergeet, wordt er een fout gegenereerd door de compiler.

Als er geen overeenkomende labels zijn, wordt het optionele label `default` vergeleken. Als er geen `default` is gedefinieerd, wordt de opdracht `switch` alleen uitgevoerd als het hoofdlettergebruik overeenkomt.

Het optionele label `default` hoeft niet per se te worden gedefinieerd na de rest van de cases. De meeste ontwikkelaars kiezen er echter voor om deze achteraan te plaatsen, omdat het logisch is als standaardoptie of laatste optie.

### Stap 2: wijzig de waarde van de niveauvariabele om te zien hoe deze door de switch-instructie wordt geëvalueerd

Als je de standaardcase wilt oefenen, wijzigt je het niveau van de werknemer door de volgende coderegel te wijzigen.

```csharp
int employeeLevel = 201;
```

Wanneer je de code uitvoert, ziet je nu de meer algemene functietitel die wordt gebruikt.

```output
John Smith, Associate
```

Omdat de `employeeLevel` niet overeenkomt met eender welk label, wordt het `default`-label vergeleken.

### Stap 3: wijzig de toepassing om overgangen te gebruiken

Ons bedrijf heeft besloten om alle werknemers op niveau 100 de functietitel Seniormedewerker te geven. Dit is dezelfde functietitel als werknemers van het niveau 200. Als ontwikkelaar besluit je dit te implementeren door de eerste switch-sectie te verwijderen die deel uitmaakt van het label `case 100:`, en in plaats daarvan de labels `case 100:` en `case 200:` op dezelfde switch-sectie te laten uitvoeren.

Wijzig de code om de `employeeLevel` in te stellen op `100`:

```csharp
int employeeLevel = 100;
```

Bewerk vervolgens de code in

```csharp
    case 100:
    case 200:
        title = "Senior Associate";
        break;
```

Wanneer je klaar bent met het aanbrengen van wijzigingen, moeten je wijzigingen overeenkomen met de volgende code:

```csharp
int employeeLevel = 100;
string employeeName = "John Smith";

string title = "";

switch (employeeLevel)
{
    case 100:
    case 200:
        title = "Senior Associate";
        break;
    case 300:
        title = "Manager";
        break;
    case 400:
        title = "Senior Manager";
        break;
    default:
        title = "Associate";
        break;
}

Console.WriteLine($"{employeeName}, {title}");
```

Voer de toepassing nu uit om het volgende resultaat te krijgen:

```output
John Smith, Senior Associate
```

De beide case labels `100` en `200` zijn nu gekoppeld aan de switch-sectie waarmee de functietitel wordt ingesteld op de tekenreekswaarde `Senior Associate`.

## Samenvatting

Dit zijn de belangrijkste aandachtspunten die je hebt geleerd over de switch-instructie:

- Gebruik de instructie `switch` als je één waarde met veel mogelijke overeenkomsten hebt, waarbij elke overeenkomst een vertakking in uw codelogica vereist.
- Eén switch-sectie met codelogica kan worden vergeleken met een of meer labels die zijn gedefinieerd met het trefwoord `case`.
- Gebruik het optionele trefwoord `default` om een label en een switch-sectie te maken die moeten worden gebruikt wanneer er geen andere case labels overeenkomen.