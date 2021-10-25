# Threading

## Inleiding en concepten

C# ondersteunt parallelle uitvoering van code door middel van multithreading. Een thread is een onafhankelijk uitvoeringstraject, dat gelijktijdig met andere threads kan lopen.

Een C# client programma (Console, WPF, of Windows Forms) start in een enkele thread die automatisch wordt aangemaakt door de CLR en het besturingssysteem (de "main" thread), en wordt multithreaded gemaakt door additionele threads aan te maken. Hier is een eenvoudig voorbeeld en zijn uitvoer:

```C#
// Alle voorbeelden gaan ervan uit dat de volgende namespaces opgenomen worden:
using System;
using System.Threading;
```

```c#
class ThreadTest
{
  static void Main()
  {
    Thread t = new Thread (WriteY);          // Kick off a new thread
    t.Start();                               // running WriteY()
 
    // Simultaneously, do something on the main thread.
    for (int i = 0; i < 1000; i++) Console.Write ("x");
  }
 
  static void WriteY()
  {
    for (int i = 0; i < 1000; i++) Console.Write ("y");
  }
}
```

![image-20211020101556993](./ThreadingImages/image-20211020101556993.png)

De hoofd-draad (main thread) creëert een nieuwe draad (thread) t waarop hij een methode uitvoert die herhaaldelijk het teken "y" afdrukt. Tegelijkertijd drukt de hoofddraad herhaaldelijk het teken "x" af:

![Starting a new Thread](./ThreadingImages/NewThread.png)

Eenmaal gestart, wordt de eigenschap **IsAlive** van een thread waar, tot het punt waarop de thread eindigt. Een thread eindigt wanneer de delegate die aan de constructor van de Thread is doorgegeven, klaar is met uitvoeren. Eenmaal beëindigd, kan een thread niet opnieuw starten.

De CLR wijst elke thread zijn eigen stack toe, zodat lokale variabelen gescheiden blijven. In het volgende voorbeeld definiëren we een methode met een lokale variabele en roepen we de methode tegelijkertijd aan op de main thread en een nieuw aangemaakte thread:

```c#
static void Main() 
{
  new Thread (Go).Start();      // Call Go() on a new thread
  Go();                         // Call Go() on the main thread
}
 
static void Go()
{
  // Declare and use a local variable - 'cycles'
  for (int cycles = 0; cycles < 5; cycles++) Console.Write ('?');
}
```

![image-20211020102050616](./ThreadingImages/TH_3.png)

Een aparte kopie van de variabele wordt aangemaakt op de geheugenstapel van elke thread, en dus is de output, voorspelbaar, tien vraagtekens.

Threads delen gegevens als ze een gemeenschappelijke referentie hebben naar dezelfde objectinstantie. Bijvoorbeeld:

```c#
class ThreadTest
{
  bool done;
 
  static void Main()
  {
    ThreadTest tt = new ThreadTest();   // Create a common instance
    new Thread (tt.Go).Start();
    tt.Go();
  }
 
  // Note that Go is now an instance method
  void Go() 
  {
     if (!done) { done = true; Console.WriteLine ("Done"); }
  }
}
```

Omdat beide threads Go() aanroepen op dezelfde ThreadTest instantie, delen ze het done veld. Dit resulteert in "Done" dat één keer wordt afgedrukt in plaats van twee keer:

![image-20211020102239266](./ThreadingImages/TH_4.png)

Statische velden bieden een andere manier om gegevens te delen tussen threads. Hier is hetzelfde voorbeeld met done als een statisch veld:

```c#
class ThreadTest 
{
  static bool done;    // Static fields are shared between all threads
 
  static void Main()
  {
    new Thread (Go).Start();
    Go();
  }
 
  static void Go()
  {
    if (!done) { done = true; Console.WriteLine ("Done"); }
  }
}
```

Beide voorbeelden illustreren een ander belangrijk concept: dat van thread safety (of liever, het gebrek eraan!) De uitvoer is eigenlijk onbepaald: het is mogelijk (hoewel onwaarschijnlijk) dat "Done" tweemaal wordt afgedrukt. Als we echter de volgorde van de statements in de Go-methode omwisselen, neemt de kans dat "Done" twee keer wordt afgedrukt dramatisch toe:

```c#
static void Go()
{
  if (!done) { Console.WriteLine ("Done"); done = true; }
}
```

![image-20211020102431273](./ThreadingImages/TH_5.png)

Het probleem is dat de ene thread de if-instructie kan evalueren op het moment dat de andere thread de WriteLine-instructie uitvoert - voordat deze de kans heeft gehad om done op true te zetten.

De oplossing is om een exclusief slot te verkrijgen tijdens het lezen en schrijven naar het gemeenschappelijke veld. C# biedt de lock-instructie voor precies dit doel:

```c#
class ThreadSafe 
{
  static bool done;
  static readonly object locker = new object();
 
  static void Main()
  {
    new Thread (Go).Start();
    Go();
  }
 
  static void Go()
  {
    lock (locker)
    {
      if (!done) { Console.WriteLine ("Done"); done = true; }
    }
  }
}
```

