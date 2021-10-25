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

