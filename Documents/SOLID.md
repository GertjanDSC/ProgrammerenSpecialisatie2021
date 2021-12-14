## 13. SOLID

### Inleiding

Software ontwikkelaars worden vaak geconfronteerd met ontwerpproblemen. Professionals stellen echter vast dat bepaalde soorten van ontwerpproblemen steeds terugkomen. Eenmaal je een probleem herkent als een variant van een probleem dat je vroeger al eens hebt opgelost, kan je gebruik maken van de inzichten die je al verworven hebt. Je ziet bepaalde patronen (patterns) terugkeren ... .

### Wat is nu precies een ontwerppatroon (design pattern)

> Een ontwerppatroon is een standaardoplossing voor een vaak voorkomend ontwerpprobleem. Deze patronen zijn belangrijk omdat ze je de moeite kunnen besparen om telkens opnieuw het warm water uit te vinden. Bovendien heeft elk patroon een eigen naam, wat ervoor zorgt dat het heel eenvoudig wordt om bepaalde complexe ideeën in een oogwenk te communiceren aan een andere programmeur.

### Geschiedenis van het ontwerpen

Sinds het begin van het computertijdperk is probleem-oplossend denken ingrijpend veranderd.

#### Programmeren: the sequel

In het begin programmeerden we met *assembly*, en was elk programma beperkt tot een honderdtal lijnen. Elke programmeur had een eigen stijl volgens intuïtie.

#### Programmeren: flow based

Toen de complexiteit toenam, gingen meerdere programmeurs code reviews verrichten bij elkaar en merkte men al dat onderhoud en begrijpen van code niet voor de hand lag. Men trachtte normen op te leggen en ging flowcharts maken om programmeurs een goed design te laten maken. Flowcharts bleken ook nuttig om programma’s eenvoudiger te begrijpen.

#### Programmeren: gestructureerd

Gestructureerd programmeren volgde in de jaren ‘70. Een gestructureerde code bestaat uit één enkel begin en afsluitpunt en daar tussen een set van modules. Gestructureerde programma’s zijn makkelijker te lezen en te begrijpen, te onderhouden en vereisen minder ontwikkel-tijd.

#### Programmeren: object oriented

Object-georiënteerd programmeren gebeurt intuïtief en identificeert natuurlijke objecten ( Hero, vijand, ...) die voorkomen in je probleem. Daarnaast worden relaties zoals composities, referenties, overerving bepaald. Dit resulteert in herbruikbaarheid van code, en overzichtelijkere en makkelijk te onderhouden code.

#### Vandaag...

Door de toenemende concurrentie moet je als programmeur tegenwoordig zeer dynamisch (Agile) zijn. Ook is de gemiddelde levensduur van een product drastisch verlaagd. Organisaties moeten snel op marktveranderingen kunnen antwoorden. Ook worden business strategieën snel aangepast wat wil zeggen dat bijvoorbeeld een goed software design zeer belangrijk is om snel op deze veranderingen in te kunnen inspelen. Software moet snel ontwikkeld kunnen worden en staat dicht bij de klant (deze kan al vaak worden betrokken bij de ontwikkeling van gepersonaliseerde software).

#### Object oriented

De basisgedachte achter object georiënteerd programmeren is dat mensen een beetje van de realiteit proberen te modelleren zodat het model in de vorm van een werkend programma kan worden gegoten. Je kan je object model beschouwen als een blackbox. Bijvoorbeeld een auto als blackbox betekent dat je een handvol pedalen, schakelaars hebt die fungeren als interface. Duwen op de rem betekent dat je auto mindert, maar je hoeft niet te weten hoe dat gebeurt, enkel maar wat er gebeurt. Dit principe heet encapsulatie.

> Encapsulatie: je probeert zoveel mogelijk zaken af te schermen.

Bijvoorbeeld een auto kan starten, maar je weet niet wat er allemaal moet gebeuren om de auto te starten. Dit noemen we een interface.

#### Klassen en objecten

> Klasse: een beschrijving en verzameling van dingen (objecten) met soortgelijke eigenschappen
>
> Object: een instantie van een klasse

Als voorbeeld kunnen we een auto nemen.Een auto catalogeren we als een klasse, want bestaat uit een aantal eigenschappen, zoals de kleur van de auto, het aantal pk, benzine of diesel motor, enzovoort. Maar ook het starten, stoppen, schakelen van de wagen worden als eigenschappen bezien.

Een object betekent bijvoorbeeld een nieuwe Renault met een rode kleur, 100pk en dieselmotor.

De auto is de klasse die beschrijft hoe een auto er voor onze probleemstelling moet uit zien, terwijl de renault een instantie van de klasse is, of ook wel object genoemd. Wat betekent dat dit een effectieve auto is die je kan gebruiken.

### Hoe maak je een klasse

Een klasse kan bestaan uit:

- private member variabelen: bepalen de toestand van de klasse
- constructor
- public methoden: aanspreekpunten voor de buitenwereld, of interfaces genoemd
- properties: een gecontroleerde toegang tot de toestand
- private methoden: hulpmethoden die enkel beschikbaar zijn binnen het object

### SOLID

[S.O.L.I.D.](https://blog.cleancoder.com/uncle-bob/2020/10/18/Solid-Relevance.html) zijn 5 principes die ons helpen om een goede software architectuur te schrijven (Robert C. Martin - Uncle Bob)

- [S : SRP (Single responsibility principle)](./SolidSRP.md)
- [O : OCP (Open closed principle)](./SolidOCP.md)
- [L : LSP (Liskov substitution principle)](./SolidLSP.md)
- [I : ISP (Interface segregation principle)](./SolidISP.md)
- [D : DIP (Dependency inversion principle)](./SolidDIP.md)

### Praktische implementatie

#### Algemeen

Zie de fantastische website [refactoring guru](https://refactoring.guru/design-patterns/csharp).

#### Mediator

* [MediatR](https://github.com/jbogard/MediatR)
* [Brighter](https://www.goparamore.io/)