Wanneer twee threads gelijktijdig een lock (in dit geval, locker) betwisten, wacht één thread, of blokkeert, totdat de lock beschikbaar komt. In dit geval wordt ervoor gezorgd dat slechts één thread tegelijk de kritieke sectie van de code kan binnengaan, en dat "Done" slechts één keer wordt afgedrukt. Code die op een dergelijke manier beschermd is - tegen onbepaaldheid in een multithreading context - wordt thread-safe genoemd.

### NOTEN

- Gedeelde gegevens zijn de voornaamste oorzaak van complexiteit en onduidelijke fouten bij multithreading. Hoewel vaak essentieel, loont het om het zo eenvoudig mogelijk te houden.
- Een geblokkeerde thread verbruikt geen CPU.

## Join en Sleep

Je kunt wachten tot een andere thread is afgelopen door zijn Join methode aan te roepen. Bijvoorbeeld:

```C#
static void Main()
{
  Thread t = new Thread (Go);
  t.Start();
  t.Join();
  Console.WriteLine ("Thread t has ended!");
}
 
static void Go()
{
  for (int i = 0; i < 1000; i++) Console.Write ("y");
}
```

Dit drukt 1.000 keer "y" af, onmiddellijk gevolgd door "Thread t has ended!". Je kunt een timeout opgeven bij het aanroepen van Join, in milliseconden of als TimeSpan. Dit geeft dan true terug als de thread is afgelopen of false als de time-out is verstreken.

Thread.Sleep pauzeert de huidige thread gedurende een gespecificeerde periode:

```c#
Thread.Sleep (TimeSpan.FromHours (1));  // sleep for 1 hour
Thread.Sleep (500);                     // sleep for 500 milliseconds
```

Terwijl een thread wacht op een Sleep of Join, is hij geblokkeerd en verbruikt hij dus geen CPU-bronnen.

Thread.Sleep(0) geeft de huidige time slice van de thread onmiddellijk op en geeft vrijwillig de CPU aan andere threads. De nieuwe Thread.Yield() methode van Framework 4.0 doet hetzelfde - behalve dat het alleen afstand doet aan threads die op dezelfde processor draaien.

Sleep(0) of Yield is af en toe nuttig in productiecode voor geavanceerde prestatie-tweaks. Het is ook een uitstekend diagnostisch hulpmiddel om problemen met de veiligheid van threads aan het licht te brengen: als het invoegen van Thread.Yield() ergens in je code het programma maakt of breekt, heb je bijna zeker een bug.

## Hoe werkt threading?

Threads worden intern beheerd door een thread scheduler, een functie die de CLR normaliter delegeert aan het besturingssysteem. Een thread scheduler zorgt ervoor dat alle actieve threads de juiste uitvoeringstijd krijgen toegewezen, en dat threads die wachten of geblokkeerd worden (bijvoorbeeld op een exclusieve lock of op gebruikersinvoer) geen CPU-tijd verbruiken.

Op een computer met één processor voert een thread scheduler time-slicing uit - het snel wisselen van de uitvoering tussen elk van de actieve threads. Onder Windows is een time-slice typisch in de orde van tientallen milliseconden - veel groter dan de CPU-overhead bij het daadwerkelijk wisselen van context tussen de ene thread en de andere (die typisch in de orde van enkele microseconden ligt).

Op een computer met meerdere processoren wordt multithreading geïmplementeerd met een mengsel van time-slicing en echte concurrency, waarbij verschillende threads code gelijktijdig op verschillende CPU's uitvoeren. Het is vrijwel zeker dat er nog steeds sprake zal zijn van time-slicing, omdat het besturingssysteem zijn eigen threads - en die van andere toepassingen - moet bedienen.

Een thread wordt geacht te worden gepreempt wanneer de uitvoering ervan wordt onderbroken door een externe factor zoals time-slicing. In de meeste situaties heeft een thread geen controle over wanneer en waar hij wordt gepreempt.

## Threads versus processen

Een thread is analoog aan een proces in het besturingssysteem waarin uw applicatie draait. Net zoals processen parallel lopen op een computer, lopen threads parallel binnen een enkel proces. Processen zijn volledig van elkaar geïsoleerd; threads hebben slechts een beperkte mate van isolatie. In het bijzonder delen threads (heap) geheugen met andere threads die in dezelfde applicatie draaien. Dit is deels de reden waarom threading nuttig is: een thread kan bijvoorbeeld op de achtergrond gegevens ophalen, terwijl een andere thread de gegevens kan weergeven zodra ze binnenkomen.

## Gebruik en misbruik van threading

Multithreading kent vele toepassingen; dit zijn de meest voorkomende:

### Behoud van een responsieve gebruikersinterface

Door tijdrovende taken uit te voeren op een parallelle "worker" thread, is de hoofd UI thread vrij om door te gaan met het verwerken van toetsenbord- en muisgebeurtenissen.

