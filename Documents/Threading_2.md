# Het Event-Based Asynchronous Pattern

Het event-based asynchronous pattern (EAP) biedt een eenvoudige manier waarmee klassen multithreading-mogelijkheden kunnen bieden zonder dat gebruikers expliciet threads hoeven te starten of te beheren. Het biedt ook de volgende mogelijkheden:

- Een coöperatief annuleringsmodel
- De mogelijkheid om WPF of Windows Forms besturingselementen veilig bij te werken wanneer de werker is voltooid
- Doorsturen van uitzonderingen naar de completion event

De EAP is slechts een patroon, dus deze functies moeten worden geschreven door de programmeur. Slechts enkele klassen in het Framework volgen dit patroon, met name BackgroundWorker (die we niet zullen behandelen), en WebClient in System.Net. In essentie is het patroon als volgt: een klasse biedt een familie van leden die intern multithreading beheren, vergelijkbaar met het volgende (de gemarkeerde secties geven code aan die deel uitmaakt van het patroon):

```c#
// These members are from the WebClient class:
 
public byte[] DownloadData (Uri address);    // Synchronous version
public void DownloadDataAsync (Uri address);
public void DownloadDataAsync (Uri address, object userToken);
public event DownloadDataCompletedEventHandler DownloadDataCompleted;
 
public void CancelAsync (object userState);  // Cancels an operation
public bool IsBusy { get; }                  // Indicates if still running
```

De *Async methodes voeren asynchroon uit: met andere woorden, ze starten een operatie op een andere thread en keren dan onmiddellijk terug naar de aanroeper. Wanneer de bewerking is voltooid, vuurt de *Completed gebeurtenis - automatisch Invoke aanroepen indien nodig door een WPF of Windows Forms toepassing. Deze gebeurtenis geeft een object met gebeurtenisargumenten terug dat het volgende bevat

Een vlag die aangeeft of de bewerking werd geannuleerd (door de consument die CancelAsync oproept)
Een Error-object dat aangeeft dat er een uitzondering werd gegooid (indien van toepassing)
Het userToken object indien geleverd bij het aanroepen van de Async methode
Hier is hoe we de EAP-leden van WebClient kunnen gebruiken om een webpagina te downloaden:

```c#
var wc = new WebClient();
wc.DownloadStringCompleted += (sender, args) =>
{
  if (args.Cancelled)
    Console.WriteLine ("Canceled");
  else if (args.Error != null)
    Console.WriteLine ("Exception: " + args.Error.Message);
  else
  {
    Console.WriteLine (args.Result.Length + " chars were downloaded");
    // We could update the UI from here...
  }
};
wc.DownloadStringAsync (new Uri ("http://www.linqpad.net"));  // Start it
```

Een klasse die het EAP volgt, kan extra groepen asynchrone methoden aanbieden. Bijvoorbeeld:

```c#
public string DownloadString (Uri address);
public void DownloadStringAsync (Uri address);
public void DownloadStringAsync (Uri address, object userToken);
public event DownloadStringCompletedEventHandler DownloadStringCompleted;
```

Deze zullen echter dezelfde CancelAsync en IsBusy leden delen. Daarom kan slechts één asynchrone operatie tegelijk plaatsvinden.

Tasks zijn beter geschikt.

# Timers

Als je een methode herhaaldelijk en met regelmatige tussenpozen moet uitvoeren, is de gemakkelijkste manier een timer. Timers zijn handig en efficiënt in het gebruik van geheugen en middelen - vergeleken met technieken als de volgende:

```c#
new Thread (delegate() {
                         while (enabled)
                         {
                           DoSomeAction();
                           Thread.Sleep (TimeSpan.FromHours (24));
                         }
                       }).Start();
```

Niet alleen wordt hierdoor een thread resource permanent vastgezet, maar zonder extra codering zal DoSomeAction elke dag op een later tijdstip plaatsvinden. Timers lossen deze problemen op.

Het .NET Framework biedt vier timers. Twee daarvan zijn general-purpose multithreaded timers:

- System.Threading.Timer

- System.Timers.Timer

  

  De andere twee zijn speciale single-threaded timers:

- System.Windows.Forms.Timer (Windows Forms timer)

