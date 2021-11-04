

# Microsoft Visual Studio 2022 stapt over op 64-bit en komt 2021 uit

Microsoft heeft Visual Studio 2022 aangekondigd. Het programma voor softwareontwikkelaars wordt 64-bit en belooft sneller en toegankelijker te zijn dan zijn voorganger. Een **preview wordt deze zomer uitgebracht**, eind 2021 volgt de releaseversie.

Dat maakt Microsoft [bekend in een blogpost](https://devblogs.microsoft.com/visualstudio/visual-studio-2022/). De nieuwe versie van Visual Studio wordt sneller, toegankelijker en meer lichtgewicht, zegt het bedrijf. Voor het eerst zal Visual Studio een 64-bit-applicatie zijn, 'zonder 4GB-beperking in het devenv.exe-proces', schrijft Microsoft. Door over te stappen op 64-bit denkt Microsoft dat het makkelijker moet worden om grote, complexe applicaties te openen, bewerken, draaien en debuggen zonder tegen geheugenproblemen aan te lopen. Ontwikkelaars kunnen nog steeds 32bit-applicaties ontwikkelen in Visual Studio 2022.

De nieuwe versie van Visual Studio krijgt een nieuwe interface, waardoor het minder ingewikkeld moet ogen dan de huidige versie, stelt Microsoft. Er komen nieuwe iconen, nieuwe thema's en een nieuw font, Cascadia Code. Ook krijgt Visual Studio 2022 meer ruimte om het *integrated development environment* naar eigen smaak aan te passen, al vertelt Microsoft nog niet hoe dat precies eruit zal zien.

Visual Studio 2022 krijgt verder ondersteuning voor cloudapplicaties met Azure, waardoor het makkelijker moet zijn om met *repositories* te werken in Visual Studio. En ook krijgt de software **volledige ondersteuning voor .NET 6, waaronder voor [.NET MAUI](https://github.com/dotnet/maui) voor het ontwikkelen van cross-platform apps voor Windows, Android, MacOS en iOS**. Visual Studio krijgt daarnaast een ***hot reload**-*optie voor .NET, zodat ontwikkelaars veranderingen kunnen aanbrengen in code zonder dat ze moeten herstarten.

Ook krijgt het volledige ondersteuning voor C++ met productiviteitstools en IntelliSense. Daarnaast wordt er ondersteuning ingebouwd voor **CMake, Linux en WSL** om het makkelijker te maken om cross-platform-apps in C++ te ontwikkelen met Visual Studio.

Microsoft voegt aan Visual Studio 2022 meer mogelijkheden toe voor gezamenlijk werken aan projecten, onder andere met nieuwe ondersteuning voor **Git en GitHub**. Microsoft introduceert ook **Live Share**, waarbij er geïntegreerde tekstchat komt om gesprekken te hebben over de code van projecten, zonder van context te moeten wisselen. Live Share brengt ook de mogelijkheid om sessies in te plannen voor samenwerken, waarbij dezelfde link kan worden gebruikt voor terugkerende sessies, zoals wekelijkse samenwerkingsafspraken.

Ook de Mac-versie van Visual Studio krijgt een update, met plannen om over te stappen naar 'native MacOS UI', met als doel dat het programma beter werkt op de Mac en zodat de software gebruik kan maken van ingebouwde MacOS-features. Nieuwe menu's moeten de Windows- en Mac-versies van Visual Studio dichter bij elkaar brengen. De Mac-versie krijgt ook de nieuwe Git-ervaring, met een Git Changes-toolwindow.

# **Platform Uno or MAUI? Nog onduidelijk...**

Xamarin.Forms (krijgt de naam MAUI) bestaat reeds langer, alleszins meer dan 5 jaar. 

De punten van gelijkenis:

- In beide gevallen betreft het C# Cross Platform Frameworks
- **Beide ondersteunen XAML**
- Beide ondersteunen iOS, Android, Universal Windows Platform, en (in mindere mate) Mac OS via non-.Forms Xamarin platform frameworks
- Xamarin.Essentials werkt met Xamarin en Uno op hierboven vermelde platformen
- Geen van beide ondersteunt van huize uit Printing, PDF generatie of PNG generatie.

De verschillen:

- Architecturaal is Xamarin.Forms een eigen abstractielaag boven native APIs, terwijl Uno UWP interfaces implementeert bovenop de native APIs. Dit laatste maakt een belangrijk verschil om drie verschillende redenen:
  - In Android omvat de abstractie van Xamarin.Forms het meet- en opmaakbeheer. Dit is ZEER KOSTELIJK en resulteert  voor alle behalve de meest eenvoudige lijstweergaven in zeer slechte prestaties. Uno, daarentegen, voert meet- en opmaakbeheer uit in de native laag - en vermijdt zo buitensporig veel heen en weer verkeer tussen Java en C#.
  - In WASM rendert Xamarin via Blazor - wat hybride server / client applicaties mogelijk maakt. Helaas voegt dit complexiteit toe in vergelijking met de aanpak van Uno. Blijft nog te bezien of de Xamarin aanpak een performantienadeel heeft.
  - Omdat Uno UWP implementeert bovenop de native UI frameworks, is het vrij makkelijk om diepgaande wijzigingen onder de motorkap aan te brengen. Het is veel moeilijker (zo niet onmogelijk in sommige gevallen) om dat in Xamarin.Forms te doen.
- Uno kan gebruikt worden op Windows 7, Tizen en Linux (GTK).
- Uno ondersteunt de complexiteit van UWP XAML: dit is een groot voordeel indien je ervaring hebt met WPF of UWP.
- Uno ondersteunt WinUI en Windows Community Toolkit libraries.
- Xamarin.Forms bestaat een drietal jaar langer dan Platform Uno. Uno heeft Xamarin echter snel ingehaald op het vlak van functionaliteit en codekwaliteit, vooral omdat UWP zeer matuur is. 
- Uno vereist minder lijnen code (in de grootteorde van 65000 lijnen in vergelijking met 132000 lijnen), vooral met betrekking tot de grafische laag (models, view models, business logica blijven doorgaans grotendeels onaangeroerd): UWP controls zijn "feature rich".

Wat betreft Linux:

- Xamarin.Forms: niet ondersteund.
- Uno Platform: ondersteund op voorwaarde dat je een migratie doorvoert. Vergelijk de inspanning met het porteren van je programma van WPF naar UWP.

Afhankelijk van hoe je wat aanpakt kan je een andere ervaring hebben, maar als je ervaring hebt met WPF of UWP kunnen we vooral Uno aanbevelen. Heb je geen ervaring met WPF of UWP, ook dan kan Uno aanbevolen worden omwille van de superieure performantie op Android.