# Dynamic

![dlr-archoverview](C:\Users\u2389\source\repos\ProgrammerenSpecialisatie2021\Documents\dlr-archoverview.png)



```C#
object obj = "Luc";
var hc = obj.GetHashCode();
var methodInfo = obj.GetType().GetMethod("GetHashCode");
methodInfo.Invoke(null, null);        

dynamic name = "Luc";
name = 10; // ok, not an exception! Different runtime types ...
// name++; // generates exception though
dynamic a = 10;
dynamic b = 5;
var x = ""; // var is not dynamic
var c = a + b; // c is dynamic
int i = 5;
dynamic d = i;
```

## 