- System.Windows.Threading.DispatcherTimer (WPF-timer)

  

  De multithreaded timers zijn krachtiger, nauwkeuriger en flexibeler; de single-threaded timers zijn veiliger en handiger voor het uitvoeren van eenvoudige taken die Windows Forms besturingselementen of WPF-elementen bijwerken.

## Multithreaded Timers

**System.Threading.Timer** is de eenvoudigste multithreaded timer: hij heeft slechts een constructor en twee methoden (een genot voor minimalisten, maar ook voor schrijvers van boeken!). In het volgende voorbeeld roept een timer de methode **Tick** aan, die "tick..." schrijft nadat vijf seconden zijn verstreken, en dan elke seconde daarna, tot de gebruiker op Enter drukt:

```c#
using System;
using System.Threading;
 
class Program
{
  static void Main()
  {
    // First interval = 5000ms; subsequent intervals = 1000ms
    Timer tmr = new Timer (Tick, "tick...", 5000, 1000);
    Console.ReadLine();
    tmr.Dispose();         // This both stops the timer and cleans up.
  }
 
  static void Tick (object data)
  {
    // This runs on a pooled thread
    Console.WriteLine (data);          // Writes "tick..."
  }
}
```

Je kan het interval van een timer later wijzigen door de Change methode aan te roepen. Als u wilt dat een timer slechts eenmaal afgaat, specificeer je **Timeout.Infinite** in het laatste argument van de constructor.

Het .NET Framework biedt een andere timer-klasse met dezelfde naam in de **System.Timers** namespace. Deze wrapt eenvoudigweg de System.Threading.Timer, en biedt extra gemak terwijl gebruik wordt gemaakt van dezelfde onderliggende engine. Hier is een samenvatting van de toegevoegde mogelijkheden:

- Een Component implementatie, waardoor het in Visual Studio's designer kan worden geplaatst
- Een interval eigenschap in plaats van een Change methode
- Een **Elapsedevent** in plaats van een callback delegate
- Een **Enabled** eigenschap om de timer te starten en te stoppen (de standaardwaarde is false)
- **Start** en **Stop** methodes voor het geval je verward bent door **Enabled**
- Een **AutoReset** vlag om een terugkerend event aan te duiden (standaardwaarde is true)
- Een **SynchronizingObject** eigenschap met **Invoke** en **BeginInvoke** methoden voor het veilig aanroepen van methoden op WPF elementen en Windows Forms besturingselementen

Hier is een voorbeeld:

```c#
using System;
using System.Timers;   // Timers namespace rather than Threading
 
class SystemTimer
{
  static void Main()
  {
    Timer tmr = new();       // Doesn't require any args
    tmr.Interval = 500;
    tmr.Elapsed += tmr_Elapsed;    // Uses an event instead of a delegate
    tmr.Start();                   // Start the timer
    Console.ReadLine();
    tmr.Stop();                    // Stop the timer
    Console.ReadLine();
    tmr.Start();                   // Restart the timer
    Console.ReadLine();
    tmr.Dispose();                 // Permanently stop the timer
  }
 
  static void tmr_Elapsed (object sender, EventArgs e)
  {
    Console.WriteLine ("Tick");
  }
}
```

Multithreaded timers gebruiken de thread pool om een paar threads toe te laten vele timers te bedienen. Dit betekent dat de callback methode of Elapsed event in een andere thread kan afgaan telkens als deze wordt aangeroepen. Bovendien vuurt **Elapsed** altijd (ongeveer) op tijd - ongeacht of de vorige **Elapsed** al klaar is met uitvoeren. Daarom moeten callbacks of event handlers thread-safe zijn.

De nauwkeurigheid van multithreaded timers hangt af van het besturingssysteem, en ligt meestal in de buurt van 10-20 ms. Als je een grotere nauwkeurigheid nodig hebt, kun je native interop gebruiken en de Windows multimedia timer aanroepen. Deze heeft een nauwkeurigheid tot op 1 ms en is gedefinieerd in **winmm.dll**. Roep eerst **timeBeginPeriod** aan om het besturingssysteem te informeren dat u een hoge timingprecisie nodig hebt, en roep dan **timeSetEvent** aan om een multimediatimer te starten. Als u klaar bent, roept u **timeKillEvent** op om de timer te stoppen en **timeEndPeriod** om het besturingssysteem te laten weten dat u niet langer een hoge tijdnauwkeurigheid nodig hebt. U kunt complete voorbeelden op het Internet vinden die gebruik maken van de multimedia timer door te zoeken op de trefwoorden *dllimport winmm.dll timesetevent*.

