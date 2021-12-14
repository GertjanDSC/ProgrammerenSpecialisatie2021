## Dependency inversion principle

> High level modules mogen niet afhankelijk zijn van low level modules. Ze mogen enkel afhankelijk zijn van abstractie.

> Depend in the direction of abstraction. High level modules should not depend upon low level details.

### Wat zijn dependencies?

> Een dependency is iets dat misschien kan veranderen tijdens de levenscyclus van je code.

Stel je voor dat je muis, keyboard, monitor door de fabrikant aan je moederbord zijn gesoldeerd zijn. Met andere woorden, wanneer je een moederbord koopt heb je ook een muis, keyboard en monitor. Stel je voor je met één van de devices vervangen. Je beschadigt misschien het moederboard.

In programmeren is het net hetzelfde. Het dependency Inversion principe is een manier om plugs aan je code toe te voegen zodat de high level modules (moederbord) onafhankelijk is van de low level modules (muis). De low level modules kunnen later ontwikkeld worden en zouden makkelijk vervangbaar moeten zijn.

Een indicatie die veranderingen te weeg brengen is het "new" keywoord.

Een praktische oplijsting van veel voorkomende dependencies:

- Third party library
- Database
- File system
- Web Service
- New keyword

Je zou er voor moeten zorgen dat constructors alle dependencies bevat die een klasse nodig heeft. Dit noemen we EXPLICIT DEPENDENCIES. In het andere geval noemen we het hidden dependencies.

Dependency injection is een techniek om een dependency (afhankelijkheid) te injecteren in een klasse wanneer deze klasse ze nodig heeft.

Een voorbeeld:

```csharp
class EventLogWriter
{
    public void Write(string message)
    {
        //Write to event log here
    }
}

class Printer
{
    // Handle to EventLog writer to write to the logs
    EventLogWriter writer = null;

    // This function will be called when the app pool has problem
    public void Notify(string message)
    {
        if (writer == null)
        {
            writer = new EventLogWriter();
        }
        writer.Write(message);
    }
}
```

Op het eerste zicht is er niets mis met bovenstaande code. Maar eigenlijk schenden we DIP. De high level module 'Printer' is afhankelijk van de klasse EventLogWriter. Deze klasse noemen we een concrete klasse, en is dus geen abstracte klasse.

Het wordt duidelijker als we ook een email naar de IT admin willen sturen bij een bepaald probleem en niet enkel een log neerschrijven.

Als we een klasse schrijven voor het versturen van emails, moet de Printer klasse het juiste kunnen afhandelen, zonder dat wij concrete code implementeren. Dus nu is de Printer klasse afhankelijk van de EventLogWriter klasse, en hiervan willen we vanaf.

Onderstaande code laat zien hoe je verandering op een ondynamische manier injecteert:

```csharp
class EventLogWriter
{
    public void Write(string message)
    {
        //Write to event log here
    }
}

class EmailLogWriter
{
    public void Send(string message)
    {
        //Send email
    }
}

class Printer
{

    EventLogWriter writer = null;
    EmailLogWriter email = null;


    public void Notify(string message, string type)
    {
        if (type == "EventViewer")
        {
            if (writer == null)
            {
                writer = new EventLogWriter();
            }
            writer.Write(message);
        }

        if (type == "email")
        {
            if (email == null)
            {
                email = new EmailLogWriter();
            }
            email.Send(message);
        }
    }
}
```

Dus onze printer klasse moet een instantie van al onze loggers bijhouden. Volgens het dependency inversion principe moeten we ons systeem ontkoppelen zodat de higher level modules, dus de Printer module afhankelijk is van een abstracte klasse of interface. Deze abstractie zal gemapt worden (polymorf gedrag) naar een concrete klasse die de juiste actie zal ondernemen.

```csharp
interface INotification
{
    void Notify(string message);
}

class EventLogWriter:INotification
{
    public void Notify(string message)
    {
    }

}

class EmailLogWriter:INotification
{
    public void Notify(string message)
    {

    }
}

class Printer
{   
    INotification writer;

    public printer(INotification w){
        writer = w;
    }
    public void Notify(string message)
    {

        writer.Notify(message);
    }
}
```

Op deze manier maken we de Printer klasse (High level module) onafhankelijk van de concrete log klassen.

Hoe kunnen we deze verder ontkoppelen zodat we bij het toevoegen van andere log klassen (vb. SMS logger), de printer klasse niet meer aangepast hoeft te worden?

Dit kan je implementeren door Dependency injectie

