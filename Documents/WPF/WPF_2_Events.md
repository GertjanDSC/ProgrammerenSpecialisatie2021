# Events in XAML (!)

De meeste Moderne UI frameworks zijn 'event driven' en dus ook WPF. Alle controls, inclusief  Window, die ook de Control class overerft, stelt een aantal events bloot waarop jij je kan abonneren. Je kan dit doen opdat je applicatie genotificeerd zou worden wanneer het event plaats grijpt zodat jij erop kan reageren.

Er zijn veel verschillende type events, maar sommige van de meest gebruikte reageren op gebruikersinteractie met jou applicatie door middel van muis en keyboard. De meeste controls bevatten events zoals KeyDown, KeyUp, MouseDown, MouseEnter, MouseLeave, MouseUp en andere.

Nu zien we hoe je een control event in XAML kan koppelen aan de code in je 'code-behind' bestand. Bestudeer het volgende voorbeeld:

```csharp
<Window x:Class="WpfTutorialSamples.XAML.EventsSample"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="EventsSample" Height="300" Width="300">
	<Grid Name="pnlMainGrid" MouseUp="pnlMainGrid_MouseUp" Background="LightBlue">
    </Grid>
</Window>
```

Zoals je ziet, hebben we ons geabonneerd op het MouseUp event van het Grid met de naam van een methode. Deze methode moet worden gedefinieerd in code-behind, met behulp van de juiste event-signatuur. In dit geval zou het er zo moeten uitzien:

```csharp
private void pnlMainGrid_MouseUp(object sender, MouseButtonEventArgs e)
{
	MessageBox.Show("You clicked me at " + e.GetPosition(this).ToString());
}
```

De MouseUp event gebruikt een delegate genaamd MouseButtonEventHandler, waarop jij je kan abonneren. Het heeft twee parameters, een 'sender' (de control die het event aanroept) en een MouseButtonEventArgs object dat bruikbare informatie bevat; we gebruiken het in het voorbeeld om de positie van de muiscursor te krijgen en weer te geven.

Verschillende events kunnen hetzelfde delegate type gebruiken. Het MouseUp en MouseDown event gebruiken allebei de MouseButtonEventHandler delegate, terwijl MouseMove event de MouseEventHandler delegate gebruikt. Tijdens het definiëren van je event handler methode moet je weten welke delegate het gebruikt en als je dat niet weet, dan kan je dat opzoeken in de documentatie.

Gelukkig kan Visual Studio ons helpen bij het genereren van een correcte event handler voor een event. De makkelijkste manier om dit toe doen, is door simpelweg de eventnaam in XAML te schrijven en de IntelliSense van VS de rest voor je te laten doen:

![wpf 2](media/vs-2019/WPF/vs_new_event.png)

**Hulp van Visual Studio om een nieuwe event handler te maken**

Als je <New Event Handler> selecteert zal Visual Studio een overeenkomstige event handler genereren in je 'code-behind' bestand. Deze zal <control name>_<event name> genoemd worden, pnlMainGrid_MouseDown in ons geval. Klik met de rechtermuistoets op de event naam en selecteer Navigate to Event Handler en VS zal je er meteen naartoe brengen.

## Abonneren op een event vanuit 'Code-behind'

De meest gangbare manier om je op events te abonneren werd hierboven uitgelegd, maar vaak zal je het abonneren eerder direct vanuit 'code-behind' doen. Dit doe je met behulp van de += C# syntax, waarmee je een event handler direct aan het object toekent. Hier volgt een voorbeeld:

```csharp
using System;
using System.Windows;
using System.Windows.Input;


namespace WpfTutorialSamples.XAML
{
	public partial class EventsSample : Window
	{
		public EventsSample()
		{
			InitializeComponent();
			pnlMainGrid.MouseUp += new MouseButtonEventHandler(pnlMainGrid_MouseUp);
		}

		private void pnlMainGrid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show("You clicked me at " + e.GetPosition(this).ToString());
		}

	}
}
```

Weerom moet je weten welke delegate er moet gebruikt worden en weerom kan Visual Studio je daarbij helpen. Als je schrijft:

pnlMainGrid.MouseDown +=

zal Visual Studio je deze hulp bieden:

![wpf 3](media/vs-2019/WPF/vs_new_event_cb.png)

**Visual Studio die je een nieuwe event handler helpt maken vanuit 'Code-behind'**

Druk simpelweg twee keer op de [Tab] toets om Visual Studio de juiste event handler te laten genereren. Deze komt juist onder de huidige method, klaar om in te vullen. Als je je op die manier op events abonneert, hoef je dit niet in XAML te doen.