## Single-Threaded Timers

Het .NET Framework biedt timers die zijn ontworpen om problemen met de veiligheid van threads voor WPF- en Windows Forms-toepassingen op te lossen:

- System.Windows.Threading.DispatcherTimer (WPF)

- System.Windows.Forms.Timer (Windows Forms)

  

  De single-threaded timers zijn niet ontworpen om buiten hun respectieve omgevingen te werken. Als je bijvoorbeeld een Windows Forms timer gebruikt in een Windows Service toepassing, dan zal de Timer event niet afgaan!

Beide zijn vergelijkbaar met System.Timers.Timer wat betreft de leden die ze tonen (Interval, Tick, Start, en Stop) en worden op een vergelijkbare manier gebruikt. Ze verschillen echter in hoe ze intern werken. In plaats van de thread pool te gebruiken om timer events te genereren, vertrouwen de WPF en Windows Forms timers op het message pumping mechanisme van hun onderliggend user interface model. Dit betekent dat de tick-event altijd afgaat op dezelfde thread die oorspronkelijk de timer heeft aangemaakt - wat in een normale toepassing dezelfde thread is die wordt gebruikt om alle gebruikersinterface-elementen en besturingselementen te beheren. Dit heeft een aantal voordelen:

Je kunt thread safety vergeten.

Een nieuwe **Tick** zal nooit afgaan voordat de vorige **Tick** klaar is met verwerken.

U kunt gebruikersinterface-elementen en besturingselementen rechtstreeks vanuit Tick event handling code updaten, zonder **Control.Invoke** of **Dispatcher.Invoke** aan te roepen.

Het klinkt te mooi om waar te zijn, totdat u zich realiseert dat een programma dat gebruik maakt van deze timers niet echt multithreaded is - er is geen parallelle uitvoering. Eén thread bedient alle timers - en ook de verwerking van UI events. Dit brengt ons bij het nadeel van single-threaded timers: tenzij de Tick event handler snel wordt uitgevoerd, wordt de gebruikersinterface onresponsief.
Dit maakt de WPF en Windows Forms timers alleen geschikt voor kleine taken, meestal die waarbij een aspect van de gebruikersinterface wordt bijgewerkt (bv. een klok of aftel-display). Anders hebt u een multithreaded timer nodig.

In termen van nauwkeurigheid zijn de single-threaded timers vergelijkbaar met de multithreaded timers (tientallen milliseconden), hoewel ze gewoonlijk minder nauwkeurig zijn, omdat ze kunnen worden vertraagd terwijl andere gebruikersinterfaceverzoeken (of andere timergebeurtenissen) worden verwerkt.

## Thread Pools

Als je met veel threads te maken hebt, moet je thread pools overwegen. Het aanmaken van threads kost wat tijd en middelen. En erger nog, je kunt een thread niet hergebruiken.

Thread pool threads blijven in leven. Het doel is om ze opnieuw te gebruiken. Ze wachten in een wachtrij om nieuwe taken toegewezen te krijgen. Je hoeft niet telkens een nieuwe thread te starten als je een nieuwe job hebt. In het algemeen vermindert het het aantal threads. En in feite heb je er niet veel aan om 100 threads te hebben draaien op een vier processor machine. Het systeem kan zelfs trager worden, omdat er veel van context gewisseld moet worden.

Thread pools kunnen automatisch het aantal threads beheren.

```c#
static void hello(object o) {
  for (int i = 0; i < 1000; i++) {
    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " line " + i + " " + o as string);
    Thread.Sleep(2);    // let's wait 2 milliseconds to get some interaction between the two threads
  }
} //
 
static void Main(string[] args) {
  ThreadPool.QueueUserWorkItem(hello, "SomeFunnyParameter");
  ThreadPool.QueueUserWorkItem(hello);
  Console.ReadLine();
} //
```

## Tasks

Een taak is als een kleine add-on voor thread pools. In feite gebruikt de standaard taakplanner dezelfde thread pool als System.Threading.ThreadPool.
Taken geven waarden terug, threads niet. Dat is het grote verschil. De instantie van een task class kan je vertellen of het werk voltooid is en wat het resultaat is. Omdat de taakplanner gebruik maakt van de standaard thread pool, liggen zijn beperkingen in het bereik van deze pool. Om het gedrag van taken aan te passen moet je dus de standaard thread pool aanpassen (tenzij je een aangepaste thread pool of task manager gebruikt). De task manager start geen extra threads als dat nodig is, maar wacht tot de pool de volgende beschikbare thread toewijst.

