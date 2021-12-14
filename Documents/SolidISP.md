# Interface Segregation Principle (ISP)

> Clients mogen niet elementen van een interface moeten implementeren die ze niet gebruiken

Clients mogen niet gedwongen worden om interfaces te implementeren die operaties bevatten die ze niet nodig hebben, of nooit zullen gebruiken. Deze interfaces noemen we ***fat interfaces***.

In plaats van een *fat interface* is het beter om deze verder op te delen in meer specifieke interfaces.

```csharp
public interface ILog
{
    void Log(string message);
    void OpenConnection();
    void CloseConnection();
}

public class DBLogger : ILog
{
    public void Log(string message)
    {
        //Code to log data to a database
    }

    public void OpenConnection()
    {
        //Opens database connection
    }

    public void CloseConnection()
    {
        //Closes the database connection
    }

}

public class FileLogger : ILog
{
    public void Log(string message)
    {
        //Code to log to a file           
    }

    public void CloseConnection()
    {
        throw new NotImplementedException();
    }

    public void OpenConnection()
    {
        throw new NotImplementedException();
    }

}
```

## Hoe ISP oplossen:

```csharp
public interface ILog
{
    void Log(string message);     
}

public interface IDBLog :ILog
{
    void OpenConnection();
    void CloseConnection();
}

public interface IFileLog :ILog
{
    void CheckFileSize();
    void GenerateFileName();
}

public class DBLogger : IDBLog
{
    public void Log(string message)
    {
        //Code to log data to a database
    }

    public void OpenConnection()
    {
        //Opens database connection
    }

    public void CloseConnection()
    {
        //Closes the database connection
    }

}

public class FileLogger : IFileLog
{
    public void Log(string message)
    {
        //Code to log data to a database
    }
    public void CheckFileSize() { }

    public void GenerateFileName() { }
}
```

Er zijn verschillende redenen waarom interfaces gesegregeerd moeten zijn:

- Functionaliteit naar clients te verstoppen
- Zelf documenterend zijn voor andere ontwikkelaars.

## Hoe ISP detecteren?

1. Implementeer je sommige methodes van een interface niet, dan voldoe je niet aan ISP.
2. Als je klasse refereert naar een andere klasse, maar gebruikt in beperkte mate deze gerefereerde klasse, dan voldoe je niet aan ISP.

Kom je code tegen zoals bovenstaande: if( b is Pinguin) ... else , is dit een teken dat je niet aan ISP voldoet, en bijgevolg je code flexibeler moet schrijven.
Kom je code tegen waarbij methoden afgeleid van een basis klasse niet geïmplementeerd worden: vb.

```csharp
public void AddLuggage() 
{
    throw new NotSupportedException("No room to carry luggage, sorry."); 
}
```
