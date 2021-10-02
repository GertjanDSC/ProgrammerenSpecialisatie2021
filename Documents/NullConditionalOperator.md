# Null Conditional Operator 

## Inleiding

Met de Null Conditional Operator kan je op een eenvoudige manier controleren of een variabele ingesteld is op null. Wanneer een property van een object gelezen moet worden mag dat object niet gelijk zijn aan null. Hier wordt vaak op gecontroleerd met behulp van een if structuur om een NullReferenceException te voorkomen. Dit levert soms lange code op, bijvoorbeeld: 

```c#
foreach (Book book in Book.GetAll()) 
{ 
    string title = null, sequelTitle = null; 
    int? pages = null; 
    if(book != null) 
    { 
        title = book.Title; 
        pages = book.Pages; 
        if (book.Sequel != null) 
            sequelTitle = book.Sequel.Title; 
    } 
    bookInfos.Add(string.Format("Title: {0}, Pages: {1}, Sequel: {2}", title, pages, sequelTitle)); 
} 
```

## Gebruik van de Null Conditional Operator 

De Null Conditional operator helpt ons om op een beknoptere manier te controleren op null. In de voorbeeldapplicatie wordt daar gebruik van gemaakt: 

```c#
foreach (Book book in Book.GetAll()) 
{ 
    string title = book?.Title; 
    int? pages = book?.Pages; 
    string sequelTitle = book?.Sequel?.Title; 
    bookInfos.Add(string.Format("Title: {0}, Pages: {1}, Sequel: {2}", title, pages, sequelTitle)); 
} 
```

De voorwaardelijke null operator is een enkel vraagteken (het ? symbool) dat geplaatst wordt na de variabele die op null moet worden gecontroleerd. Wanneer p niet gelijk is aan null, dan wordt de Book.Title property uitgelezen en toegekend aan de lokale variabele title. Als p wél gelijk is aan null, dan wordt de Book.Title property niet uitgelezen om een NullReferenceException te vermijden. De lokale variabele title wordt in dat geval op null ingesteld. Opgelet: een ? symbool na een Type is geen Null Conditional Operator maar betekent dat je van dat Type een Nullable type maakt. In de bovenstaande code zie je dat we het type int (dat nooit null kan zijn) op null wensen te kunnen instellen door gebruik te maken van de int? declaratie.

## Chaining van de Null Conditional Operator 

De operator kan toegepast worden op een ketting van opeenvolgende Properties om zo doorheen de hiërarchie van object te navigeren, zonder dat er if structuren aan te pas komen. Dat zie je op de volgende regel code: 

```c#
string sequelTitle = book?.Sequel?.Title; 
```

Er wordt eerst gecontroleerd indien p gelijk is aan null. Zo ja, dan wordt sequelTitle ook null. Zo neen, dan wordt er gecontroleerd of de property Book.Sequel gelijk is aan null. Is dat het geval, dan wordt sequelTitle ook null. Is dat niet zo, dan krijgt sequelTitle de waarde van Book.Sequel.Title. Hiermee voorkom je een geneste if structuur. 

## Combinatie van Null Coalescing en Null Conditional Operator 

De Null Coalescing Operator en de Null Conditional Operator zijn twee verschillende operatoren die we gerust samen kunnen gebruiken. Omdat het resultaat van de Null Conditional Operator ook null kan zijn, is het mogelijk om alsnog een “fallback” waarde toe te kennen met behulp van de Null Coalescing operator. 

```c#
 foreach (Book book in Book.GetAll()) 
 { 
     string title = book?.Title ?? "[untitled book]"; 
     int? pages = book?.Pages ?? 0; 
     string sequelTitle = book?.Sequel?.Title ?? "[no sequels]"; 
     bookInfos.Add(string.Format("Title: {0}, Pages: {1}, Sequel: {2}", title, pages, sequelTitle)); 
 }
```

