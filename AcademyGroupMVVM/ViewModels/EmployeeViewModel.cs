using AcademyGroupMVVM.Models;

namespace AcademyGroupMVVM.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        private Employee employee;

        public EmployeeViewModel(Employee e)
        {
            employee = e;
        }

        public string FirstName
        {
            get { return employee.FirstName!; }
            set
            {
                employee.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return employee.LastName!; }
            set
            {
                employee.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public int Age
        {
            get { return employee.Age; }
            set
            {
                employee.Age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public string Position
        {
            get { return employee.Position; }
            set
            {
                employee.Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public string GroupName
        {
            get { return employee.Company.Name; }
        }
    }
}
