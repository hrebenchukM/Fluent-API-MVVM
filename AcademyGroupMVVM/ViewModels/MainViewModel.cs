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
        public ObservableCollection<EmployeeViewModel> EmployeesList { get; set; }

        public MainViewModel(IQueryable<Company> companies, IQueryable<Employee> employees)
        {
            CompaniesList = new ObservableCollection<CompanyViewModel>(companies.Select(c => new CompanyViewModel(c)));
            EmployeesList = new ObservableCollection<EmployeeViewModel>(employees.Select(e => new EmployeeViewModel(e))); 
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

        private int index_selected_groups = -1;

        public int Index_selected_groups
        {
            get { return index_selected_groups; }
            set
            {
                index_selected_groups = value;
                OnPropertyChanged(nameof(Index_selected_groups));
            }
        }

        private int index_selected_students = -1;

        public int Index_selected_students
        {
            get { return index_selected_students; }
            set
            {
                index_selected_students = value;
                OnPropertyChanged(nameof(Index_selected_students));
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

        private DelegateCommand refreshGroupCommand;

        public ICommand RefreshGroupCommand
        {
            get
            {
                if (refreshGroupCommand == null)
                {
                    refreshGroupCommand = new DelegateCommand(param => RefreshGroup(), null);
                }
                return refreshGroupCommand;
            }
        }

        private void RefreshGroup()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var groups = from g in db.Companies
                                 select g;
                    CompaniesList = new ObservableCollection<CompanyViewModel>(groups.Select(g => new CompanyViewModel(g)));
                    OnPropertyChanged(nameof(CompaniesList));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DelegateCommand refreshStudentCommand;

        public ICommand RefreshStudentCommand
        {
            get
            {
                if (refreshStudentCommand == null)
                {
                    refreshStudentCommand = new DelegateCommand(param => RefreshStudent(), null);
                }
                return refreshStudentCommand;
            }
        }

        private void RefreshStudent()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var groups = from g in db.Companies
                                 select g;
                    var students = from st in db.Employees
                                   select st;
                    CompaniesList = new ObservableCollection<CompanyViewModel>(groups.Select(g => new CompanyViewModel(g)));
                    EmployeesList = new ObservableCollection<EmployeeViewModel>(students.Select(st => new EmployeeViewModel(st)));
                    OnPropertyChanged(nameof(CompaniesList));
                    OnPropertyChanged(nameof(EmployeesList));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DelegateCommand addGroupCommand;

        public ICommand AddGroupCommand
        {
            get
            {
                if (addGroupCommand == null)
                {
                    addGroupCommand = new DelegateCommand(param => AddGroup(), param => CanAddGroup());
                }
                return addGroupCommand;
            }
        }

        private DelegateCommand removeGroupCommand;

        public ICommand RemoveGroupCommand
        {
            get
            {
                if (removeGroupCommand == null)
                {
                    removeGroupCommand = new DelegateCommand(param => RemoveGroup(), param => CanRemoveGroup());
                }
                return removeGroupCommand;
            }
        }

        private DelegateCommand updateGroupCommand;

        public ICommand UpdateGroupCommand
        {
            get
            {
                if (updateGroupCommand == null)
                {
                    updateGroupCommand = new DelegateCommand(param => UpdateGroup(), param => CanUpdateGroup());
                }
                return updateGroupCommand;
            }
        }

        private void AddGroup()
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

        private bool CanAddGroup()
        {
            return !CompanyName.IsNullOrEmpty();
        }

        private void RemoveGroup()
        {
            try
            {
                var delcompany = CompaniesList[Index_selected_groups];
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

        private bool CanRemoveGroup()
        {
            return Index_selected_groups != -1;
        }

        private void UpdateGroup()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var updatecompany = CompaniesList[Index_selected_groups];
                    var query = (from g in db.Companies
                                 where g.Name == updatecompany.Name
                                 select g).Single();
                    query.Name = CompanyName;
                    db.SaveChanges();
                    CompaniesList[Index_selected_groups] = new CompanyViewModel(query);
                    MessageBox.Show("IT-Компания обновлена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private bool CanUpdateGroup()
        {
            return !CompanyName.IsNullOrEmpty() && Index_selected_groups != -1;
        }

        private DelegateCommand addStudentCommand;

        public ICommand AddStudentCommand
        {
            get
            {
                if (addStudentCommand == null)
                {
                    addStudentCommand = new DelegateCommand(param => AddStudent(), param => CanAddStudent());
                }
                return addStudentCommand;
            }
        }

        private void AddStudent()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var company = CompaniesList[Index_selected_groups];
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
                    EmployeesList.Add(employeeviewmodel);

                    MessageBox.Show("Работник добавлен!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanAddStudent()
        {
            return !FirstName.IsNullOrEmpty() && !LastName.IsNullOrEmpty()  && !Position.IsNullOrEmpty() && Index_selected_groups != -1;
                
        }

        private DelegateCommand removeStudentCommand;

        public ICommand RemoveStudentCommand
        {
            get
            {
                if (removeStudentCommand == null)
                {
                    removeStudentCommand = new DelegateCommand(param => RemoveStudent(), param => CanRemoveStudent());
                }
                return removeStudentCommand;
            }
        }

        private void RemoveStudent()
        {
            try
            {
                var delemployee = EmployeesList[Index_selected_students];
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
                    EmployeesList.Remove(delemployee);
                    MessageBox.Show("Работник удален!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanRemoveStudent()
        {
            return Index_selected_students != -1;
        }

        private DelegateCommand updateStudentCommand;

        public ICommand UpdateStudentCommand
        {
            get
            {
                if (updateStudentCommand == null)
                {
                    updateStudentCommand = new DelegateCommand(param => UpdateStudent(), param => CanUpdateStudent());
                }
                return updateStudentCommand;
            }
        }

        private void UpdateStudent()
        {
            try
            {
                using (var db = new CompanyContext())
                {
                    var company = CompaniesList[Index_selected_groups];
                    var query = (from g in db.Companies
                                 where g.Name == company.Name
                                 select g).Single();
                    var updateemployee = EmployeesList[Index_selected_students];
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
                    EmployeesList[Index_selected_students] = new EmployeeViewModel(employee);
                    MessageBox.Show("Данные о работнике изменены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanUpdateStudent()
        {
            return !FirstName.IsNullOrEmpty() && !LastName.IsNullOrEmpty() &&
                !Position.IsNullOrEmpty() && Index_selected_groups != -1 && Index_selected_students != -1;

        }
    }
}
