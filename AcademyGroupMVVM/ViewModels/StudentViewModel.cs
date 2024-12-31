using AcademyGroupMVVM.Models;

namespace AcademyGroupMVVM.ViewModels
{
    public class StudentViewModel : ViewModelBase
    {
        private Student student;

        public StudentViewModel(Student st)
        {
            student = st;
        }

        public string FirstName
        {
            get { return student.FirstName!; }
            set
            {
                student.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return student.LastName!; }
            set
            {
                student.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public int? Age
        {
            get { return student.Age; }
            set
            {
                student.Age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public double? GPA
        {
            get { return student.GPA; }
            set
            {
                student.GPA = value;
                OnPropertyChanged(nameof(GPA));
            }
        }

        public string GroupName
        {
            get { return student.AcademyGroup.Name; }
        }
    }
}