## Dependency Injection

Dependency Injection betekent dat we een concrete implementatie in een klasse injecteren, met als doel om de koppeling tussen klassen te verminderen.

3 manieren voor Dependency injection:

- Constructor injection
- Method injection
- Property injection

### Contructor injection

Constructor Injection

We geven het object van de concrete klasse mee met de constructor van de afhankelijke klasse.

```csharp
class Printer
{
    INotification writer;
    public Printer(INotification logger)
    {
        writer = logger;
    }

    public void Notify(string message)
    {            
        writer.Notify(message);
    }
}
static void Main(string[] args)
{

    EventLogWriter log = new EventLogWriter();
    Printer p = new Printer(log);
    p.Notify("dit is een test");
}
```

### Method Injection

In constructor injection wordt de concrete klasse gedurende de volledige levenscyclus van de Printer gebruikt. Als je verschillende concrete klassen moet aanroepen, moet je deze in de methode zelf injecteren.

```csharp
static void Main(string[] args)
{
    EventLogWriter log = new EventLogWriter();
    Printer p = new Printer();
    p.Notify(log,"dit is een test");
}
```

### Property Injection

We geven het object van de concrete klasse mee via een set property.

```csharp
class Printer
{
    // Handle to EventLog writer to write to the logs
    public INotification writer { get; set; }

    // This function will be called when the app pool has problem
    public void Notify(INotification logger, string message)
    {
        writer = logger;
        writer.Notify(message);
    }
}
 static void Main(string[] args)
{
    EventLogWriter log = new EventLogWriter();
    Printer p = new Printer();
    p.writer = log;
    p.Notify("dit is een test");

}
```

### Een tweede voorbeeld

Bepaal de dependencies:

```csharp
public class Order
{
    public void Checkout(Cart cart, PaymentDetails paymentDetails, bool notifyCustomer)
    {
        if (paymentDetails.PaymentMethod == PaymentMethod.CreditCard)
        {
            ChargeCard(paymentDetails, cart);
        }

        ReserveInventory(cart);

        if (notifyCustomer)
        {
            NotifyCustomer(cart);
        }
    }

    public void NotifyCustomer(Cart cart)
    {
        string customerEmail = cart.CustomerEmail;
        if (!String.IsNullOrEmpty(customerEmail))
        {
            using (var message = new MailMessage("orders@somewhere.com", customerEmail))
            using (var client = new SmtpClient("localhost"))
            {
                message.Subject = "Your order placed on " + DateTime.Now;
                message.Body = "Your order details: \n " + cart;

                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Logger.Error("Problem sending notification email", ex);
                    throw;
                }
            }
        }
    }

    public void ReserveInventory(Cart cart)
    {
        foreach (OrderItem item in cart.Items)
        {
            try
            {
                var inventorySystem = new InventorySystem();
                inventorySystem.Reserve(item.Sku, item.Quantity);
            }
            catch (InsufficientInventoryException ex)
            {
                throw new OrderException("Insufficient inventory for item " + item.Sku, ex);
            }
            catch (Exception ex)
            {
                throw new OrderException("Problem reserving inventory", ex);
            }
        }
    }

    public void ChargeCard(PaymentDetails paymentDetails, Cart cart)
    {
        using (var paymentGateway = new PaymentGateway())
        {
            try
            {
                paymentGateway.Credentials = "account credentials";
                paymentGateway.CardNumber = paymentDetails.CreditCardNumber;
                paymentGateway.ExpiresMonth = paymentDetails.ExpiresMonth;
                paymentGateway.ExpiresYear = paymentDetails.ExpiresYear;
                paymentGateway.NameOnCard = paymentDetails.CardholderName;
                paymentGateway.AmountToCharge = cart.TotalAmount;

                paymentGateway.Charge();
            }
            catch (AvsMismatchException ex)
            {
                throw new OrderException("The card gateway rejected the card based on the address provided.", ex);
            }
            catch (Exception ex)
            {
                throw new OrderException("There was a problem with your card.", ex);
            }
        }
    }
}
```

De dependencies opgelijst:

- MailMessage
- SmtpClient
- Inventory
- PaymentGateway

Hoe moeten we dit refactoren?

1. Dependencies in interfaces stoppen
2. Injecteer de implementatie van deze interface in de order klasse
3. Zorg voor Single Responsible principle

Het toepassen van dependency injection zorgt typisch voor heel wat interfaces die ergens moeten geinstantierd worden. Dit doen we meestal in de default constructor of in de Main (de startup routine van je applicatie)