### Efficiënt gebruik maken van een anders geblokkeerde CPU

Multithreading is nuttig wanneer een thread wacht op een reactie van een andere computer of hardware. Terwijl een thread geblokkeerd is tijdens het uitvoeren van de taak, kunnen andere threads gebruik maken van de anders onbelaste computer.

### Parallel programmeren

Code die intensieve berekeningen uitvoert, kan sneller worden uitgevoerd op multicore- of multiprocessorcomputers als de werklast wordt verdeeld over meerdere threads in een "verdeel-en-heers"-strategie (zie deel 5).

### Speculatieve uitvoering

Op multicore machines kun je soms de prestaties verbeteren door iets te voorspellen dat misschien gedaan moet worden, en het dan van tevoren te doen. LINQPad gebruikt deze techniek om het maken van nieuwe queries te versnellen. Een variatie is om een aantal verschillende algoritmes parallel uit te voeren die allemaal dezelfde taak oplossen. Degene die als eerste klaar is, "wint" - dit is effectief als je van tevoren niet kunt weten welk algoritme het snelst zal werken.

### Gelijktijdige verwerking van verzoeken mogelijk maken

Op een server kunnen client requests gelijktijdig binnenkomen en dus parallel afgehandeld moeten worden (het .NET Framework maakt hier automatisch threads voor aan als je ASP.NET, WCF, Web Services, of Remoting gebruikt). Dit kan ook nuttig zijn op een client (b.v. het afhandelen van peer-to-peer netwerken - of zelfs meerdere verzoeken van de gebruiker).
Met technologieën zoals ASP.NET en WCF, kan het zijn dat je niet eens weet dat er multithreading plaatsvindt - tenzij je toegang hebt tot gedeelde data (misschien via statische velden) zonder de juiste locking, waardoor je in overtreding bent met de thread safety.

Aan threads zijn ook voorwaarden verbonden. De grootste is dat multithreading de complexiteit kan verhogen. Het hebben van veel threads zorgt op zichzelf niet voor veel complexiteit; het is de interactie tussen threads (meestal via gedeelde data) die dat doet. Dit geldt ongeacht of de interactie opzettelijk is of niet, en kan leiden tot lange ontwikkelcycli en een voortdurende gevoeligheid voor intermitterende en niet-reproduceerbare bugs. Om deze reden loont het om interactie tot een minimum te beperken, en waar mogelijk vast te houden aan eenvoudige en bewezen ontwerpen. Dit artikel richt zich grotendeels op het omgaan met juist deze complexiteiten; verwijder de interactie en er is veel minder te zeggen!

Een goede strategie is om multithreading logica in te kapselen in herbruikbare klassen die onafhankelijk onderzocht en getest kunnen worden. Het Framework zelf biedt veel threading constructies op hoger niveau, die we later behandelen.

Threading brengt ook kosten met zich mee voor resources en CPU bij het plannen en wisselen van threads (als er meer actieve threads zijn dan CPU cores) - en er is ook een creatie/tear-down kost. Multithreading zal je applicatie niet altijd versnellen - het kan zelfs vertragen als het overmatig of ongeschikt wordt gebruikt. Als er bijvoorbeeld veel schijf-I/O bij komt kijken, kan het sneller zijn om een paar worker threads taken achter elkaar te laten uitvoeren dan om 10 threads tegelijk te laten werken. 

## Threads starten en stoppen

Zoals we in de inleiding hebben gezien, worden threads aangemaakt met de constructor van de Thread klasse, waarbij een ThreadStart delegate wordt meegegeven die aangeeft waar de uitvoering moet beginnen.  Hier zie je hoe de ThreadStart delegate wordt gedefinieerd:

```c#
public delegate void ThreadStart();
```

Door Start op de thread aan te roepen, wordt deze in werking gesteld. De thread gaat door tot de methode terugkeert, waarna de thread eindigt. Hier is een voorbeeld, waarbij gebruik wordt gemaakt van de uitgebreide C# syntaxis voor het maken van een TheadStart delegate:

```c#
class ThreadTest
{
  static void Main() 
  {
    Thread t = new Thread (new ThreadStart (Go));
 
    t.Start();   // Run Go() on the new thread.
    Go();        // Simultaneously run Go() in the main thread.
  }
 
  static void Go()
  {
    Console.WriteLine ("hello!");
  }
}
```

In dit voorbeeld voert thread t Go() uit - op (bijna) hetzelfde moment dat de main thread Go() aanroept. Het resultaat zijn twee bijna-onmiddellijke begroetingen.

Een thread kan eenvoudiger worden aangemaakt door alleen een methodegroep op te geven - en C# de ThreadStart delegate te laten afleiden:

```c#
Thread t = new Thread (Go);    // No need to explicitly use ThreadStart
```

Een andere kortere weg is het gebruik van een lambda expressie of anonieme methode:

```c#
static void Main()
{
  Thread t = new Thread ( () => Console.WriteLine ("Hello!") );
  t.Start();
}
```

