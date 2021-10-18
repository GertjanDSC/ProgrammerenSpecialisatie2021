# Entity Framework Core "Code First"

## Annotaties of Fluent API

![image-20211007094955428](./EFImages/EF_2_1.png)

Courses table

Description kolom: nvarchar(MAX), nullable

EF interpreteert dit op deze manier standaard. Wens je nullable == false?

Ofwel gebruik je dan een annotatie:

```plain
[Required] 
```

uit System.ComponentModel.DataAnnotations

Je voert vervolgens uit in Package Manager Console:

```plain
add-migration foo
```

Bekijk nu de migratie: 

```plain
nullable: false
```

We voeren deze niet uit met 

```plain
Update-Database 
```

en verwijderen de migratie: gewoon een "delete" van de migratie is voldoende omdat we deze nog niet uitvoerden.

Ofwel: "Fluent API"

ga naar context, methode OnModelCreating(). 

```C#
modelBuilder.Entity<Course>(). Property(t => t.Description).IsRequired();
```

Je kan methods "chainen".

```plain
add-migration foo
```

Bekijk de migratie: je ziet hetzelfde resultaat.

Wat is nu het beste? 

Annotaties: 

- minder code
- minder flexibel, vb. voor het configureren van relaties tussen klassen
- je klassen worden vervuild met databankkennis. Annotaties als "foreign key" zijn databankspecifiek. "Separation of Concern" is minder goed
- voor een eenvoudige applicatie zijn annotaties nog verdedigbaar. 
- ix de twee benaderingen echter niet, want dan wordt je onderhoud veel moeilijker!

**Conclusie: beter Fluent API.**

Voorbeelden van Annotations:

```C#
[Table("tbl_Course")]

[Column("sName", TypeName="varchar")]

[Key] [DatabaseGenerated(DatabaseGeneratedOption.None)] : None, Identity, Computed

[MaxLength(255)]

[Index(IsUnique=true)] of [Index("IX_AuthorStudentsCount", 1)]

[ForeignKey("Author")]
```

## Fluent API

![image-20211007100415602](./EFImages/EF_2_2.png)

![image-20211007100521404](./EFImages/EF_2_3.png)

![image-20211007100454051](./EFImages/EF_2_4.png)

![image-20211007100606891](./EFImages/EF_2_6.png)

... een anoniem object.

![image-20211007100716714](./EFImages/EF_2_7.png)

![image-20211007100754700](./EFImages/EF_2_8.png)

![image-20211007100831691](./EFImages/EF_2_9.png)

![image-20211007100909273](./EFImages/EF_2_10.png)

![image-20211007100937839](./EFImages/EF_2_11.png)

![image-20211007100957767](./EFImages/EF_2_12.png)

**Voor relaties is Fluent API superieur!**

Voor het leggen van relaties kan je in twee richtingen werken tussen twee types.

![image-20211007101205177](./EFImages/EF_2_13.png)

![image-20211007101233338](./EFImages/EF_2_14.png)

![image-20211007101301366](./EFImages/EF_2_15.png)

![image-20211007101417078](./EFImages/EF_2_16.png)

![image-20211007101516116](./EFImages/EF_2_17.png)

![image-20211007101548066](./EFImages/EF_2_18.png)

![image-20211007101640036](./EFImages/EF_2_19.png)

... of:

![image-20211007101727202](./EFImages/EF_2_20.png)

## LINQ voor het opvragen van gegevens

### Language Integrated Query

Een programmeermodel van de hand van Microsoft - brug tussen twee werelden: programmeren, gegevens. Mits een zogenaamde LINQ Provider kan je eender welke gegevensbron ondervragen:

![image-20211010105337776](./EFImages/EF_2_21.png)



![image-20211010105359220](./EFImages/EF_2_22.png)

![image-20211010105423571](./EFImages/EF_2_23.png)

#### Voordelen

* Geen SQL queries en stored procedures meer ...
* Verhoogde productiviteit: dezelfde "taal" en interface

#### Nadelen

![image-20211010105531499](./EFImages/EF_2_24.png)

Bij grote databanken en wanneer je met ingewikkelde queries moet werken, is SQL met stored procedures beter, handiger, sneller.

### Werken met LINQ

Zie [LINQ overzicht](./Linq.md).

### LINQPad en Entity Framework

Installeer de meest recente versie van LINQPad:

![LINQPad](./linqpad1.png)

Voor voorbeelden die uitvoerbaar zijn via LINQPad, zie gecomprimeerd bestand [LinqPadExamples.zip](./LinqPadExamples.zip).

Pas je DbContext code aan zodat deze optimaal geschikt is om te gebruiken vanuit LINQPad, gebaseerd op volgend voorbeeld:

```C#
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Sample.Data
{
	// This class demonstrates how to write constructors on your DbContext class such that:
	//
	//   (a) it's friendly to LINQPad, ASP.NET Core, Visual Studio tools and Migrations
	//   (b) you get better ease and flexiblity in how/where you specify the connection string

	public class SampleDbContext : DbContext
	{
		string _connectionString;

		// The constructor that ASP.NET Core expects. LINQPad can use it too.
		public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }

		// This constructor is simpler and more robust. Use it if LINQPad errors on the constructor above.
		// Note that _connectionString is picked up in the OnConfiguring method below.
		public SampleDbContext(string connectionString) => _connectionString = connectionString;

		// This constructor obtains the connection string from your appsettings.json file.
		// Tell LINQPad to use it if you don't want to specify a connection string in LINQPad's dialog.
		public SampleDbContext ()
		{
			IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			_connectionString = config.GetConnectionString("DefaultConnection");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Assign _connectionString to the optionsBuilder:
			if (_connectionString != null)				
				optionsBuilder.UseSqlServer(_connectionString);    // Change to UseSqlite if you're using SQLite

			// Recommended: uncomment the following line to enable lazy-loading navigation hyperlinks in LINQPad:
			// if (InsideLINQPad) optionsBuilder.UseLazyLoadingProxies ();
			// (You'll need to add a reference to the Microsoft.EntityFrameworkCore.Proxies NuGet package, and
			//  mark your navigation properties as virtual.)

			// Recommended: uncomment the following line to enable the SQL trace window:
			// if (InsideLINQPad) optionsBuilder.EnableSensitiveDataLogging (true);
		}

		// This property indicates whether or not you're running inside LINQPad:
		internal bool InsideLINQPad => AppDomain.CurrentDomain.FriendlyName.StartsWith("LINQPad");
	}

	// This is just For Visual Studio design-time support and Migrations (LINQPad doesn't use it).
	// Include this class if you want to specify a different connection string when using Visual Studio design-time tools.
	public class SampleDbContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<SampleDbContext>
	{
		public SampleDbContext CreateDbContext(string[] args)
			=> new SampleDbContext("...design-time connection string...");
	}
}
```

![image-20211010115206228](./EFImages/EF_2_25.png)

![image-20211010115309519](./EFImages/EF_2_26.png)

![image-20211010113052234](./EFImages/EF_2_27.png)

![image-20211010115354002](./EFImages/EF_2_28.png)

![image-20211010115413997](./EFImages/EF_2_29.png)

![image-20211010115442553](./EFImages/EF_2_30.png)

Je kan bijvoorbeeld "Courses" slepen op je editor:

![image-20211010115525135](./EFImages/EF_2_31.png)

En vervolgens uitvoeren:

![image-20211010115548302](./EFImages/EF_2_32.png)

Klik eens op de links in het resultaat: je kan verder doorklikken. Het is zelfs mogelijk om te bekijken welke queries Entity Framework uitvoert. Klikt hiervoor op tab "SQL" in plaats van tab "Results":

![image-20211010115645009](./EFImages/EF_2_33.png)

Deze mogelijkheden kunnen gebruikt worden om (LINQ) queries te optimaliseren! Je wijzigt de code en je ziet onmiddellijk wat de SQL queries worden. Je ziet ook de zogenaamde query execution time onder je "SQL" tab. 

Je kan eender welke C# code intikken en het resultaat zien: methode Dump(); Makkelijker dan het watch venster gebruiken in Visual Studio!

![image-20211010115847966](./EFImages/EF_2_34.png)

Probeer eens:

```c#
Courses.AsEnumerable().GroupBy(c => c.Level)
```

en bekijk de door Entity Framework gegeneerde SQL: een GroupBy() met LINQ is niet gelijk aan een "GROUP BY" in SQL.

![image-20211010120300568](./EFImages/EF_2_35.png)

Met oudere versies van Entity Framework kon je "AsEnumerable()" (of: ToList()) weglaten en dan werd een bijzonder ingewikkelde query gegenereerd. 

### SQLProfiler

Start "SSMS" (geef dit in in je zoekbalk) en kies voor Tools > SQL Profiler:

![image-20211010120712644](./EFImages/EF_2_36.png)

![image-20211010120755771](./EFImages/EF_2_37.png)

![image-20211010120810589](./EFImages/EF_2_38.png)

Voer je LINQPad LINQ query uit en bekijk hoe SQL Profiler toont welke queries er uitgevoerd worden op je databank:

![image-20211010120906642](./EFImages/EF_2_39.png)
