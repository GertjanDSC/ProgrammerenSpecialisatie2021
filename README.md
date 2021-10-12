# Programmeren Specialisatie FLEX Semester 1 AVOND (2021 - 2022)

## 1. Planning

| Datum      | Inhoud                                                       |
| ---------- | ------------------------------------------------------------ |
| 29/9/2021  | [Kennismaking](./Documents/Kennismaking.md), afspraken en inhoudsbepaling, setup omgeving SQLServer 2019/SSMS, introductie Entity Framework |
| 6/10/2021  | [EF (1) - Db First, Code First](./Documents/EF_1_CodeFirst.md) |
| 13/10/2021 | [EF (2) - Code First](./Documents/EF_2.md)                   |
| 20/10/2021 | EF (3) - Repository pattern                                  |
| 27/10/2021 | Async, threads, tasks (1)                                    |
| 3/11/2021  | Async, threads, tasks (2)                                    |
| 10/11/2021 | TCP/UDP client/server, Wireshark                             |
| 17/11/2021 | Design patterns: WPF MVVM                                    |
| 24/11/2021 | Design patterns: DDD (Domain Driven Development) in de praktijk |
| 1/12/2021  | Design patterns: SOLID in de praktijk (1)                    |
| 8/12/2021  | Design patterns: SOLID in de praktijk (2)                    |
| 15/12/2021 | Security: OAuth 2.0 en OpenID Connect (IdentityServer, OpenIddict) |
| 5/1/2022   | Blok                                                         |
| 12/1/2022  | Eerste kans finale evaluatieoefening                         |
| 19/1/2022  | Blok                                                         |
| 26/1/2022  | Tweede kans finale evaluatieoefening                         |

Tussendoortjes:

* Oefening workflow "Code first"
* Events/Delegates: een herhaling
* GraphQL
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
5. EF [attributes en Fluent API, LINQ; eindopdracht: muntwaarden](./Documents/EF_2.md)
6. Opdracht "EF tutorial": volg [tutorial](./Documents/1_EntityFrameworkCore_GetStarted.pdf) en [tutorial](./Documents/2_EntityFrameworkCore_DataModelling.pdf), werk de EF attributen weg en dien het resultaat in onder [Chamilo](https://chamilo.hogent.be/index.php?go=CourseViewer&application=Chamilo%5CApplication%5CWeblcms&course=47725&tool=Assignment&browser=Table&tool_action=Display&publication=1875737).

## 5. Asynchroon programmeren

## 6. TCP/UDP Client/Server

1. [TCP/IP en C#: een inleiding](./Documents/SimpleTCP.md)

## 7. Design Patterns

### 7.1. WPF: MVVM

1. [Inleiding](./Documents/MVVM.md)

### 7.2. DDD (Domain Driven Development)

### 7.3. SOLID

2. [Inleiding](./Documents/SOLID.md)

## 8. OAuth 2.0 en OpenID Connect

* Principe van Kerckhoffs

## 9. Reflection

- [Reflection](./Documents/Reflection.md)
- [Dynamic](./Documents/Dynamic.md)

## 10. Eindopdracht

* [Beschrijving](./Documents/Eindopdracht.md)

## 11. Aanvullende informatie

1. [Null coalescing](./Documents/NullCoalescing.md)
2. [Null conditional operator](./Documents/NullConditionalOperator.md)
3. [Switch statement](./Documents/switch.md)
4. ADO .NET
   1. Installeer MySQL en SQLServer (docker)
   2. [Eerste stappen](./Documents/adonet1.md)
   3. [Transacties](./Documents/adonetTransactions.md)
   4. [Disconnected](./Documents/adonet3.md)
5. WPF