## Gegevens doorgeven aan een thread

De eenvoudigste manier om argumenten door te geven aan de doelmethode van een thread is het uitvoeren van een lambda-expressie die de methode aanroept met de gewenste argumenten:

```c#
static void Main()
{
  Thread t = new Thread ( () => Print ("Hello from t!") );
  t.Start();
}
 
static void Print (string message) 
{
  Console.WriteLine (message);
}
```

Met deze aanpak kun je een willekeurig aantal argumenten aan de methode doorgeven. Je kunt zelfs de hele implementatie verpakken in een multi-statement lambda:

```c#
new Thread (() =>
{
  Console.WriteLine ("I'm running on another thread!");
  Console.WriteLine ("This is so easy!");
}).Start();
```

Je kunt hetzelfde bijna net zo gemakkelijk doen in C# 2.0 met anonymous methods:

```c#
new Thread (delegate()
{
  ...
}).Start();
```

Een andere techniek is om een argument mee te geven aan de Start methode van Thread:

```c#
static void Main()
{
  Thread t = new Thread (Print);
  t.Start ("Hello from t!");
}
 
static void Print (object messageObj)
{
  string message = (string) messageObj;   // We need to cast here
  Console.WriteLine (message);
}
```

Dit werkt omdat de constructor van Thread overloaded is om een van de volgende twee delegates te accepteren:

```c#
public delegate void ThreadStart();
public delegate void ParameterizedThreadStart (object obj);
```

De beperking van ParameterizedThreadStart is dat het maar één argument accepteert. En omdat het van het type object is, moet het meestal gecast worden.

Lambda expressies en "gevangen variabelen" (captured variables)

Zoals we zagen, is een lambda expressie de krachtigste manier om gegevens aan een thread door te geven. Je moet echter oppassen voor het per ongeluk wijzigen van vastgelegde variabelen na het starten van de thread, omdat deze variabelen gedeeld worden. Denk bijvoorbeeld aan het volgende:

```c#
for (int i = 0; i < 10; i++)
  new Thread (() => Console.Write (i)).Start();
```

De uitvoer is nondeterministisch! Hier is een typisch resultaat:

![image-20211020110224978](./ThreadingImages/TH_6.png)

Het probleem is dat de variabele i tijdens de hele duur van de lus naar dezelfde geheugenplaats verwijst. Daarom roept elke thread Console.Write aan op een variabele waarvan de waarde kan veranderen tijdens de uitvoering!

 De oplossing is het gebruik van een tijdelijke variabele als volgt:

```c#
for (int i = 0; i < 10; i++)
{
  int temp = i;
  new Thread (() => Console.Write (temp)).Start();
}
```

De variabele temp is nu lokaal voor elke lusiteratie. Daarom gebruikt elke thread een eigen geheugenplaats en is er geen probleem. We kunnen het probleem in de eerdere code eenvoudiger illustreren met het volgende voorbeeld:

```c#
string text = "t1";
Thread t1 = new Thread ( () => Console.WriteLine (text) );
 
text = "t2";
Thread t2 = new Thread ( () => Console.WriteLine (text) );
 
t1.Start();
t2.Start();
```

Omdat beide lambda-expressies dezelfde tekstvariabele vangen, wordt t2 tweemaal afgedrukt:

![image-20211020110522073](./ThreadingImages/TH_7.png)

## Threads een naam geven

Elke thread heeft een Name eigenschap die u kunt instellen ten behoeve van debugging. Dit is vooral nuttig in Visual Studio, omdat de naam van de thread wordt weergegeven in het venster Threads en in de werkbalk Debug Location. U kunt de naam van een thread slechts eenmaal instellen; pogingen om deze later te wijzigen zullen een exception opleveren.

De statische eigenschap Thread.CurrentThread geeft u de momenteel uitvoerende thread. In het volgende voorbeeld stellen we de naam van de hoofdthread in:

```c#
class ThreadNaming
{
  static void Main()
  {
    Thread.CurrentThread.Name = "main";
    Thread worker = new Thread (Go);
    worker.Name = "worker";
    worker.Start();
    Go();
  }
 
  static void Go()
  {
    Console.WriteLine ("Hello from " + Thread.CurrentThread.Name);
  }
}
```

## Foreground threads en background threads

Standaard zijn threads die je expliciet aanmaakt "voorgrond-threads". Voorgrond-threads houden de applicatie in leven zolang een van hen actief is, terwijl achtergrond-threads dat niet doen. Zodra alle voorgrond-threads zijn voltooid, eindigt de toepassing en worden alle nog draaiende achtergrond-threads abrupt beëindigd.

De foreground/background status van een thread heeft geen relatie tot de prioriteit of de toewijzing van uitvoeringstijd.

Je kan de achtergrondstatus van een thread opvragen of wijzigen met de eigenschap IsBackground. Hier is een voorbeeld:

```c#
class PriorityTest
{
  static void Main (string[] args)
  {
    Thread worker = new Thread ( () => Console.ReadLine() );
    if (args.Length > 0) worker.IsBackground = true;
    worker.Start();
  }
}
```

Als dit programma zonder argumenten wordt aangeroepen, neemt de worker thread de foreground status aan en wacht op het ReadLine statement tot de gebruiker op Enter drukt. Ondertussen sluit de main thread af, maar de applicatie blijft draaien omdat er nog een foreground thread in leven is.

Aan de andere kant, als een argument wordt doorgegeven aan Main(), krijgt de werker achtergrond status, en het programma verlaat bijna onmiddellijk als de hoofddraad eindigt (en de ReadLine beëindigt).

Wanneer een proces op deze manier eindigt, worden alle "finally blocks" in de uitvoeringsstack van achtergrondthreads omzeild. Dit is een probleem als uw programma gebruik maakt van "finally" blokken (of blokken gebruikt) om opruimwerkzaamheden uit te voeren, zoals het vrijgeven van bronnen of het verwijderen van tijdelijke bestanden. Om dit te voorkomen, kun je dergelijke achtergrondthreads expliciet laten wachten bij het verlaten van een applicatie. Er zijn twee manieren om dit te doen:

- Als je de thread zelf hebt aangemaakt, roep je Join aan op de thread.

- Als je in een gepoolde thread zit, gebruik je een event wait handle.

In beide gevallen moet je een timeout opgeven, zodat je een afvallige thread kunt verlaten als deze om een of andere reden weigert te voltooien. Dit is je backup exit strategie: uiteindelijk wil je dat je applicatie afsluit - zonder dat de gebruiker de hulp van Task Manager hoeft in te roepen!

Als een gebruiker de Task Manager gebruikt om een .NET-proces geforceerd te beëindigen, vallen alle threads "dood neer", alsof het achtergrondthreads zijn. Dit is eerder waargenomen dan gedocumenteerd gedrag, en het kan variëren afhankelijk van de CLR en de versie van het besturingssysteem.

Foreground threads hebben deze behandeling niet nodig, maar je moet er wel voor zorgen dat je geen bugs tegenkomt die ervoor kunnen zorgen dat de thread niet eindigt. Een veel voorkomende oorzaak voor het niet correct afsluiten van applicaties is de aanwezigheid van actieve voorgrondthreads.

## Thread priority

De eigenschap Prioriteit van een thread bepaalt hoeveel uitvoeringstijd hij krijgt ten opzichte van andere actieve threads in het besturingssysteem, op de volgende schaal:

```c#
enum ThreadPriority { Lowest, BelowNormal, Normal, AboveNormal, Highest }
```

Het verhogen van de prioriteit van een thread maakt hem niet in staat om real time werk uit te voeren, omdat hij nog steeds wordt vertraagd door de proces prioriteit van de applicatie. Om real-time werk uit te voeren, moet je ook de proces prioriteit verhogen met behulp van de Process klasse in System.Diagnostics (we hebben je niet verteld hoe je dit moet doen):

```c#
using (Process p = Process.GetCurrentProcess())
  p.PriorityClass = ProcessPriorityClass.High;
```

**ProcessPriorityClass.High** is eigenlijk één stap te weinig voor de hoogste prioriteit: Realtime. Door een proces prioriteit op **Realtime** te zetten, instrueer je het besturingssysteem dat je nooit wilt dat het proces CPU tijd afstaat aan een ander proces. Als je programma per ongeluk in een oneindige lus terecht komt, kan zelfs het besturingssysteem geblokkeerd raken, en is er niets meer over om je te redden, behalve de aan/uit knop! Om deze reden is High meestal de beste keuze voor real-time toepassingen.

Als je real-time applicatie een gebruikersinterface heeft, geeft het verhogen van de proces prioriteit schermupdates buitensporige CPU tijd, waardoor de hele computer trager wordt (vooral als de UI complex is). Het verlagen van de prioriteit van de main thread in combinatie met het verhogen van de prioriteit van het proces zorgt ervoor dat de real-time thread niet wordt voorkruist door het vernieuwen van het scherm, maar lost het probleem van het verhongeren van andere toepassingen van CPU-tijd niet op, omdat het besturingssysteem nog steeds onevenredig veel bronnen zal toewijzen aan het proces als geheel. Een ideale oplossing is om de real-time werker en de gebruikersinterface te laten draaien als afzonderlijke applicaties met verschillende proces-prioriteiten, communicerend via Remoting of in het geheugen gemapte bestanden. Memory-mapped files zijn bij uitstek geschikt voor deze taak.

Zelfs met een verhoogde procesprioriteit is er een grens aan de geschiktheid van de beheerde omgeving om met harde real-time eisen om te gaan. Naast de latentieproblemen die worden veroorzaakt door automatische garbage collection, kan het besturingssysteem extra uitdagingen opleveren - zelfs voor onbeheerde applicaties - die het best kunnen worden opgelost met speciale hardware of een gespecialiseerd real-time platform.