```c#
static void task1() {
  Task t = new Task(() =>
    Thread.Sleep(1000);
    Console.WriteLine("hello task!");
  });
 
  t.Start();
  t.Wait();
  Console.WriteLine("press enter to exit");
  Console.ReadLine();
}
```

Het aanroepen van t.Wait() is gelijkwaardig aan het aanroepen van Thread.Join().

```c#
static void task2() {
  Task<int> t = new Task<int>(() => {
    Thread.Sleep(1000);
    Console.WriteLine("hello task!");
    return Thread.CurrentThread.ManagedThreadId;
  });
  t.Start();
  //t.Wait();
  Console.WriteLine("task result is: " + t.Result); // implicit wait
  Console.ReadLine();
}
```

Maar wanneer je het t.Result probeert te lezen, zal de huidige thread wachten tot het resultaat van de taak beschikbaar is.
Daarom is t.Wait() uitgecommentarieerd in methode task2().

Laten we nu een vervolg-taak toevoegen. Dit betekent dat we wachten tot de eerste taak klaar is en dan een andere taak starten.

```c#
static void task3() {
  Task<int> t = new Task<int>(() => {
    Thread.Sleep(5000);
    Console.WriteLine("hello task!");
    return 1;
  });
  t.Start();
  Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
  Task<int> t2 = t.ContinueWith((i) => {
    return i.Result + 1;
  });
  Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
  Console.WriteLine("task result is: " + t2.Result); // implicit wait
  Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
  Console.ReadLine();
}
```

De output zou er zo uit kunnen zien:

```text
14:33:31
14:33:31
hello task!
task result is: 2
14:33:36
```

Het punt dat ik hier probeer te maken is dat de t.ContinueWith instructie de huidige thread niet stopt. Op deze manier kun je een keten van commando's construeren die in de toekomst worden uitgevoerd. Het t2.Result wacht dan impliciet tot de eerste en tweede opdracht voltooid zijn.

Het volgende voorbeeld demonstreert het gedrag van ContinueWith wanneer een exceptie wordt gegooid tijdens de taakuitvoering. De code spreekt voor zich. Commentarieer de "throw new Exception();" uit en kijk wat er verandert.

```c#
static void task4() {
  Task<int> t = new Task<int>(() => {
    throw new Exception();
    return 1;
  });
 
  t.Start();
  t.ContinueWith((i) => { Console.WriteLine("Task Canceled"); }, TaskContinuationOptions.OnlyOnCanceled);
  t.ContinueWith((i) => { Console.WriteLine("Task Faulted"); }, TaskContinuationOptions.OnlyOnFaulted);
  var tasks = t.ContinueWith((i) => { Console.WriteLine("Task Completion"); }, TaskContinuationOptions.OnlyOnRanToCompletion);
  tasks.Wait();
} //
```

Tot nu toe maakten we taakinstanties aan door het "new" sleutelwoord te gebruiken. Er is nog een andere optie. Je kunt ook een takenfabriek maken. Dit maakt herhaalde taakcreaties eenvoudiger. Ik voeg ook kindertaken toe om de snelheid wat op te voeren.

```c#
static void task5() {
  Task<int> parent = new Task<int>(() => {
    int[] results = new int[4];
    TaskFactory factory = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);
    factory.StartNew(() => { results[0] = 0; Thread.Sleep(1000); Console.WriteLine("executing sub task 0"); });
    factory.StartNew(() => { results[1] = 1; Thread.Sleep(500); Console.WriteLine("executing sub task 1"); });
    factory.StartNew(() => { results[2] = 2; Thread.Sleep(1000); Console.WriteLine("executing sub task 2"); });
    factory.StartNew(() => { results[3] = 3; Thread.Sleep(750); Console.WriteLine("executing sub task 3"); });
    return results;
  });
 
  parent.Start();
  Task t = parent.ContinueWith(x => {
    for (int i = 0; i < 4; i++) {
      Console.WriteLine("result of task " + i + " = " + parent.Result[i].ToString());
    }
  });
 
  t.Wait();
  Console.ReadLine();
} //
```

