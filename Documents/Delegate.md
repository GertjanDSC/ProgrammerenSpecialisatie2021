# Delegate

Een delegate is als een verbeterde klassieke pointer. Het is type-veilig en heeft een specifieke methode handtekening nodig. Delegates kunnen methodes aanroepen, dus kunnen ze gebruikt worden voor events.
Om een delegate te declareren voeg je gewoon het woord "delegate" toe voor een methode definitie.

```c#
// methods
private double Multiply(double a, double b) { return a * b; }
private double Add(double a, double b) { return a + b; }
 
// a delegate for any of the above methods
private delegate double dCalc(double a, double b);
```

Wijs een methode toe aan een delegate en gebruik dan de delegate om de methode aan te roepen.

```c#
void Delegates1() {
    dCalc calc = Add; // creates a new delegate
    Console.WriteLine("Sum " + calc(6.0, 3.0));
 
    calc = Multiply; // creates a new delegate
    Console.WriteLine("Product " + calc(6.0, 3.0));
 
    Console.ReadLine();
} //
```

```text
example output:
Sum 9
Product 18
```

Je kunt operatoren gebruiken om delegates toe te voegen of te verwijderen. Het volgende voorbeeld toont dat een delegate naar meerdere methodes kan verwijzen. Dit kan, omdat delegates erven van de klasse System.MulticastDelegate, die op haar beurt erft van System.Delegate.

```c#
private delegate void dPrint();
 
private void PrintA() { Console.WriteLine("Print A"); }
private void PrintB() { Console.WriteLine("Print B"); }
 
void Delegates2() {
    Console.WriteLine("--------------------------------");
    dPrint print = PrintA;
    print += PrintB;
    Console.WriteLine("added A and B");
    print();
 
    Console.WriteLine("--------------------------------");
    print -= PrintA;
    Console.WriteLine("removed A");
    print();
 
    Console.ReadLine();
} //
```

```text
example output:
——————————–
added A and B
Print A
Print B
——————————–
removed A
Print B
```

Delegates hebben iterators. Je kunt ze doorlopen en/of tellen. Nu zou je gemakkelijk moeten begrijpen dat delegates niets te maken hebben met pointers (zoals b.v. gebruikt in C++). Ze wijzen niet zomaar ergens naar toe. Een delegate is een complexe klasse. En daarbovenop hebben we te maken met multicast delegates.

```c#
private delegate void dPrint();
 
private void PrintA() { Console.WriteLine("Print A"); }
private void PrintB() { Console.WriteLine("Print B"); }
 
void Delegates3() {
    dPrint print = PrintA;
    print += PrintB;
    print += PrintB;
    print();
    Console.WriteLine("number of invokes: " + print.GetInvocationList().GetLength(0));
    foreach (Delegate d in print.GetInvocationList()) {
        Console.WriteLine(d.Method.Name + "()");
    }
 
    Console.ReadLine();
} //
```

```text
example output:
Print A
Print B
Print B
number of invokes: 3
PrintA()
PrintB()
PrintB()
```

Er zijn hier enkele woorden te leren:

Covariantie beschrijft een terugkeerwaarde die meer afgeleid is dan gedefinieerd in de delegate.
Contravariantie beschrijft een methode met parameters die minder afgeleid zijn dan die gedefinieerd in de delegatie.
Invariant beschrijft een generieke typeparameter die noch covariant noch contravariant is.
Variantie Covariantie en Contravariantie worden samen variantie genoemd.

Covariantie lijkt op standaard polymorfisme, terwijl contravariantie contra-intuïtief lijkt. Wat belangrijk is om te onthouden is dat een covariante type parameter kan gebruikt worden als het return type van een delegate, en contravariante type parameters kunnen gebruikt worden als parameter types.

Contravariantie werd geïntroduceerd in C# 4. Broncode die in C# 3.5 niet compileerde, compileert en draait nu heel goed onder C# 4. Contravariantie is zinvol voor write-only methods. Dus de input levert geen output op van dezelfde hiërarchie.

Covariantie voorbeeld:

```c#
class A { public int dummyA = 0; }
class B : A { public double dummyB = 0.0; }
 
delegate A dCovariance();
 
B Test() { return new B(); }
 
void Delegates4() {
    dCovariance covariance = Test;  // test returns class B, not class A as defined in the delegate
 
    foreach (Delegate d in covariance.GetInvocationList()) {
        Console.WriteLine(d.Method.Name + "()");
    }
 
    Console.ReadLine();
} //
```

```text
example output:
Test()
```

Contravariantie voorbeeld:

```c#
class A { public int dummyA = 0; }
class B : A { public double dummyB = 0.0; }
 
void Test2(A xParameter) { return; }
 
delegate void dContravariance(B xClassB);
 
void Delegates5() {
    // The parameter in Test2()  of type class "A", but the
    // delegate was defined with a class "B" parameter.
    dContravariance contravariance = Test2;
 
    foreach (Delegate d in contravariance.GetInvocationList()) {
        Console.WriteLine(d.Method.Name + "()");
    }
 
    contravariance(new B());  // <= important !!!!
 
    Console.ReadLine();
} //
```

```text
example output:
Test2()
```

Bij het aanroepen van contravariance(...) moeten we een klasse B instantie als parameter doorgeven. En de methode Test2() heeft hier uiteraard geen problemen mee. Klasse B is meer afgeleid. Het is nu wel logisch dat Test2() minder afgeleid kan zijn, nietwaar?

Een veelgebruikte contravariante interface is IComparer. Leun even achterover en kijk naar een aangepaste interface met de "in T" annotatie. 

```c#
interface IPerson<in T> {
    string Name { get; set; }
    int Age { get; set; }
}
```

Lambda

https://csharphardcoreprogramming.wordpress.com/2013/12/17/lambda-expressions-advanced/

Event