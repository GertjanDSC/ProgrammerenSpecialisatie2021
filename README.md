# Programmeren Specialisatie FLEX Semester 1 AVOND (2021 - 2022)

## 1. Planning

| Datum        | Inhoud                                                       |
| ------------ | ------------------------------------------------------------ |
| 28/09/2021   | [Kennismaking](./Documents/Kennismaking.md), afspraken en inhoudsbepaling, setup omgeving SQLServer 2019/SSMS, introductie Entity Framework Core |
| 05/10/2021   | [EF (1) - Db First, Code First](./Documents/EF_1_CodeFirst.md) |
| 12/10/2021   | [EF (2) - Code First](./Documents/EF_2.md)                   |
| 19/10/2021   | [EF (3) - CRUD, Change Tracker en Repository pattern](./Documents/EF_3.md) |
| 26/10/2021   | [Async, threads, tasks (1)](./Documents/Threading_1.md)      |
| 09/11/2021   | Design patterns: [WPF MVVM](./Documents/MVVM.md), [live charting](./Documents/LiveCharting.md) |
| 16/11/2021   | [Async, threads, tasks (2)](./Documents/Threading_2.md)      |
| 23/11/2021   | [TCP/UDP client/server](./Documents/SimpleTCP.md), [Protocol Buffers, encryptie, compressie](./Documents/Serialisatie.md), [WireShark](https://www.wireshark.org/download.html) |
| 30/11/2021   | [IOC](./Documents/ioc.md)                                    |
| 07/12/2021   | Design patterns: [DDD (Domain Driven Development) in de praktijk](./Documents/DDD.md) |
| 14/12/2021   | Design patterns: [SOLID in de praktijk](./Documents/SOLID.md) |
| *21/12/2021* | *Inhaalweek*                                                 |
| 11/01/2022   | Blok                                                         |
| 18/01/2022   | Eerste kans finale evaluatieoefening                         |
| 25/01/2022   | Blok                                                         |
| 01/02/2022   | Tweede kans finale evaluatieoefening                         |

* Git
  * [Cheat sheet](./Documents/GitCheatSheet.pdf)
  * Videos

## 2. SqlServer 2019

### 2.1. Optioneel: [Docker](./Documents/Docker.md)

### 2.2. Optioneel: [SQLServer met Docker](./Documents/SQLServer2019ViaDocker.md)

### 2.3. .NET 6

* Opvolger .NET Core 3.1 en tegelijk het einde van de "Frameworks", .NET Core, Mono, enzovoort - voorbij met de hoofdpijn! Door COVID-19 zal pas .NET 6 in november 2021 de unificatiebeweging volledig afronden, met .NET MAUI, de Universal UI, een evolutie van Xamarin.Forms, in preview en ondersteuning voor Android en iOS.
* https://dotnet.microsoft.com/download/dotnet/6.0
* Optioneel: [.NET 6 onder WSL2](./Documents/NET6onWSL2.md)

### 2.4. SQLServer 2019

### 2.5. SSMS

## 3. Tien geboden

1. VS2019 of **VS 2022 Enterprise Preview** (17.0.0 Preview 4.1 en hoger)
2. .NET 5.0 of **.NET 6.0** C#
3. Gebruik het **unit testing** framework xUnit
4. Geen console app, tenzij anders gevraagd; een console app mag wel bijkomend
5. Een **bestand per klasse**
6. **Method volledig zichtbaar** op je VS scherm
7. Unit testing **code coverage: >= 80%**
8. Voorzie je code van zinvolle **commentaar** en #region markeringen
9. Plaats je properties eerst
10. Gebruik een _ voor private variabelen

## 4. Entity Framework

1. Inleiding: video.
2. [Demo Database-First](./Documents/EF_1_DbFirstDemo.md)
3. [Demo Code-First](./Documents/EF_1_CodeFirst.md)
4. Database-First of Code-First? Video.
5. EF [attributes en Fluent API, LINQ](./Documents/EF_2.md)
6. [EF CRUD, Change Tracker en Repository pattern](./Documents/EF_3.md)
7. Opdrachten "EF tutorial": volg [tutorial](./Documents/1_EntityFrameworkCore_GetStarted.pdf), [tutorial](./Documents/2_EntityFrameworkCore_DataModelling.pdf) en [tutorial](./Documents/4_EntityFramework_CRUD.pdf) werk de EF attributen weg en dien de resultaten in onder [Chamilo](https://chamilo.hogent.be/index.php?go=CourseViewer&application=Chamilo%5CApplication%5CWeblcms&course=47725&tool=Assignment&browser=Table&tool_action=Display&publication=1875737).

## 5. Asynchroon programmeren

1. [Threads](./Documents/Threading_1.md)
2. [Timers, Tasks](./Documents/threading_2.md)

## 6. TCP/UDP Client/Server

1. [TCP/IP en C#: een inleiding](./Documents/SimpleTCP.md)
2. [Serialisatie, encryptie, compressie](./Documents/Serialisatie.md)

## 7. Design Patterns

### 7.1. WPF: MVVM

1. [Inleiding](./Documents/MVVM.md)

### 7.2. DDD (Domain Driven Development)

1.   [Design patterns: DDD (Domain Driven Development) in de praktijk](./Documents/DDD.md)

### 7.3. SOLID

2. [Inleiding](./Documents/SOLID.md)
3.   [IOC](./Documents/ioc.md)

## 8. Eindopdracht

* [Beschrijving](./Documents/Eindopdracht.md)
* [Uitwerking](./Documents/EindopdrachtUitwerking.md)
* [Logging](./Documents/SeriLog.md)

## 9. Aanvullende informatie

### C#

- [Null coalescing](./Documents/NullCoalescing.md)
- [Null conditional operator](./Documents/NullConditionalOperator.md)
- [Switch statement](./Documents/switch.md)
- [Delegates en events (oud)](./Documents/DelegatesEvents.pdf)
- [Delegates en events](./Documents/delegate.md)
- ADO .NET
  1. Installeer MySQL en SQLServer (docker)
  2. [Eerste stappen](./Documents/adonet1.md)
  3. [Transacties](./Documents/adonetTransactions.md)
  4. [Disconnected](./Documents/adonet3.md)

### WPF

#### Eerste stappen

- [Walkthrough](./Documents/WPF/WPFIntro.md)
- [Inleiding](./Documents/WPF/WPF_1_XAML.md)

#### Basis

- [Events](./Documents/WPF/WPF_2_Events.md)              
- [Application](./Documents/WPF/WPF_3_AppCommandLine.md)
- [Resources](./Documents/WPF/WPF_4_Resources.md)   
- [Exceptions](./Documents/WPF/WPF_5_Exceptions.md)
- [Basic Controls](./Documents/WPF/WPF_6_ControlsBasic.md): Button, TextBlock, TextBox
- [Layout Management](./Documents/WPF/WPF_7_LayoutManagement.md): Grid "basics"
- [List Controls](./Documents/WPF/WPF_11_ControlsList.md): DataGrid "basics"
- [Basic Controls](./Documents/WPF/WPF_6_ControlsBasic.md)
- [Menu and Status Bar](./Documents/WPF/WPF_13_MenuStatusBar.md)
- [Interfaces](./Documents/Interfaces1.md)
- [MaterialDesign (14:02)](https://www.youtube.com/watch?v=F0V01mYER5E&list=PLM3q9wWBZWb-_ZzoI8AFDxJRLYWTXDyYE&index=1)
- [Debugging (8:59)](https://www.youtube.com/watch?v=CHhgN5DoOMM&list=PLM3q9wWBZWb9ZkhEDkQLqQ43qtDSL_ANJ&index=1)
- [Debugging Binding Problems (8:21)](https://www.youtube.com/watch?v=gr4Ye8EvvU0&list=PLM3q9wWBZWb9ZkhEDkQLqQ43qtDSL_ANJ&index=2)
- [Debugging Binding Problems Revisited (3:37)](https://www.youtube.com/watch?v=TMpHLmDDwQo&list=PLM3q9wWBZWb9ZkhEDkQLqQ43qtDSL_ANJ&index=3)
- [Styles (4:54)](https://www.youtube.com/watch?v=kC9-Xow-aEg&list=PLM3q9wWBZWb9ZkhEDkQLqQ43qtDSL_ANJ&index=4)
- Vertaling en taalinstelling:
  - public class in Translations.resx
  - Per alternatieve taal: een bijkomend .resx bestand zonder code behind
  - Editeer je vertalingen via Visual Studio
  - In App.xaml.cs (vergelijk globale exception handler): 

```c#
Translations.Culture = new System.Globalization.CultureInfo("nl-BE"); // en-US nl-BE
```
#### Laatste loodjes

1. [ValueConverter](./Documents/WPF/WPF_9_ValueConverter.md)
2. [Advanced Controls](./Documents/WPF/WPF_10_ControlsAdvanced.md)   
3. [List Controls](./Documents/WPF/WPF_11_ControlsList.md)
4. [Styles](./Documents/WPF/WPF_12_Styles.md)            

<!-- 
## Introspection

1. [Reflection](./Documents/Reflection.md)
2. [Dynamic](./Documents/Dynamic.md)

## 10. OAuth 2.0 en OpenID Connect

* Principe van Kerckhoffs 
-->

# Notes

<!-- - GraphQL: REST, zie cursus Web4. https://cloud-trends.medium.com/grpc-vs-restful-api-vs-graphql-web-socket-tcp-sockets-and-udp-beyond-client-server-43338eb02e37 en https://blog.logrocket.com/why-you-shouldnt-use-graphql/. -->