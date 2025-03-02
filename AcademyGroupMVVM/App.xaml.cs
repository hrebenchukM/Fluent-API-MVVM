using AcademyGroupMVVM.Models;
using AcademyGroupMVVM.ViewModels;
using System.Data;
using System.Windows;

namespace AcademyGroupMVVM
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
               

                using (var db = new CompanyContext())
                {
                    Company company1 = new Company { Name = "Luxoft" };
                    Company company2 = new Company { Name = "Rozetka" };
                    Company company3 = new Company { Name = "DataArt" };
                    db.Companies?.Add(company1);
                    db.Companies?.Add(company2);
                    db.Companies?.Add(company3);
                    db.Employees?.Add(new Employee { FirstName = "Богдан", LastName = "Иваненко", Age = 20, Position = "Fullstack developer", Company = company1 });
                    db.Employees?.Add(new Employee { FirstName = "Анна", LastName = "Шевченко", Age = 23, Position = "Front-end developer", Company = company2 });
                    db.Employees?.Add(new Employee { FirstName = "Петро", LastName = "Петренко", Age = 25, Position = "Backend developer", Company = company3 });
                    db.Employees?.Add(new Employee { FirstName = "Елена", LastName = "Артемьева", Age = 42, Position = "Manual QA Engineer", Company = company1 });
                    db.Employees?.Add(new Employee { FirstName = "Елена", LastName = "Алексеева", Age = 47, Position = "Automation QA Engineer", Company = company2 });
                    db.Employees?.Add(new Employee { FirstName = "Виктория", LastName = "Бабенко", Age = 29, Position = "Team Lead", Company = company3 });

                    db.SaveChanges();

                    var groups = from g in db.Companies
                                 select g;
                    var students = from st in db.Employees
                                   select st;
                    MainWindow view = new MainWindow();
                    MainViewModel viewModel = new MainViewModel(groups, students);
                    view.DataContext = viewModel;
                    view.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