## Exception handling

Eventuele try/catch/finally-blokken in het bereik wanneer een thread wordt aangemaakt, zijn van geen belang voor de thread wanneer hij begint met uitvoeren. Beschouw het volgende programma:

```c#
public static void Main()
{
  try
  {
    new Thread (Go).Start();
  }
  catch (Exception ex)
  {
    // We'll never get here!
    Console.WriteLine ("Exception!");
  }
}
 
static void Go() { throw null; }   // Throws a NullReferenceException
```

De try/catch in dit voorbeeld is ineffectief, en de nieuw aangemaakte thread wordt opgezadeld met een niet-afgehandelde NullReferenceException. Dit gedrag is logisch als je bedenkt dat elke thread een onafhankelijk uitvoeringstraject heeft.

De oplossing is om de exception handler in de Go methode te plaatsen:

```c#
public static void Main()
{
   new Thread (Go).Start();
}
 
static void Go()
{
  try
  {
    // ...
    throw null;    // The NullReferenceException will get caught below
    // ...
  }
  catch (Exception ex)
  {
    // Typically log the exception, and/or signal another thread
    // that we've come unstuck
    // ...
  }
}
```

Je hebt een exception handler nodig op alle thread entry methodes in productieapplicaties - net zoals je dat doet (meestal op een hoger niveau, in de execution stack) op je main thread. Een niet-afgehandelde uitzondering zorgt ervoor dat de hele applicatie wordt afgesloten met een lelijk dialoogvenster!

Bij het schrijven van dergelijke exception handling blokken mag je zelden de fout negeren: meestal zal je de details van de uitzondering loggen, en dan misschien een dialoogvenster tonen waarmee de gebruiker automatisch die details aan je webserver kan doorgeven. Je zou dan de toepassing kunnen afsluiten - omdat het mogelijk is dat de fout de toestand van het programma heeft beschadigd. De kosten hiervan zijn echter dat de gebruiker zijn recente werk zal verliezen - open documenten, bijvoorbeeld.

De "globale" gebeurtenissen voor het afhandelen van uitzonderingen voor WPF- en Windows Forms-applicaties (**Application.DispatcherUnhandledException** en **Application.ThreadException**) lopen alleen af alleen voor uitzonderingen die op de hoofddraad van de UI worden gegooid. U moet uitzonderingen op worker threads nog steeds handmatig afhandelen.

**AppDomain.CurrentDomain.UnhandledException** loopt af op elke niet-afgehandelde uitzondering, maar biedt geen mogelijkheid om te voorkomen dat de toepassing daarna wordt afgesloten.

## Thread pooling

Telkens als je een thread start, worden een paar honderd microseconden besteed aan het organiseren van zaken als een verse private local variable stack. Elke thread gebruikt (standaard) ook ongeveer 1 MB geheugen. De thread pool vermindert deze overheadkosten door threads te delen en te recyclen, waardoor multithreading op een zeer granulair niveau kan worden toegepast zonder prestatieverlies. Dit is nuttig wanneer multicore processoren gebruikt worden om rekenintensieve code parallel uit te voeren in "verdeel-en-heers" stijl.

De thread pool houdt ook een maximum aan het aantal worker threads dat gelijktijdig kan draaien. Te veel actieve threads belasten het besturingssysteem met administratieve rompslomp en maken de CPU-caches ondoeltreffend. Zodra een limiet is bereikt, worden taken in een wachtrij geplaatst en starten ze pas als een andere klaar is. Dit maakt willekeurig gelijktijdige toepassingen mogelijk, zoals een webserver. (Het "asynchrone methode patroon" is een geavanceerde techniek die zeer efficiënt gebruik maakt van de gepoolde threads).

Er zijn een aantal manieren om gebruik te maken van de thread pool:

- Via de Task Parallel Library (vanaf Framework 4.0)
- Door ThreadPool.QueueUserWorkItem aan te roepen
- Via asynchrone delegates
- Via BackgroundWorker

De volgende constructies maken indirect gebruik van de thread pool:

- WCF, Remoting, ASP.NET, en ASMX Web Services toepassingsservers

- System.Timers.Timer en System.Threading.Timer

- Framework methoden die eindigen op Async, zoals die op WebClient (het event-gebaseerde asynchrone patroon), en de meeste BeginXXX methoden (het asynchrone programmeermodel patroon)

- PLINQ

De Task Parallel Library (TPL) en PLINQ zijn krachtig genoeg en van een hoog niveau dat je ze zult willen gebruiken om te helpen bij multithreading, zelfs als thread pooling onbelangrijk is. We bespreken deze in detail in Deel 5; nu bekijken we kort hoe je de Task klasse kan gebruiken als een eenvoudige manier om een delegate op een gepoolde thread uit te voeren.

Er zijn een paar dingen waar je op moet letten bij het gebruik van gepoolde threads:

