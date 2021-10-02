# Null Coalescing

Coalescing betekent samenvoegen. In het geval van Null Coalescing betekent dit dat, indien een null waarde toegekend zou worden aan een variabele, dat we er een “fallback”-waarde aan toekennen. 

Voorbeeld: 

```c#
string info = "Account Info: "; 
string name = "Carlton Banks"; 
string accountNumber = null; // has no account, so no number 
info += name; 
if (accountNumber == null) 
    info += ""; 
else 
    info += accountNumber; 
Console.WriteLine(info); 
```

Indien accountNumber gelijk is aan null, dan moet de string "" worden afgebeeld. We kunnen hier Null Coalescing toepassing om de code beknopter te maken: 

```c#
string info = "Account Info: "; 
string name = "Carlton Banks"; 
string accountNumber = null; // has no account, so no number 
info += name; 
info += accountNumber ?? ""; 
Console.WriteLine(info); 
```

De Null Coalescing operator is een dubbel vraagteken (het ?? symbool) dat zich rechts bevindt van de waarde die gecontroleerd wordt op null en links van de waarde die teruggegeven wordt indien de linkse waarde null is.