De MailMessage en SmtpClient zorgt voor een eventuele verandering. Stel je wil later de klant niet via email een notificatie sturen, maar via facebook messenger, dan zal je deze code moeten aanpassen. Door dit in interface te duwen, zal je veel flexibelere code schrijven:

```csharp
 public interface INotifyCustomer
{
    void NotifyCustomer(Cart cart);
}
public class NotifyCustomerService : INotifyCustomer
{
    /**
    * Method Notifies the customer via Email
    * @param cart a cart object to mail all cart details
    */
    public void NotifyCustomer(Cart cart)
    {
        string customerEmail = cart.CustomerEmail;
        if (!String.IsNullOrEmpty(customerEmail))
        {
            using (var message = new MailMessage("orders@somewhere.com", customerEmail))
            using (var client = new SmtpClient("localhost"))
            {
                message.Subject = "Your order placed on " + DateTime.Now;
                message.Body = "Your order details: \n " + cart;

                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Logger.Error("Problem sending notification email", ex);
                    throw;
                }
            }
        }
    }
}
```

In de order klasse zie je de flexibiliteit terugkomen:

```csharp
 public class Order
{
    INotifyCustomer _notifier;

    public Order(INotifyCustomer notification)
    {
        _notifier = notification;

    }
    public void Checkout(Cart cart, PaymentDetails paymentDetails, bool notifyCustomer)
    {           

        if (notifyCustomer)
        {
            _notifier.NotifyCustomer(cart);
        }
    }
```

Op deze manier moet de Order klasse niet weten of we een email notificatie sturen, en push notification voor mobile phone, een facebook message, ...

# Strategy patroon

We gaan verder met het voorbeeld van Single Responsibility op *method*-niveau.

### De conditionele logica van PrijsCode veranderen met behulp van Polymorfisme

#### Overerving

Omdat we verschillende types films hebben, die elk een verschillende GetCharge() implementatie hebben, kan je beter gebruik maken van subclasses. Op die manier is het zeer onderhoudbaar om nieuwe types toe te voegen (OPC Principe). Indien je niet met subclasses werkt, zal je in verschillende functies aanpassingen moeten doen: dit leidt tot bugs en inefficiëntie..

Klassediagramma:

