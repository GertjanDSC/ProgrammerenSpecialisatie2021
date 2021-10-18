# 1. Inleiding

Om vlot met de eindopdracht te kunnen beginnen en eraan door te werken, wordt een basis aangereikt. Bekijk https://github.com/lucvervoort/ProgrammerenSpecialisatie2021/Examples/Stock.

# 2. ECB muntwaarden

```c#
// Call a method to get the exchange rate between two currencies: from, to
var exchangeRateUsd = CurrencyConverter.GetExchangeRate("usd", "eur", amount);
var exchangeRateChf = CurrencyConverter.GetExchangeRate("eur", "chf", amount);
```

Implementeer een klasse "CurrencyConverter" met static methodes.

```c#
/// <summary>
/// Available currency tags
/// </summary>
public static string[] GetCurrencyTags()
{
            // Hardcoded currency tags neccesairy to parse the ecb xml's
            return new string[] {"eur", "usd", "jpy", "bgn", "czk", "dkk", "gbp", "huf", "ltl", "lvl"
            , "pln", "ron", "sek", "chf", "nok", "hrk", "rub", "try", "aud", "brl", "cad", "cny", "hkd", "idr", "ils"
            , "inr", "krw", "mxn", "myr", "nzd", "php", "sgd", "zar"};
}
```

Implementeer een methode die currency rates ophaalt bij de ECB in een "canoniek" formaat: we opteren voor EURO.

ECB stelt muntkoersen beschikbaar in RDF/XML formaat: https://en.wikipedia.org/wiki/RDF/XML#:~:text=RDF%2FXML%20is%20a%20syntax,W3C%20standard%20RDF%20serialization%20format (Resource Description Framework, zie ook https://en.wikipedia.org/wiki/Resource_Description_Framework).

"Labeled, directed multi-graph": ![image-20211010125208662](C:\Users\u2389\source\repos\ProgrammerenSpecialisatie2021\Documents\EFImages\EF_2_40.png)

```c#
/// <summary>
/// Get currency exchange rate in euro
/// </summary>
public static decimal GetCurrencyRateInEuro(string currency)
{
  if (currency.ToLower().IsNullOrEmpty())
    throw new ArgumentException("Invalid argument: currency parameter must be specified.");
  if (currency.ToLower() == "eur")
    return (decimal)1.0;

  try
  {
    // Create with currency parameter, a valid RSS url to ECB euro exchange rate feed
    string rssUrl = string.Concat("http://www.ecb.int/rss/fxref-", currency.ToLower() + ".html");

    // Create & load new Xml document
    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
    doc.Load(rssUrl);

    // Create XmlNamespaceManager for handling XML namespaces
    System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager(doc.NameTable);
    nsmgr.AddNamespace("rdf", "http://purl.org/rss/1.0/");
    nsmgr.AddNamespace("cb", "http://www.cbwiki.net/wiki/index.php/Specification_1.1");

    // Get list of daily currency exchange rate between selected "currency" and the EURO
    System.Xml.XmlNodeList nodeList = doc.SelectNodes("//rdf:item", nsmgr);

    // Loop Through all XMLNODES with daily exchange rates
    foreach (System.Xml.XmlNode node in nodeList)
    {
      // Create a CultureInfo, this is because EU and USA use different sepperators in float (, or .)
      CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
      ci.NumberFormat.CurrencyDecimalSeparator = ".";

      try
      {
        // Get currency exchange rate with EURO from XMLNODE
        decimal exchangeRate = decimal.Parse(
          node.SelectSingleNode("//cb:statistics//cb:exchangeRate//cb:value", nsmgr).InnerText,
          NumberStyles.Any, 
          ci);

        return exchangeRate;
      }
      catch { }
    }
      
    // No result
    return (decimal)-1.0;
  }
  catch
  {   
    // No result
    return (decimal)-1.0;
  }
}
```

Deze code kan flink verbeterd worden:

* foutafhandeling: vang uitzonderingen op, controleer alle waarden die teruggegeven worden via methodes.
* voorzie logging (later, via SeriLog).

Implementeer vervolgens methode:

```C#
public static decimal GetExchangeRate(string from, string to, decimal amount = 1)
{
    // Fake:
    return 0.0;
}
```

Bedenk hierbij:

* let op alle randvoorwaarden: wat als de hoeveelheid 0 is of de muntcode is niet behoorlijk opgegeven.
* van "eur" naar "eur" is eenvoudig: er is geen herrekening nodig.
* als je "eur" omrekent naar een andere muntcode, dan moet je vermenigvuldigen en reken je een andere muntcode om naar "eur" dan moet je delen.
* wat als geen van beide muntcodes "eur" is? De rekenformule wordt ingewikkelder: (hoeveelheid * waarde_doelmunt) / waarde_vertrekmunt.

# 3. Koers van een aandeel ophalen met Yahoo API

De Yahoo Finance API is een bibliotheek (verzameling API's/methoden) om historische en real-time gegevens te verkrijgen voor een verscheidenheid van financiële markten en producten, zoals weergegeven op Yahoo Finance- https://finance.yahoo.com/.

### Waarom zou je de Yahoo Finance API gebruiken?

- Gratis
- Indrukwekkende reeks van gegevens
- Snel en gemakkelijk op te zetten
- Eenvoudig

**Indrukwekkend aanbod van gegevens**. Bovenop de standaard gegevens, biedt de Yahoo Finance API extra's zoals opties en fundamentele gegevens, alsmede marktnieuws en analyses, die alternatieven zoals IEX en Alpha Vantage niet altijd hebben.

Het is ook **gemakkelijk om zelf gebruik te maken van deze dienst**. Afhankelijk van de weg die je kiest, varieert de installatie van een paar regels code met een bibliotheek tot het aanmaken van een account om toegang te krijgen tot persoonlijke API-sleutels en vervolgens gewoon een API aan te roepen met een specifieke URL en deze sleutels.

**Het kan eenvoudig zijn**. Sommige bibliotheken hebben documentatie die op één pagina past, met behoud van functionaliteit die gericht, maar voldoende, is voor de meeste normale gebruikssituaties. Hoewel ze compact zijn, kunnen ze nog steeds van dienst zijn door functies te voorzien die veel moeizaam beenwerk voor je doen - zoals automatisch gegevens in data frames krijgen.

### Waarom zou je de Yahoo Finance API niet gebruiken?

Door bezuinigingsmaatregelen werd de API niet meer verder ondersteund tussen mei 2017 en 2019. Er is echter een nieuwe API ... .

### Alternatieven

- [IEX Cloud](https://iexcloud.io/)
- [Alphavantage](.https://www.alphavantage.co/)