## Klasse Parallel

De parallelle klasse kan worden gevonden in de System.Threading.Tasks namespace. Het heeft een paar statische methodes die ontworpen zijn om code gelijktijdig uit te voeren. Het heeft zin om parallellisme te gebruiken als de lengte van je code het aanmaken van taken rechtvaardigt, de code elkaar niet te veel blokkeert (bv. lock(this) {}), de processoren vrije capaciteit hebben en de code niet in een sequentie moet lopen. Anders zal de prestatie er waarschijnlijk onder lijden.

Laten we eens kijken naar enkele voorbeelden:

```c#
public static void Parallel_For() {
    Parallel.For(0, 10, i => {
        Console.WriteLine("parallel start " + i);
        Thread.Sleep(0);
        Console.WriteLine("parallel end " + i);
    });
    Console.ReadLine();
} //
```

```c#
public static void Parallel_ForEach() {
    int[] n = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    Parallel.ForEach(n, i => {
        Console.WriteLine("parallel start " + i);
        Thread.Sleep(0);
        Console.WriteLine("parallel end " + i);
    });
    Console.ReadLine();
} //
```

```c#
public static void Parallel_ForBreak() {
    ParallelLoopResult result = Parallel.For(0, 10, (i, loopstate) => {
        Console.WriteLine("parallel start " + i);
        Thread.Sleep(0);
        if (i >= 5) loopstate.Break();
        //if (i >= 3) loopstate.Stop();
        Console.WriteLine("parallel end " + i);                                
    });
    Console.WriteLine("IsCompleted: " + result.IsCompleted);
    Console.WriteLine("LowestBreakIteration: " + result.LowestBreakIteration);
    Console.ReadLine();
} //
```

Het veld IsCompleted retourneert false als de lus niet is voltooid. En LowestBreakIteration vertegenwoordigt het laagste iteratiegetal van waaruit de break-instructie werd aangeroepen. Alle lagere iteratienummers worden uitgevoerd. De functie breekt niet, de code na de break wordt nog steeds uitgevoerd! Het Break() statement kan worden gebruikt in zoek-gebaseerde algoritmen waar een ordening aanwezig is in de gegevensbron.

```c#
public static void Parallel_ForBreak() {
    ParallelLoopResult result = Parallel.For(0, 100, (i, loopstate) => {
        Console.WriteLine("parallel start " + i);
        Thread.Sleep(0);
        //if (i >= 5) loopstate.Break();
        if (!loopstate.IsStopped) {
            if (i >= 25) loopstate.Stop();
            Console.WriteLine("parallel end " + i);
        }
    });
    Console.WriteLine("IsCompleted: " + result.IsCompleted);
    Console.WriteLine("LowestBreakIteration: " + result.LowestBreakIteration);
    Console.ReadLine();
} //
```

Het Stop() statement retourneert null in LowestBreakIteration. De code na Stop() wordt nog steeds uitgevoerd!

Break() voltooit alle iteraties op alle threads die voor de huidige iteratie op de huidige thread staan, en verlaat dan de lus.

Stop() stopt alle iteraties zo snel als mogelijk is.

## PLINQ

Language-Integrated Query (LINQ) is in C# geïmplementeerd om queries uit te voeren over alle soorten gegevens, zoals objecten of databases. Parallel Language-Integrated Query (PLINQ) is een andere benadering om objecten te benaderen. Zoals de naam al zegt, heeft het te maken met parallellisme. De evolutie van sequentiële query's (LINQ) naar parallelle query's (PLINQ) was voorspelbaar. Uitbreidingsmethoden voor PLINQ zijn gedefinieerd in de klasse System.Linq.ParallelEnumerable.