![klassediagram](https://timdams.gitbooks.io/csharpfromantwerp/content/assets/20_se/klassediagram.PNG)

Op deze manier heb je het switch statement (getCharge() functie) in de klasse Movie niet meer nodig!

```csharp
switch (GetPriceCode()) 
{ 
    case Movie.REGULAR: 
        result += 2; 
        if (daysRented > 2) 
        { 
            result += (daysRented - 2) * 1.5; 
        } 
        break; 

    case Movie.NEW_RELEASE: 
        result += daysRented * 3; 
        break; 

    case Movie.CHILDRENS: 
        result += 1.5; 
        if (daysRented > 3) 
        { 
            result += (daysRented - 3) * 1.5; 
        } 
        break; 
}
```

Maar elke subklasse zal een implementatie maken van getCharge(). Voorbeeldcode:

```csharp
public abstract class Movie 
{ 
    private string _title; 

    public Movie(string title)  
    { 
        _title = title; 
    } 

    public string GetTitle() 
    { 
        return _title; 
    } 

    public abstract double GetCharge(int daysRented); 
}

class NewReleaseMovie : Movie 
{ 
    public NewReleaseMovie(string title) : base(title) { } 

    public override double GetCharge(int daysRented) 
    { 
        return  daysRented * 3; 
    } 
}
```

Aan deze implementatie zit wel een zeer groot nadeel. Het is niet mogelijk om een film van classificatie te wisselen. Dus stel dat je de film “Rundskop” maakt als een NewReleaseMovie, dan is het later niet mogelijk om van deze film een RegularMovie te maken.

```csharp
Movie m2 = new NewReleaseMovie ("Rundskop");
```

Indien je nu van m2 een RegularMovie wil maken, zal je de variabele opnieuw moeten instantiëren, met als gevolg dat je alle originele data kwijt bent.

```csharp
m2 = new RegularMovie ("Rundskop");
```

Met andere woorden, onze objecten kunnen niet van klasse veranderen gedurende hun levenstijd. Er is wel een oplossing voor, namelijk gebruik maken van het strategy Patroon.

![strategy](https://timdams.gitbooks.io/csharpfromantwerp/content/assets/20_se/strategy.PNG)

In plaats van te subklassen met Movie (RegularMovie, NewReleaseMovie, ChildrensMovie,) gaan we dit op een indirecte manier doen en van price subklassen maken. (zie klassediagramma hierboven). Op die manier kunnen we van een movie object steeds de prijs veranderen, zonder dat we het ganse movie object opnieuw moeten instantiëren.

Het strategy pattern is dus in staat om een status/ of om in realtime algoritme te veranderen ( bijvoorbeeld prijs van een film veranderen na een week van New Release naar Regular) bij te houden. Door vele programmeurs wordt dit initieel via enum en switch/case structuur opgezet. Het state patroon wordt aangemaakt door de type code naar state klassen over te brengen. Daarna gaan we de conditionele logica (switch/case) naar de price klassen zetten, om tenslotte deze switch case te verwijderen.
De strategy klassen

```csharp
public abstract class Price 
{ 
    public abstract double getCharge(int daysRented); 

} 

class RegularPrice : Price 
{ 
    public override double getCharge(int daysRented) 
    { 

        double result = 2; 
        if (daysRented > 3) 
            result += (daysRented - 2) * 1.5; 

        return result; 
    } 
} 

public  class ChildrensPrice : Price 
{ 

    public override double getCharge(int daysRented) 
    { 
        double result = 1.5; 
        if (daysRented > 3) 
            result += (daysRented - 3) * 1.5; 

        return result; 
    } 
} 


public class NewReleasePrice:Price 
{ 
    public override double getCharge(int daysRented) 
    { 
        return daysRented * 3; 
    } 

    public override int getFrequentRenterPoints(int daysRented) 
    { 
        return (daysRented > 1) ? 2 : 1; 
    } 
}
```

We maken nu een abstracte klasse price met zijn subklassen. De getCharge() functie in de superklasse is abstract, zodat de afgeleide klassen verplicht zijn een eigen implementatie te geven aan deze functie. In de Movie klasse zal je in plaats van de int _priceCode een member van het type Price moeten definiëren. In runtime beslis je dan of deze member de constructor van NewReleasePrice, ChildrensPrice of RegularPrice oproept.

```csharp
private Price _priceCode; 

public Movie(string title, Price priceCode) 
{ 
    _title = title; 
    _priceCode = priceCode; 
} 

public Price GetPriceCode() 
{ 
    return _priceCode; 
} 

public void SetPriceCode(Price arg) 
{ 
    //is dit juist? copyconstructor..? 
    _priceCode = arg; 
}
```

De volgende stap is om de getCharge() functie in de klasse Movie over te dragen naar de juiste Price klasse. (de implementatie van getCharge() in de desbetreffende klassen kan je reeds zien op de vorige pagina). De getCharge() functie van de Movie klasse wordt dan:

```csharp
public double GetCharge(int daysRented) 
{ 
    return _priceCode.getCharge(daysRented);             
}
```

Hierbij is ook de conditionele logica in de functie verdwenen (want deze is overgedragen naar de Price klassen).

We kunnen dit nu ook doen voor de getFrequentRenterPoints(int daysRented). We moeten deze functie in de superclasse ‘Price’ niet abstract maken maar geven er hier reeds een implementatie aan. We maken deze wel virtual zodat de afgeleide kunnen bepalen of ze er een eigen definitie aan geven.

```csharp
public abstract class Price 
{ 
    public abstract double getCharge(int daysRented); 
    public virtual int getFrequentRenterPoints(int daysRented) 
    { 
        return 1; 
    }           
}
```

Enkel de NewReleasePrice geeft een eigen implementatie aan de getFrequentRenterPoints(int daysRented) functie:

```csharp
public class NewReleasePrice:Price 
{ 
    public override double getCharge(int daysRented) 
    { 
        return daysRented * 3; 
    } 

    public override int getFrequentRenterPoints(int daysRented) 
    { 
        return (daysRented > 1) ? 2 : 1; 
    } 
}
```

Dit strategy patroon invoegen bezorgde ons heel wat werk, maar de winst is dat we prijzen gemakkelijk kunnen wijzigen en nieuwe prijsklassen kunnen aanmaken zonder andere code te wijzigen, want de rest van de implementatie weet niets van dit *state pattern*! Voor grote complexe projecten betekent dat heel wat winst.

Alle bovenstaande veranderingen moeten leiden tot makkelijker te onderhouden code. Een heel verschil met proceduraal programmeren, maar wanneer je dit onder knie hebt, zal je veel gemakkelijker tests kunnen schrijven en veranderingen implementeren!
