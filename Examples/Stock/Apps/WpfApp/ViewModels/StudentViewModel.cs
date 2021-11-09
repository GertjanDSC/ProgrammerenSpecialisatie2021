using MVVM;
using Stock.Domain;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp.MVVM;

namespace WpfApp.ViewModels
{
    public class StudentsViewModel : ViewModelBase
    {
        // ObservableCollection is als een List<>, maar implementeert ook INotifyPropertyChanged; als er een element aan de lijst wordt toegevoegd of uit de lijst verdwijnt, zal WPF gewaarschuwd worden
        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>
        {
                new Student { FirstName = "Bruce", Age = 25 },
                new Student { FirstName = "Harry", Age = 35 },
                new Student { FirstName = "Stuart", Age = 50 },
                new Student { FirstName = "Robert", Age = 60 }
        };

        public string SelectedStudent { get; set; }

        private string _selectedName;
        public string SelectedName
        {
            get => _selectedName;
            set
            {
                // Voorkom nutteloze updates bij WPF door niets toe te kennen wanneer de waarde ongewijzigd is: veel sneller!
                if (_selectedName == value)
                {
                    return;
                }
                _selectedName = value;
                // Het doorgeven van het veld is niet meer nodig:
                RaisePropertyChanged(/*"SelectedName"*//*nameof(SelectedName)*/);
            }
        }

        private ICommand _updateStudentNameCommand;
        public ICommand UpdateStudentNameCommand
        {
            get => _updateStudentNameCommand;
            set => _updateStudentNameCommand = value;
        }

        // Om te tonen hoe je Mode=TwoWay gebruikt: wordt aangepast wanneer een student geselecteerd wordt
        private Student _selectedStudentItem;
        public Student SelectedStudentItem
        {
            get => _selectedStudentItem;
            set { if (_selectedStudentItem != value) { _selectedStudentItem = value; } }
        }

        public StudentsViewModel()
        {
            UpdateStudentNameCommand = new RelayCommand(o => SelectedStudentDetails(o), o => CanStudentBeShown());
        }

        // Wanneer het command uitgevoerd wordt:
        public void SelectedStudentDetails(object parameter)
        {
            if (parameter != null)
                SelectedName = (parameter as Student)?.FirstName;
        }

        // Criterium dat bruikbaarheid van knop bepaalt:
        public bool CanStudentBeShown()
        {
            return _selectedStudentItem != null;
        }
    }
}