```c#
public static void PLINQ1() {
    int[] range = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
 
    Console.WriteLine("old school");
    for (int i = 0, n = range.Length; i < n; i++) {         if (range[i] % 2 == 0) Console.WriteLine(range[i]);     }     Console.WriteLine("LINQ");     var linq = from i in range where (i % 2 == 0) select i;     foreach (int i in linq) Console.WriteLine(i);     Console.WriteLine("LINQ2");     var linq2 = range.Where(i => i % 2 == 0);
    foreach (int i in linq2) Console.WriteLine(i);
 
    Console.WriteLine("PLINQ1");
    var plinq = from i in range.AsParallel() where (i % 2 == 0) select i;
    foreach (int i in plinq) Console.WriteLine(i);
 
    Console.WriteLine("PLINQ2");
    var plinq2 = range.AsParallel().Where(i => i % 2 == 0);
    foreach (int i in plinq2) Console.WriteLine(i);
 
    Console.WriteLine("PLINQ3");
    var plinq3 = range.AsParallel().Where(i => { Thread.Sleep(1000); return (i % 2 == 0); });
    foreach (int i in plinq3) Console.WriteLine(i);
 
    Console.WriteLine("PLINQ3 sorted");
    var plinq3sorted = range.AsParallel().AsOrdered().Where(i => { Thread.Sleep(1000); return (i % 2 == 0); });
    foreach (int i in plinq3sorted) Console.WriteLine(i);
 
    Console.ReadLine();
} //
```

Interessant is dat de runtime zelf beslist of het zinvol is om je query parallel uit te voeren. Parallelle uitvoering is dus niet gegarandeerd, tenzij je WithExecutionMode(ParallelExecutionMode.ForceParallelism) specificeert.
Je kunt zelfs nog specifieker worden door WithDegreeOfParallelism() te gebruiken en het aantal processoren dat wordt gebruikt te beperken.

```c#
public static void PLINQ2() {
    var range = Enumerable.Range(0, 25);
    var result = range.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Where(i => i % 2 == 0);
    //var result = range.AsParallel().WithDegreeOfParallelism(4).Where(i => i % 2 == 0);
    //var result = range.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).WithDegreeOfParallelism(4).Where(i => i % 2 == 0);
    foreach (int i in result) Console.WriteLine(i);
 
    Console.ReadLine();
} //
```

En nu worden we geconfronteerd met iets vreemds. We hebben een gesorteerd resultaat gemaakt, maar op de een of andere manier gaat de sortering verloren! De reden is dat in tegenstelling tot "foreach" ForAll() niet wacht op alle resultaten voordat het begint met uitvoeren.
Vandaar dat we een onverwacht resultaat zien.

```c#
public static void PLINQ3() {
    var range = Enumerable.Range(0, 25);
 
    var plinq2sorted = range.AsParallel().AsOrdered().Where(i => { Thread.Sleep(1000); return (i % 2 == 0); });
    Console.WriteLine("sorted");
    foreach (int i in plinq2sorted) Console.WriteLine(i);
    Console.WriteLine("something is going wrong?");
    plinq2sorted.ForAll(i => Console.WriteLine(i));
 
    Console.ReadLine();
} //
```

Parallelle queries kunnen mislukken en excepties werpen. Deze excepties zullen de uitvoering van de queries niet stoppen. Ze worden verzameld en geretourneerd in een enkele uitzondering die kan worden opgevangen door het gebruikelijke try/catch blok.

```c#
public static void PLINQ4() {
    var range = Enumerable.Range(0, 25);
 
    try {
        var plinq2sorted = range.AsParallel().AsOrdered().Where(i => {
            Thread.Sleep(500);
            if (i > 15) throw new ArgumentException("exception for number " + i);
            return (i % 2 == 0);
        });
 
        foreach (int i in plinq2sorted) Console.WriteLine(i);
    }
    catch (AggregateException e) {
        Console.WriteLine("Exception thrown for " + e.InnerExceptions.Count + " results");
        foreach (var innerException in e.InnerExceptions) Console.WriteLine(innerException.Message);
    }
 
    Console.ReadLine();
} //
```

## Concurrent collections

Collecties benaderen kan lastig zijn in een multithreaded omgeving. Je kan "lock()" gebruiken om toegang te krijgen tot gegevens van verschillende threads. C# 4.0 introduceerde concurrent collections, deze vereisen geen expliciete synchronisatie. Wees voorzichtig met het gebruik ervan. Ik heb veel performantietesten uitgevoerd en concurrent collections bleken vaak trager te zijn. In feite moet je je persoonlijke aanpak benchmarken met de hoeveelheid en frequentie van gegevens die je verwacht. Alleen dan kun je zeggen wat voor soort collectie je moet gebruiken.

### BlockingCollection

