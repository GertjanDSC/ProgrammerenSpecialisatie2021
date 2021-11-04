# WPF Command Pattern: een inleiding

## Maak een class die interface ICommand implementeert

```c#
using KlantBestellingen.WPF.ViewModels;
using Serilog;
using System;
using System.Windows.Input;

namespace KlantBestellingen.WPF.Commands
{
    public class NieuwProductCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ProductenViewModel _source;

        public NieuwProductCommand(ProductenViewModel source)
        {
            _source = source;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Log.Information("-> NieuwProductCommand::Execute");
            Log.Information("<- NieuwProductCommand::Execute");
        }
    }
}

```

- Plaats class in een subdirectory "Commands" en pas namespace naam aan
  - Betere leesbaarheid

  ## ViewModel

  ```c#
  public class ProductenViewModel : INotifyPropertyChanged
  {
          public ICommand BtnNieuwProductCommand { get; set; }
  
          public event PropertyChangedEventHandler PropertyChanged;
  
          public ProductenViewModel()
          {
              BtnNieuwProductCommand = new NieuwProductCommand(this);
          }
  }
  ```

  ## Trigger update
  
  ```c#
   CommandManager.InvalidateRequerySuggested();
  ```
  
  of:
  
  ```c#
  using System;
  using System.Windows.Input;
  
  namespace MVVMBase
  {
    public class DelegateCommand : ICommand
    {
      // These delegates store methods to be called that contains the Execute and CanExecue methods
      // for each particular instance of DelegateCommand.
      private readonly Predicate<object> _canExecute;
      private readonly Action<object> _Execute;
  
      //Two Constructors, for convenience and clean code - often you won't need CanExecute
      public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
      {
        _canExecute = canExecute;
        _Execute = execute;
      }
  
      public DelegateCommand(Action<object> execute)
        : this (execute, null)
      { 
      }
  
      //CanExecute and Execute come from ICommand
      public event EventHandler CanExecuteChanged;
  
      public bool CanExecute(object parameter)
      {
        return _canExecute == null ? true : _canExecute(parameter);
      }
  
      public void Execute(object parameter)
      {
        if (!CanExecute(parameter))
          return;
  
        _Execute(parameter);
      }
  
      /// <summary>
      /// Not a part of ICommand, but commonly added so you can trigger a manual refresh on the result of CanExecute.
      /// </summary>
      public void RaiseCanExecuteChanged()
      {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
      }
    }
  }
  ```
  
  

## Specificeer Command in xaml

```xml
<Button x:Name="BtnNieuwProduct" Command="{Binding Path=BtnNieuwProductCommand}" />
```

## CanExecute() in Execute()

Controleer of je wel mag uitvoeren.