- Je kunt de naam van een gepoolde thread niet instellen, wat debuggen moeilijker maakt (hoewel je een beschrijving kunt toevoegen bij het debuggen in het venster Threads van Visual Studio).

- Pooled threads zijn altijd achtergrond threads (dit is meestal geen probleem).

- Het blokkeren van een gepoolde thread kan extra latency veroorzaken in de beginfase van een toepassing, tenzij u ThreadPool.SetMinThreads oproept (zie De Thread Pool optimaliseren).

Het staat u vrij om de prioriteit van een gepoolde thread te wijzigen - deze wordt weer op de normale waarde teruggezet wanneer deze weer wordt vrijgegeven aan de pool.

Je kunt opvragen of je momenteel bezig bent met een gepoolde thread via de eigenschap Thread.CurrentThread.IsThreadPoolThread.

### Thread pooling met TPL

Je kunt gemakkelijk de thread pool aanwenden door gebruik te maken van de Task classes in de Task Parallel Library. De Task-klassen zijn geïntroduceerd in Framework 4.0: als je bekend bent met de oudere constructies, beschouw de niet-generieke Task-klasse dan als een vervanging voor ThreadPool.QueueUserWorkItem, en de generieke Task<TResult> als een vervanging voor asynchrone delegates. De nieuwere constructies zijn sneller, handiger en flexibeler dan de oude.

Om de niet-generieke klasse Task te gebruiken, roep je Task.Factory.StartNew aan, waarbij je een delegatie van de doelmethode doorgeeft:

```c#
static void Main()    // The Task class is in System.Threading.Tasks
{
  Task.Factory.StartNew (Go);
}
 
static void Go()
{
  Console.WriteLine ("Hello from the thread pool!");
}
```

**Task.Factory.StartNew** retourneert een Task object, dat je vervolgens kunt gebruiken om de taak te monitoren - je kunt bijvoorbeeld wachten tot de taak is voltooid door de Wait methode aan te roepen.

Alle onbehandelde uitzonderingen worden handig teruggegooid naar de host thread wanneer je de **Wait** methode van een taak aanroept. (Als je Wait niet aanroept en in plaats daarvan de taak verlaat, zal een niet-afgehandelde uitzondering het proces afsluiten zoals bij een gewone thread).

De generieke Task<TResult> klasse is een subklasse van de niet generieke Task. Ze laat je toe een return waarde terug te krijgen van de taak nadat die klaar is met uitvoeren. In het volgende voorbeeld downloaden we een webpagina met Task<TResult>:

```c#
static void Main()
{
  // Start the task executing:
  Task<string> task = Task.Factory.StartNew<string>
    ( () => DownloadString ("http://www.linqpad.net") );
 
  // We can do other work here and it will execute in parallel:
  RunSomeOtherMethod();
 
  // When we need the task's return value, we query its Result property:
  // If it's still executing, the current thread will now block (wait)
  // until the task finishes:
  string result = task.Result;
}
 
static string DownloadString (string uri)
{
  using (var wc = new System.Net.WebClient())
    return wc.DownloadString (uri);
}
```

Het gemarkeerde <string> type argument is voor de duidelijkheid: het zou worden afgeleid als we het weglaten.

Alle niet-afgehandelde uitzonderingen worden automatisch teruggeworpen wanneer je de eigenschap Resultaat van de taak opvraagt, gewikkeld in een **AggregateException**. Als je er echter niet in slaagt de **Result** eigenschap op te vragen (en geen **Wait** oproept), zal elke onbehandelde uitzondering het proces platleggen.

De Task Parallel Library heeft nog veel meer mogelijkheden, en is bijzonder geschikt voor het gebruik van multicore processoren. 

## Optimalisatie van de thread pool

De thread pool begint met één thread in zijn pool. Naarmate taken worden toegewezen, "injecteert" de poolmanager nieuwe threads om de extra gelijktijdige werkbelasting aan te kunnen, tot een maximumlimiet. Na een voldoende lange periode van inactiviteit kan de poolmanager threads "terugtrekken" als hij vermoedt dat dit tot een betere doorvoer zal leiden.

Je kande bovengrens van het aantal threads dat de pool zal aanmaken instellen door ThreadPool.SetMaxThreads aan te roepen; de standaardwaarden zijn:

- 1023 in Framework 4.0 in een 32-bit omgeving

- 32768 in Framework 4.0 in een 64-bit omgeving

- 250 per core in Framework 3.5

- 25 per core in Framework 2.0

(Deze getallen kunnen variëren afhankelijk van de hardware en het besturingssysteem.) De reden dat dit aantal zo hoog is, is om de voortgang te garanderen in het geval dat sommige threads geblokkeerd worden (inactief zijn in afwachting van een bepaalde voorwaarde, zoals een antwoord van een computer op afstand).

Je kunt ook een lagere limiet instellen door **ThreadPool.SetMinThreads** aan te roepen. De rol van de ondergrens is subtieler: het is een geavanceerde optimalisatietechniek die de poolmanager instrueert om de toewijzing van threads niet te vertragen totdat de ondergrens is bereikt. Het verhogen van het minimum aantal threads verbetert de concurrency als er geblokkeerde threads zijn (zie zijbalk).

