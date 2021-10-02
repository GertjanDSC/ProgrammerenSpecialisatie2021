# UDP versus TCP

![TCP versus UDP](./tcp_udp.jpg "TCP versus UDP")

TCP, UDP, netwerkprotocollen, databits: de kans is groot dat deze termen je niets zeggen als je geen ICT-expert bent. Toch is het handig om te weten wat deze termen betekenen. Wanneer je surft op het internet, je e-mail gebruikt of bestanden verzendt, maak je gebruik van TCP. Beschikbaarheid van servers en streaming zoals een live video bekijken zijn enkele toepassingen van UDP. 

TCP staat voor **Transmission Control Protocol**. Het is een veelgebruikt protocol. Hiermee worden gegevens overgedragen op het internet via netwerkverbindingen, maar ook op computernetwerken. TCP kan gegevens in een datastroom versturen, wat betekent dat deze gegevens gegarandeerd aankomen op hun bestemming. Communicatiefouten worden daarnaast ook opgevangen. TCP wordt niet alleen gebruikt voor verkeer op het internet, maar ook voor het downloaden en streamen van video’s.

Hoe werkt TCP? Wanneer je vanaf jouw computer op een link van een website klikt, stuurt de browser zogenaamde TCP packets naar de server van de betreffende website. De server van de website stuurt ook weer TCP packets terug. De packets krijgen een getal, waardoor de ontvanger deze packets in de juiste volgorde krijgt. Behalve het sturen van de packets controleert TCP deze data ook. De server stuurt dan bericht naar de verzender om de ontvangst van packets te bevestigen. Bij een onjuist antwoord worden de packets opnieuw gestuurd.

![TCP](./TCP_vs_UDP_01.gif "TCP")

UDP staat voor **User Datagram Protocol**. Dit is een bericht-georiënteerd protocol. Dit wil zeggen dat een verzender een bericht stuurt aan de ontvanger, net als bij het TCP protocol. Het verschil met TCP is echter dat de ontvanger bij UDP geen bevestiging stuurt naar de verzender. Dit betekent dat UDP vooral geschikt is voor eenrichtingscommunicatie, waarbij het verlies van enige data geen probleem vormt. Het UDP protocol wordt vooral gebruikt bij live streaming en online gaming.

UDP is sneller dan TCP, omdat het geen controles uitvoert en geen tweerichtingsverkeer is. Dit betekent echter wel dat UDP minder betrouwbaar is dan TCP als het gaat om het versturen van data.

![UDP](./TCP_vs_UDP_02.gif "UDP")

Wanneer gebruik je TCP en wanneer gebruik je UDP? TCP wordt vaak gebruikt wanneer er sprake is van een belangrijke overdracht van informatie. Denk hierbij aan het versturen van een bestand van de ene naar de andere computer. Het gaat hierbij niet om de snelheid, maar om de accuratesse waarmee een bestand wordt verstuurd. UDP wordt vooral gebruik wanneer snelheid boven veiligheid en accuratesse gaat. Een 100% foutloze verbinding is in dit geval niet noodzakelijk. Denk hierbij aan het streamen van een live video of online gaming. Kort gezegd draait het bij TCP om nauwkeurigheid en bij UDP om snelheid. Wil je een beide gevallen verzekerd zijn van een veilige verbinding? Gebruik dan een Virtual Private Network (VPN). Een VPN versleutelt je connectie, terwijl de snelheid op peil blijft.

UDP vs. TCP: wat zijn de belangrijkste verschillen tussen deze twee? In de eerste plaats gaat het om de manier waarop gegevens en data uitgewisseld worden. Toch zijn er nog meer verschillen zichtbaar. Hier vind je een overzicht van deze verschillen in een overzichtelijke tabel:

![TCP](./tcp-upd_infographic.png "TCP")

TCP zorgt gegarandeerd voor een betrouwbare maar ook een geordende levering van gegevens van de gebruiker naar de server en andersom. UDP is niet bedoeld voor end-to-end verbindingen en communicatie en controleert de gereedheid van de ontvanger niet. Verschillen:

### 1: Betrouwbaarheid

Wanneer je verzekerd wil zijn van een betrouwbare overdracht van informatie, kan je het beste TCP gebruiken. Waarom is TCP betrouwbaarder? Hier worden bericht-bevestiging en hertransmissies beheert wanneer er sprake is van verloren onderdelen. Er zullen dus nooit gegevens ontbreken. Bij UDP heb je nooit de zekerheid of de communicatie de ontvanger heeft bereikt. Concepten van bevestiging, hertransmissie en time-out zijn niet aanwezig.

### 2: Ordening van pakketten

Bij TCP overdrachten is er altijd sprake van een bepaalde volgorde. De data wordt in een bepaalde reeks naar de server verzonden, en kom in dezelfde volgorde terug. Komen bepaalde gegevens in de verkeerde volgorde aan? Dan herstelt TCP dat en verstuurt de data opnieuw. Bij UDP is er geen sprake van een volgorde. Van te voren kun je dan ook niet voorspellen in welke volgorde de gegevens worden ontvangen.

### 3: Verbinding

Bij TCP is een zwaargewicht verbinding die drie pakketten vereist voor een zogenaamde socket-verbinding. Een socket-verbinding wordt toegepast wanneer een verbinding met een andere host tot stand wordt gebracht. Een socket bestaat altijd uit een IP-adres. Deze verbinding zorgt bij TCP voor betrouwbaarheid. UDP is een lichtgewicht transportlaag, gecreëerd op een IP. Volgverbindingen of het ordenen van gegevens is dan ook niet mogelijk.

