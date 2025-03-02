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
                   
                    var companies = from c in db.Companies
                                 select c;
                    var employees = from em in db.Employees
                                   select em;
                    MainWindow view = new MainWindow();
                    MainViewModel viewModel = new MainViewModel(companies, employees);
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
