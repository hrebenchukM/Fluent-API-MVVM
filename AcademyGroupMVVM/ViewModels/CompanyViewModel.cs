using AcademyGroupMVVM.Models;
namespace AcademyGroupMVVM.ViewModels
{
    public class CompanyViewModel : ViewModelBase
    {
        private Company Company;

        public CompanyViewModel(Company company)
        {
            Company = company;
        }

        public string Name
        {
            get { return Company.Name!; }
            set
            {
                Company.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
}