De standaard ondergrens is één thread per processorkern - het minimum dat volledige CPU-benutting toestaat. In serveromgevingen (zoals ASP.NET onder IIS) is de ondergrens echter meestal veel hoger - tot wel 50 of meer.

### Hoe werkt het minimum aantal threads?

Het verhogen van het minimum aantal threads van de thread pool naar x dwingt niet echt om direct x threads aan te maken - threads worden alleen op aanvraag aangemaakt. Het instrueert eerder de poolmanager om tot x threads aan te maken op het moment dat ze nodig zijn. De vraag is dan waarom de thread pool anders zou wachten met het aanmaken van een thread wanneer die nodig is?

Het antwoord is om te voorkomen dat een korte uitbarsting van kortstondige activiteit een volledige toewijzing van threads veroorzaakt, waardoor het geheugen van een applicatie plotseling volloopt. Ter illustratie: op een quad-core computer draait een clienttoepassing die 40 taken tegelijk in de wachtrij plaatst. Als elke taak een berekening van 10 ms uitvoert, is het geheel in 100 ms voorbij, ervan uitgaande dat het werk over de vier cores wordt verdeeld. Idealiter zouden we willen dat de 40 taken op precies vier threads draaien:

- Minder en we maken niet maximaal gebruik van alle vier de cores.

- Bij meer zouden we geheugen en CPU-tijd verspillen door onnodige threads te maken.

En dit is precies hoe de thread pool werkt. Door het aantal threads aan te passen aan het aantal cores kan een programma een klein geheugengebruik behouden zonder de prestaties te verslechteren - zolang de threads efficiënt worden gebruikt (wat in dit geval het geval is).

Maar stel nu dat in plaats van 10 ms te werken, elke taak het internet bevraagt en een halve seconde wacht op een antwoord terwijl de lokale CPU niet actief is. De thread-economiestrategie van de poolmanager stort in; het zou nu beter zijn om meer threads aan te maken, zodat alle internetqueries tegelijk kunnen plaatsvinden.

Gelukkig heeft de poolmanager een backup plan. Als zijn wachtrij langer dan een halve seconde stilstaat, reageert hij door meer threads aan te maken - één elke halve seconde - tot de capaciteit van de thread pool.

De vertraging van een halve seconde is een tweesnijdend zwaard. Aan de ene kant betekent het dat een eenmalige uitbarsting van korte activiteit er niet voor zorgt dat een programma plotseling 40 MB (of meer) extra onnodig geheugen in beslag neemt. Aan de andere kant kan het nodeloos dingen vertragen wanneer een gepoolde thread blokkeert, zoals bij het bevragen van een database of het aanroepen van **WebClient.DownloadFile**. Daarom kun je de poolmanager vertellen dat hij de toewijzing van de eerste x threads niet moet vertragen, door bijvoorbeeld **SetMinThreads** aan te roepen:

```c#
ThreadPool.SetMinThreads (50, 50);
```

De tweede waarde geeft aan hoeveel threads moeten worden toegewezen aan I/O completion ports, die worden gebruikt door de APM.

De standaardwaarde is één thread per core.

# Oefeningen

1. Start een thread die 1000 keer het cijfer 1 uitschrijft en schrijf vervolgens 1000 keer het cijfer 0 uit (in de "main" thread).
2. Slaap 10 keer 1 seconde in de "main" thread.
3. Vertrek van klasse "BankAccount":

```c#
namespace ThreadingExample
{
    public class BankAccount
    {
        #region Properties
        public double Balance { get; set; }
        public string Name { get; set; }
        #endregion

        #region Ctor
        public BankAccount(string name, double bal)
        {
            Name = name;
            Balance = bal;
        }
        #endregion

        #region Methods
        public double Withdraw(double amt)
        {
            if ((Balance - amt) < 0)
            {
                Console.WriteLine($"Sorry ${Balance} in Account");
                return Balance;
            }

            if (Balance >= amt)
            {
                Console.WriteLine("Removed {0} and {1} left in Account", amt, (Balance - amt));
                Balance -= amt;
            }

            return Balance;
        }

        // You can only point at methods
        // without arguments and that return 
        // nothing
        public void IssueWithdraw()
        {
            Withdraw(1);
        }
        #endregion
    }
}
```

Start 15 threads die een afhaling van 1 EUR uitvoeren op een instantie van BankAccount waarop 10 EUR staat. Maak zeker dat je code "thread safe" uitvoert.

4. Vertrek van volgende static method:

```c#
           static void CountTo(int maxNum)
           {
               for (int i = 0; i <= maxNum; i++)
               {
                   Console.WriteLine(i);
               }
           }
```

Start een thread die tot 10 telt en deze cijfers uitschrijft.  Start vervolgens een thread die telt tot 5 en daarna telt tot 6 en deze cijfers uitschrijft.