### 4: Foutencontrole

TCP gebruikt niet alleen foutencontrole, maar ook foutenherstel. Fouten worden gedetecteerd door middel van een controle. Is een pakket foutief? Dan wordt het niet door de ontvanger bevestigd. Daarna is er sprake van een hertransmissie door de verzender. Dit mechanisme wordt ook wel **Positive Acknowledgement with Retransmission** (PAR) genoemd.

UDP werkt op basis van best-effort. Dit betekent dat het protocol foutdetectie wel ondersteunt, maar er niets mee doet. Een fout kan worden gedetecteerd, maar daarna wordt het pakket genegeerd. Er wordt niet geprobeerd om het pakket opnieuw te verzenden om de fout te herstellen zoals dat bij TCP wel het geval is. Dit komt omdat UDP vooral wordt gebruikt voor de snelheid.

## SimpleTCP

SimpleTcp is een leuke bibliotheek, die veel kan. [Hier](https://github.com/BrandonPotter/SimpleTCP) zie je enkele ideeën. 

### Client-server chat programma

We maken een klein , eenvoudige applicatie zodat meerdere clients berichten naar de server kunnen sturen. We gaan hiervoor gebruik maken van een bestaande Nuget-Bibliotheek `SimpleTcp`

Voer volgende stappen uit:

1. Maak een nieuwe solution aan en voeg 2 projecten toe, 1 voor de client, 1 voor de server.
2. Voeg aan ieder project de SimpleTc Nuget-bibliotheek toe als volgt:
   1. Rechterklik op je project (in solution explorer)
   2. Kies "Manage nuget packages..."
   3. Klik op "Browse" in het nieuw verschenen scherm
   4. Zoek naar `SimpleTcp`
   5. Klik op de eerste hit (die van BrandonPotter) en kies rechts op "Install"
3. Als alles goed is verlopen zie je bij de references in beide projecten nu ook `SimpleTcp` staan.
4. Voeg in iedere Program.cs bovenaan `using SimpleTCP;` toe
5. Profit!

### Beide projecten starten

Om je programma de komende tijd te testen wil je uiteraard steeds dat er minstens 1 server en 1 client loopt. We gaan dit als volgt doen:

1. Rechterklik op je solution (in solution explorer)
2. Kies "Properties"
3. Ga naar "Startup Project" onder de "Common properties"
4. Selecteer "Multiple startup project"
5. Verander de action van beide projecten naar "Start"
6. Zorg ervoor dat je server-project eerst start: indien nodig klik je op het pijltje omhoog zodat die bovenaan staat

Als je nu je programma start (F5) of debugt dan zullen steeds beide projecten uitgevoerd worden.

### Server-code

Telkens de server een string krijgt die eindigt op een enter zal de server deze boodschap op het scherm tonen. Om te voorkomen dat de server afsluit van zodra hij lijn 2 heeft uitgevoerd plaatsen we een ``ReadLine```achteraan. Op die manier zal de server blijven reageren op events tot de gebruiker op enter duwt om alles af te sluiten:

```csharp
static void Main(string[] args)
{
    var server = new SimpleTCP.SimpleTcpServer().Start(1111);
    server.DelimiterDataReceived += Server_DelimiterDataReceived;
    Console.ReadLine();
}

private static void Server_DelimiterDataReceived(object sender, SimpleTCP.Message e)
{
    Console.WriteLine( e.MessageString);
}
```

### Client-code

```csharp
static void Main(string[] args)
{
    var client = new SimpleTcpClient().Connect("127.0.0.1", 1111);
    while (true)
    {
        string msg = Console.ReadLine();
        client.WriteLine(msg);
    }
}
```

Je kan nu meerdere clients tegelijk starten. Zolang ze allemaal maar op dezelfde poort (1111 in dit geval) verbinden kunnen ze berichten naar de server sturen.

## Multicasting

**Multicasting** laat het toe om over een netwerk te communiceren naar groepen van willekeurige grootte via een enkele transmissie door de bron. Men kan gecontroleerd data versturen naar een aantal (maar niet noodzakelijk alle) gebruikers. Hierdoor worden bijvoorbeeld televisie-uitzendingen via internet haalbaar vanuit een bron die zelf weinig bandbreedte ter beschikking heeft (men kan dus vanuit huis of met een beperkt budget zenden). Gebruikers moeten zich inschrijven op een multicastgroep om de datapakketten die hiernaar verzonden worden, te kunnen ontvangen. Als men niet meer wenst gebruik te maken van een bepaalde multicastgroep, kan men zich hiervoor uitschrijven. Gebruikers kunnen zich tegelijkertijd voor verscheidene multicastgroepen inschrijven. Om data te verzenden naar een multicastgroep is inschrijving echter niet vereist.

Alternatieven voor multicast zijn:

- **Unicast**: het verzenden van een pakket naar één host. De normale gang van zaken.
- **Broadcast**: het verzenden van een pakket naar alle hosts op een gegeven netwerk.
- **Anycast**: het verzenden van een pakket naar de dichtstbijzijnde host van een bepaalde klasse.

## WireShark

https://www.wireshark.org/

We installeren WireShark om TCP/UDP communicatie te bekijken: [eavesdropping](https://en.wikipedia.org/wiki/Eavesdropping)