Stel je voor dat er gegevens op je PC binnenkomen en je zou die gegevens in een wachtrij willen zetten, zodat de buffer niet overloopt. De gegevens worden dan verwerkt zodra de CPU's vrije capaciteit hebben. BlockingCollection is een wrapper voor andere collections. Het standaard verzameltype is ConcurrentQueue. De BlockingCollection wacht (blokkeert) tot er gegevens aankomen voor het geval hij leeg is.

Zoals je weet hou ik ervan korte stukjes code te schrijven in plaats van veel uit te leggen. Door naar de resultaten te kijken kun je het mechanisme gemakkelijk begrijpen.

```c#
public static void BlockingCollection1() {
    BlockingCollection<int> lCollection = new BlockingCollection<int>();
 
    Task.Factory.StartNew(() => {
        for (int i = 0; i < 5; i++) {
            lCollection.Add(i);
            Thread.Sleep(200);
        }
 
        lCollection.CompleteAdding();
    });
 
    Task.Factory.StartNew(() => {
        try {
            while (true) Console.WriteLine(lCollection.Take());
        }
        catch (Exception e) {
            Console.WriteLine("exception thrown: " + e.Message);
        }
    });
 
    Console.ReadLine();
} //
```

Mogelijke uitvoer:

```text
0
1
2
3
4
exception thrown: The collection argument is empty and has been marked as complete with regards to additions.
```

Elk nummer verschijnt met een vertraging van ongeveer 200 milliseconden na het vorige. lCollection.CompleteAdding() werpt dan een exceptie en vertelt dat er geen elementen meer zullen volgen. Dit kan nuttig zijn om een taak te beëindigen die zich in een wachtstand bevindt vanwege Take().

```c#
public static void BlockingCollection2() {
    BlockingCollection<int> lCollection = new BlockingCollection<int>();
 
    Task.Factory.StartNew(() => {
        for (int i = 0; i < 5; i++) {
            lCollection.Add(i);
            Thread.Sleep(200);
        }
 
        lCollection.CompleteAdding();  // comment it out for testing purposes
    });
 
 
    foreach (int i in lCollection.GetConsumingEnumerable()) Console.WriteLine(i);    
 
    Console.ReadLine();
} //
```

De uitvoer van bovenstaande code gedraagt zich hetzelfde, alleen de exception wordt niet gegooid.
En als je "lCollection.CompleteAdding();" weglaat, verandert het gedrag niet zichtbaar. Maar er is één groot verschil. De foreach weet niet dat je klaar bent met toevoegen aan de BlockingCollection. Daarom gedraagt "foreach" (in combinatie met .GetConsumingEnumerable()) zich als een eindeloze lus en komt het programma nooit aan bij Console.Readline().
Je programma vertellen dat je klaar bent met het toevoegen aan de verzameling is van vitaal belang.

Laten we ".GetConsumingEnumerable()" vermijden en kijken wat er gebeurt:

```c#
public static void BlockingCollection3() {
    BlockingCollection<int> lCollection = new BlockingCollection<int>();
 
    Task.Factory.StartNew(() => {
        for (int i = 0; i < 5; i++) {
            lCollection.Add(i);
            Thread.Sleep(200);
        }
 
        lCollection.CompleteAdding();
    });
 
 
    //Thread.Sleep(2000);
    foreach (int i in lCollection) Console.WriteLine(i);
 
    Console.ReadLine();
} //
```

Lieve hemel! Het programma wacht niet tot de collectie gevuld is. Het programma drukt geen uitvoer af. Haal het commentaar weg van "Thread.Sleep(2000);" en de verzameling wordt correct gevuld op het moment dat het programma de uitvoer afdrukt. De getallen 1 tot en met 4 worden in één keer afgedrukt, zonder vertraging ertussen.

### ConcurrentBag

Aangezien er geen wachtrij betrokken is bij de klasse ConcurrentBag, kan en zal ze IEnumerabe implementeren. TryTake() krijgt wel de volgende waarde. Het commando gebruikt een "out" parameter, wat vrij slim is. Je kunt de geldigheid van het resultaat testen en tegelijkertijd het resultaat zelf krijgen. Dat is erg handig in multithreading. Anders zou je eerst moeten testen en dan de waarde moeten ophalen. Maar om dat te doen, zou je de collectie moeten lock(). En dat is hier niet nodig.

