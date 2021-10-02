# Reflection

Wikipedia zegt dat "In de computerwetenschap is 'reflectie' het proces waarbij een computerprogramma zijn eigen structuur en gedrag kan observeren en wijzigen". Dit is precies hoe Reflection in C# werkt, en terwijl je je dit hier nog niet zal realiseren, biedt het in staat zijn om informatie over je applicatie gedurende runtime te onderzoeken en te veranderen een gigantisch potentieel. Reflection, dat zowel een algemene term is als ook de actuele naam van de reflection mogelijkheden van C#, werkt erg erg goed, en is in feite niet moeilijk te gebruiken. In de volgende paar hoofdstukken gaan we dieper in op hoe het werkt en geven we een paar mooie voorbeelden die laten zien hoe nuttig Reflection is.

Maar om jouw in beweging te zetten en ervoor te interesseren, is hier een klein voorbeeld. Het lost een vraag op die ik heb gezien bij vele nieuwkomers bij elke programmeertaal: Hoe kan ik nu de waarde van een variabele gedurende runtime veranderen door alleen maar de naam ervan te weten? Kijk naar deze kleine demo applicatie voor een oplossing, en lees de volgende hoofdstukken voor uitleg over de verschillende technieken die worden gebruikt

```c#
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ReflectionTest
{
    class Program
    {
        private static int a = 5, b = 10, c = 20;

        static void Main(string[] args)
        {
            Console.WriteLine("a + b + c = " + (a + b + c));
            Console.WriteLine("Please enter the name of the variable that you wish to change:");
            string varName = Console.ReadLine();
            Type t = typeof(Program);
            FieldInfo fieldInfo = t.GetField(varName, BindingFlags.NonPublic | BindingFlags.Static);
            if(fieldInfo != null)
            {
                Console.WriteLine("The current value of " + fieldInfo.Name + " is " + fieldInfo.GetValue(null) + ". You may enter a new value now:");
                string newValue = Console.ReadLine();
                int newInt;
                if(int.TryParse(newValue, out newInt))
                {
                    fieldInfo.SetValue(null, newInt);
                    Console.WriteLine("a + b + c = " + (a + b + c));
                }
            }
        }
    }
}
```

## Type

De Type class is het fundament van Reflection. Hij dient als runtime informatie over een assembly, een module of een type. Gelukkig is een referentie naar het Type van een object erg eenvoudig, omdat elke class die erft (inherits) van de Object class een GetType() method heeft. Als je informatie nodig hebt over een niet-geinstantieerd type, mag je de globaal(globally) beschikbare typeof() method gebruiken, die daarvoor is. Bekijk de volgende voorbeelden waar we beide benaderingen gebruiken:

```c#
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ReflectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string test = "test";
            Console.WriteLine(test.GetType().FullName);
            Console.WriteLine(typeof(Int32).FullName);
        }
    }
}
```

We passen de GetType() method toe op onze eigen variabele, en daarna gebruiken we de typeof() op een bekende class, de Int32. Zoals je ziet is het resultaat in beide gevallen een Type object waarvoor we de Fullname property kunnen lezen.

Soms zou je ook alleen de naam kunnen hebben van het type waar je naar zocht. In dat geval moet je een referentie er naar toe krijgen van de juiste assembly. In het volgende voorbeeld krijgen we een referentie naar de uitvoerende assembly, dat wil zeggen, de assembly van waaruit de code wordt uitgevoerd, en daarna tonen we alle type ervan:

```c#
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ReflectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] assemblyTypes = assembly.GetTypes();
            foreach(Type t in assemblyTypes)
                Console.WriteLine(t.Name);
            Console.ReadKey();
        }
    }

    class DummyClass
    {
        //Just here to make the output a tad less boring :)
    }
}
```

De output zal de naam zijn van de twee gedeclareerde classes, Program en DummyClass, maar in een complexere applicatie is de lijst waarschijnlijk interessanter. In dit geval krijgen we alleen maar de naam van het type, maar uiteraard zouden we meer willen doen met de Type referentie die we krijgen. In de volgende hoofdstukken zal ik je wat meer laten zien van wat we ermee kunnen doen.

## Instantiatie van een class

```c#
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ReflectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Type testType = typeof(TestClass);
            ConstructorInfo ctor = testType.GetConstructor(System.Type.EmptyTypes);
            if(ctor != null)
            {
                object instance = ctor.Invoke(null);
                MethodInfo methodInfo = testType.GetMethod("TestMethod");
                Console.WriteLine(methodInfo.Invoke(instance, new object[] { 10 }));
            }
            Console.ReadKey();
        }
    }

    public class TestClass
    {
        private int testValue = 42;

        public int TestMethod(int numberToAdd)
        {
            return this.testValue + numberToAdd;
        }
    }
}
```

We hebben een simpele class gedefinieerd om dit te testen, met de naam TestClass. Hij omvat een private veld en een public method. De method retourneert de waarde van de private property, met de waarde van de parameter eraan toegevoegd. Wat we nu willen is een nieuw exemplaar van deze TestClass creëren, de testMethod oproepen en het resultaat naar de console sturen.

In dit voorbeeld hebben we de luxe om de typof() direct bij de TestClass te kunnen gebruiken, maar op een gegeven moment moet je het alleen doen door de naam van de gewenste class te gebruiken. In dat geval kun je er een referentie naartoe krijgen door de assembly waar hij is gedeclareerd.

Dus met een Type referentie naar de class, vragen we om de default constructor door gebruik van de GetConstructor() method, waarbij we System.Type.EmptyTypes als parameter stellen. Als we een specifieke constructor wilden, zouden we een array van Type's moeten leveren, waarvan elk definieert welke parameter de constructor, waar we naar zochten, zou nemen.

Eenmaal we een referentie hebben naar de constructor, kunnen we de Invoke() methode gebruiken om een nieuwe instantie van TestClass aan te maken. We geven null door als parameter omdat we geen specifieke parameters wensen door te geven. We gebruiken GetMethod() met de naam van de methode die we wensen op te roepen, om de TestMethod() functie te bekomen. Ook in dit geval kunnen we gebruik maken van de Invoke() methode. Deze keer moeten we wel een array van objecten meegeven. 



