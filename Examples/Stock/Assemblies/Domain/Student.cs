using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class Student
    {
        #region Properties
        public int Age { get; set; }
        public string FirstName { get; set; }
        #endregion

        #region Ctor
        public Student() // constructor
        {
            System.Diagnostics.Debug.WriteLine("Creating student " + FirstName);
            Age = 18;
        }
        #endregion       

        #region Dtor
        // vergelijk C++: destructor is altijd met tilde vooraan en verder als constructor; nooit public: niet van buiten af op te roepen
        ~Student() // destructor
        {
            System.Diagnostics.Debug.WriteLine("Destroying student " + FirstName);
        }
        #endregion
    }
}