ConcurrentBag staat dubbele entries toe. De volgorde is willekeurig, verwacht niet dat het zich gedraagt als een lijst. Je kunt getallen 0 tot 5 toevoegen en ze kunnen bv. in omgekeerde volgorde worden afgedrukt. Maar dit hoeft niet noodzakelijkerwijs te gebeuren.

Er is een methode die niet erg bruikbaar is in een multithreaded omgeving. TryPeek() kan je in sommige collections vinden. Maar als je die methode gebruikt, kun je er niet zeker van zijn dat hij er nog is als je de waarde probeert te krijgen. Je zou dan lock() opnieuw moeten gebruiken. En dit is in feite in tegenspraak met het idee van concurrent collections.

```c#
public static void ConcurrentBag1() {
    ConcurrentBag<int> lCollection = new ConcurrentBag<int>();
 
    Task.Factory.StartNew(() => {
        for (int i = 0; i < 5; i++) {
            lCollection.Add(i);
            Thread.Sleep(200);
        }
    });
 
    //Thread.Sleep(500);
    //Thread.Sleep(2000);
 
    int lResult;
    while (lCollection.TryTake(out lResult)) {
        Console.WriteLine(lResult);
    }
             
    Console.ReadLine();
} //
```

Het programma wacht niet tot de collectie gevuld is. Als je geluk hebt zie je een "0" op het scherm verschijnen. TryTake() is helemaal niet aan het blokkeren (wachten). Speel met het commentaar en kijk naar de resultaten. Hoe langer je wacht, hoe meer getallen er verschijnen.

```c#
public static void ConcurrentBag2() {
    ConcurrentBag<int> lCollection = new ConcurrentBag<int>();
    for (int i = 0; i < 10; i++) lCollection.Add(i);
    foreach (int r in lCollection) Console.WriteLine(r);
  
    Console.ReadLine();
} //
```

Hierboven is een snelle demonstratie van de IEnumerable. Merk op dat de volgorde van de uitvoer willekeurig kan zijn.

Het basisgedrag van concurrent collection is nu uitgelegd. Stacks en Queues zijn geen nieuwe ideeën in C#.
Ik zal dus alleen de hoofdpunten opnoemen:

### ConcurrentStack

- Push()/PushRange() om gegevens toe te voegen
- TryPop()/TryPopRange() om gegevens te krijgen
- Last in first out (LIFO)

### ConcurrentQueue

- Enqueue() om gegevens toe te voegen
- TryDequeue() om gegevens te krijgen
- Eerste in, eerste uit (FIFO)

### ConcurrentDictionary

Ook de Dictionary is welbekend in C#. De concurrent versie ervan kan atomair gegevens toevoegen, ophalen en bijwerken. Atomic betekent dat operaties starten en eindigen als enkele stappen en zonder tussenkomst van andere threads.

TryAdd geeft true terug als de nieuwe invoer met succes werd toegevoegd. Als de sleutel al bestaat, retourneert deze methode false.
TryUpdate vergelijkt de bestaande waarde voor de gespecificeerde sleutel met een gespecificeerde waarde, en als ze gelijk zijn, wordt de sleutel bijgewerkt met een derde waarde.
AddOrUpdate voegt een record toe als de sleutel nog niet bestaat, of werkt deze bij als de sleutel al bestaat.
GetOrAdd voegt een item toe als de sleutel nog niet bestaat.

```c#
public static void ConcurrentDictionary1() {
    ConcurrentDictionary<string, int> lCollection = new ConcurrentDictionary<string, int>();
    if (lCollection.TryAdd("a", 1)) Console.WriteLine("successfully added entry: a/1");
    PrintConcurrentDictionary(lCollection);
    if (lCollection.TryUpdate("a", 2, 1)) Console.WriteLine("updated value to 2");
    PrintConcurrentDictionary(lCollection);
    lCollection["a"] = 3;
    PrintConcurrentDictionary(lCollection);
    int x = lCollection.AddOrUpdate("a", 4, (s, i) => i * i);
    PrintConcurrentDictionary(lCollection);
    int y = lCollection.GetOrAdd("b", 5);
    PrintConcurrentDictionary(lCollection);
 
    Console.ReadLine();
} //
```

Mogelijke uitvoer:

```text
successfully added entry: a/1
—————————————–
a = 1
updated value to 2
—————————————–
a = 2
—————————————–
a = 3
—————————————–
a = 9
—————————————–
b = 5
a = 9
```

## Deadlocks

