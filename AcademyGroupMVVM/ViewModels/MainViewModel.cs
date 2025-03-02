using AcademyGroupMVVM.Commands;
using AcademyGroupMVVM.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace AcademyGroupMVVM.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public ObservableCollection<CompanyViewModel> CompaniesList { get; set; }
        public ObservableCollection<EmployeeViewModel> AllEmployeesList { get; set; }
        public ObservableCollection<EmployeeViewModel> FilteredEmployeesList { get; set; }

        public ObservableCollection<EmployeeViewModel> EmployeesToDisplay
        {
            get
            {
                if (IsSearching)
                    return FilteredEmployeesList;
                else
                    return AllEmployeesList;
            }
        }

        public MainViewModel(IQueryable<Company> companies, IQueryable<Employee> employees)
        {
            CompaniesList = new ObservableCollection<CompanyViewModel>(companies.Select(c => new CompanyViewModel(c)));
            AllEmployeesList = new ObservableCollection<EmployeeViewModel>(employees.Select(e => new EmployeeViewModel(e)));
            FilteredEmployeesList = new ObservableCollection<EmployeeViewModel>(AllEmployeesList);
        }


        private bool isSearching;
        public bool IsSearching
        {
            get { return isSearching; }
            set
            {
                isSearching = value;
                OnPropertyChanged(nameof(IsSearching));
                OnPropertyChanged(nameof(EmployeesToDisplay));
            }
        }


        private string searchName;
        public string SearchName
        {
            get
            {
                return searchName;
            }
            set
            {
                searchName = value;
                OnPropertyChanged(nameof(SearchName));
            }
        }


        private string searchLastName;
        public string SearchLastName
        {
            get
            {
                return searchLastName;
            }
            set
            {
                searchLastName = value;
                OnPropertyChanged(nameof(SearchLastName));
            }
        }

        private string searchPosition;
        public string SearchPosition
        {
            get
            {
                return searchPosition;
            }
            set
            {
                searchPosition = value;
                OnPropertyChanged(nameof(SearchPosition));
            }
        }


        private string name;

        public string CompanyName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private int index_selected_companies = -1;

        public int Index_selected_companies
        {
            get { return index_selected_companies; }
            set
            {
                index_selected_companies = value;
                OnPropertyChanged(nameof(Index_selected_companies));
            }
        }

        private int index_selected_employees = -1;

        public int Index_selected_employees
        {
            get { return index_selected_employees; }
            set
            {
                index_selected_employees = value;
                OnPropertyChanged(nameof(Index_selected_employees));
            }
        }

        private string firstname;

        public string FirstName
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastname;

        public string LastName
        {
            get
            {
                return lastname;
            }
            set
            {
                lastname = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private int age;

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        private string pos;

        public string Position
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        private DelegateCommand searchEmployeeCommand;

        public ICommand SearchEmployeeCommand
        {
            get
            {
                if (searchEmployeeCommand == null)
                {
                    searchEmployeeCommand = new DelegateCommand(param => SearchEmployee(), null);
                }
                return searchEmployeeCommand;
            }
        }
        private void SearchEmployee()
        {
            try
            {
                IsSearching = true;
                using (var db = new CompanyContext())
                {
                    var employees = from e in db.Employees
                                    where (string.IsNullOrEmpty(SearchName) || e.FirstName.Contains(SearchName)) &&
                                          (string.IsNullOrEmpty(SearchLastName) || e.LastName.Contains(SearchLastName)) &&
                                          (string.IsNullOrEmpty(SearchPosition) || e.Position.Contains(SearchPosition))
                                    select e;
                    FilteredEmployeesList = new ObservableCollection<EmployeeViewModel>(employees.Select(e => new EmployeeViewModel(e)));
                    OnPropertyChanged(nameof(FilteredEmployeesList));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private DelegateCommand refreshCompanyCommand;

        public ICommand RefreshCompanyCommand
        {
            get
            {
                if (refreshCompanyCommand == null)
                {
                    refreshCompanyCommand = new DelegateCommand(param => RefreshCompany(), null);
                }
                return refreshCompanyCommand;
            }
        }

        private void RefreshCompany()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var companies = from g in db.Companies
                                 select g;
                    CompaniesList = new ObservableCollection<CompanyViewModel>(companies.Select(g => new CompanyViewModel(g)));
                    OnPropertyChanged(nameof(CompaniesList));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DelegateCommand refreshEmployeeCommand;

        public ICommand RefreshEmployeeCommand
        {
            get
            {
                if (refreshEmployeeCommand == null)
                {
                    refreshEmployeeCommand = new DelegateCommand(param => RefreshEmployee(), null);
                }
                return refreshEmployeeCommand;
            }
        }

        private void RefreshEmployee()
        {
            try
            {
                IsSearching = false;

                using (var db = new CompanyContext())
                {
                    var companies = from g in db.Companies
                                 select g;
                    var employees = from st in db.Employees
                                   select st;
                    CompaniesList = new ObservableCollection<CompanyViewModel>(companies.Select(g => new CompanyViewModel(g)));
                    AllEmployeesList = new ObservableCollection<EmployeeViewModel>(employees.Select(st => new EmployeeViewModel(st)));
                    OnPropertyChanged(nameof(CompaniesList));
                    OnPropertyChanged(nameof(AllEmployeesList));
                    OnPropertyChanged(nameof(FilteredEmployeesList));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DelegateCommand addCompanyCommand;

        public ICommand AddCompanyCommand
        {
            get
            {
                if (addCompanyCommand == null)
                {
                    addCompanyCommand = new DelegateCommand(param => AddCompany(), param => CanAddCompany());
                }
                return addCompanyCommand;
            }
        }

        private DelegateCommand removeCompanyCommand;

        public ICommand RemoveCompanyCommand
        {
            get
            {
                if (removeCompanyCommand == null)
                {
                    removeCompanyCommand = new DelegateCommand(param => RemoveCompany(), param => CanRemoveCompany());
                }
                return removeCompanyCommand;
            }
        }

        private DelegateCommand updateCompanyCommand;

        public ICommand UpdateCompanyCommand
        {
            get
            {
                if (updateCompanyCommand == null)
                {
                    updateCompanyCommand = new DelegateCommand(param => UpdateCompany(), param => CanUpdateCompany());
                }
                return updateCompanyCommand;
            }
        }

        private void AddCompany()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var company = new Company { Name = CompanyName };
                    db.Companies.Add(company);
                    db.SaveChanges();
                    var companyviewmodel = new CompanyViewModel(company);
                    CompaniesList.Add(companyviewmodel);
                    MessageBox.Show("IT-Компания добавлена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanAddCompany()
        {
            return !CompanyName.IsNullOrEmpty();
        }

        private void RemoveCompany()
        {
            try
            {
                var delcompany = CompaniesList[Index_selected_companies];
                DialogResult result = MessageBox.Show("Вы действительно желаете удалить IT-Компанию " + delcompany.Name +
                    " ?", "Удаление IT-Компании", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
                using (var db = new CompanyContext())
                {
                    var query = (from g in db.Companies
                                 where g.Name == delcompany.Name
                                select g).Single();
                    db.Companies.Remove(query);
                    db.SaveChanges();
                    CompaniesList.Remove(delcompany);
                    MessageBox.Show("IT-Компания удалена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanRemoveCompany()
        {
            return Index_selected_companies != -1;
        }

        private void UpdateCompany()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var updatecompany = CompaniesList[Index_selected_companies];
                    var query = (from g in db.Companies
                                 where g.Name == updatecompany.Name
                                 select g).Single();
                    query.Name = CompanyName;
                    db.SaveChanges();
                    CompaniesList[Index_selected_companies] = new CompanyViewModel(query);
                    MessageBox.Show("IT-Компания обновлена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private bool CanUpdateCompany()
        {
            return !CompanyName.IsNullOrEmpty() && Index_selected_companies != -1;
        }

        private DelegateCommand addEmployeeCommand;

        public ICommand AddEmployeeCommand
        {
            get
            {
                if (addEmployeeCommand == null)
                {
                    addEmployeeCommand = new DelegateCommand(param => AddEmployee(), param => CanAddEmployee());
                }
                return addEmployeeCommand;
            }
        }

        private void AddEmployee()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var company = CompaniesList[Index_selected_companies];
                    var query = (from g in db.Companies
                                 where g.Name == company.Name
                                 select g).Single();

                    var employee = new Employee
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        Age = age,
                        Position = pos,
                        Company = query
                    };
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    var employeeviewmodel = new EmployeeViewModel(employee);
                    AllEmployeesList.Add(employeeviewmodel);

                    MessageBox.Show("Работник добавлен!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanAddEmployee()
        {
            return !FirstName.IsNullOrEmpty() && !LastName.IsNullOrEmpty()  && !Position.IsNullOrEmpty() && Index_selected_companies != -1;
                
        }

        private DelegateCommand removeEmployeeCommand;

        public ICommand RemoveEmployeeCommand
        {
            get
            {
                if (removeEmployeeCommand == null)
                {
                    removeEmployeeCommand = new DelegateCommand(param => RemoveEmployee(), param => CanRemoveEmployee());
                }
                return removeEmployeeCommand;
            }
        }

        private void RemoveEmployee()
        {
            try
            {
                var delemployee = AllEmployeesList[Index_selected_employees];
                DialogResult result = MessageBox.Show("Вы действительно желаете удалить работника " + delemployee.LastName +
                    " ?", "Удаление работника", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
                using (var db = new CompanyContext())
                {
                    var query = from st in db.Employees
                                where st.LastName == delemployee.LastName
                                select st;
                    db.Employees.RemoveRange(query);
                    db.SaveChanges();
                    AllEmployeesList.Remove(delemployee);
                    MessageBox.Show("Работник удален!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanRemoveEmployee()
        {
            return Index_selected_employees != -1;
        }

        private DelegateCommand updateEmployeeCommand;

        public ICommand UpdateEmployeeCommand
        {
            get
            {
                if (updateEmployeeCommand == null)
                {
                    updateEmployeeCommand = new DelegateCommand(param => UpdateEmployee(), param => CanUpdateEmployee());
                }
                return updateEmployeeCommand;
            }
        }

        private void UpdateEmployee()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var company = CompaniesList[Index_selected_companies];
                    var query = (from g in db.Companies
                                 where g.Name == company.Name
                                 select g).Single();
                    var updateemployee = AllEmployeesList[Index_selected_employees];
                    if (query == null)
                        return;

                    var employee = (from st in db.Employees
                                   where st.LastName == updateemployee.LastName
                                  select st).Single();

                    employee.Company = query;
                    employee.FirstName = firstname;
                    employee.LastName = lastname;
                    employee.Age = age;
                    employee.Position = pos;
                    db.SaveChanges();
                    AllEmployeesList[Index_selected_employees] = new EmployeeViewModel(employee);
                    MessageBox.Show("Данные о работнике изменены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanUpdateEmployee()
        {
            return !FirstName.IsNullOrEmpty() && !LastName.IsNullOrEmpty() &&
                !Position.IsNullOrEmpty() && Index_selected_companies != -1 && Index_selected_employees != -1;

        }
    }
}
